# Architecture

## Project structure

Each database and version has its own project and Nuget package:

- Quib.MySql56
- Quib.MySql57
- Quib.MySql80
- Quib.MySql84
- Quib.Postgres18

Each project also uses its own namespace to allow usage of multiple databases in the same application. Most of the code 
resides in a shared project (Quib.Shared). In order to assign the shared code to the correct database project, we use 
compiler directives and declare the namespace in each file like in this example: 

```csharp
#if MYSQL56
namespace Quib.MySql56.QueryBuilder;
#elif MYSQL57
namespace Quib.MySql57.QueryBuilder;
#elif MYSQL80
namespace Quib.MySql80.QueryBuilder;
#elif MYSQL84
namespace Quib.MySql84.QueryBuilder;
#elif POSTGRES18
namespace Quib.Postgres18.QueryBuilder;
#endif
```

Same goes for the unit test projects. Each database has its own unit test project like: 

- Quib.MySql56.UnitTests
- Quib.MySql57.UnitTests
- Quib.MySql80.UnitTests
- Quib.MySql84.UnitTests
- Quib.Postgres18.UnitTests

Again, most of the code resides in a shared project (Quib.Shared.UnitTests). But in this case they all use the same 
namespaces, so no compiler directives are needed.


## Namespaces

Namespaces are structured from the consumers perspective, grouped by typical use cases or tasks that the consumers will 
perform.

- `Quib.MySql80`: Root namespace with main entry into the library.
- `Quib.MySql80.Schema`: Types used to describe the db schema with tables, columns and datatypes. Used to build the metadata for your db queries.
- `Quib.MySql80.QueryBuilder`: Types used to build SQL DQL and DML queries.
- `Quib.MySql80.Expressions`: Types used to represent and build SQL expressions in queries and schemas.
- `Quib.MySql80.ORMapper`: Types used to process query results and map them to .NET objects and types.
- `Quib.MySql80.Installer`: Types used to install and migrate database schemas (Installer API).


## Types and Inheritance

- Whenever feasible, use structs to avoid heap allocations. But be aware of boxing. So in many cases, classes are the better choice.
- When the type holds other classes, especially lists, heap allocation is anyway required, so struct might not be appropriate.
- `IQueryPart` implementations may be structs if feasible, and no other interface prevents it.
- `QueryExpression<T>` implementations should be classes, as they would have to be boxed in expression trees anyway.


## SQL expression builder

SQL expressions are parts of a query that can be evaluated to a value. Like `users.name` or `users.age + 1`. The implementations
of SQL expressions reside in `Quib.Shared.Expressions`. Usage is typically through `ExpressionExtensions` extensions class
or `SQL` factory class.


## DB types for expressions

DB types are marker types used to represent the SQL types in expressions like `DbText` or `DbInt`. They are used to ensure 
type safety. They implement `IExpressionType` and `INotNullExpressionType`. All DB type structs reside in DbTypes.cs.

For every type there should be a corresponding nullable type like `DbTextNullable` or `DbIntNullable`. The nullable types 
implement `IExpressionType` and `INullableExpressionType` in addition.

DB types have a corresponding .NET value type like `string` for `DbText`, `string?` for `DbTextNullable` or `int` for `DbInt`.

You can take `DbInt`and `DbText` as reference implementations. Assume these are correct. But they could be lacking features.

### Fetch and mapping methods

- Add `Get` methods to the `ResultReader` class for every db type (and nullable). Use the existing methods as a reference.
- Add fetch methods to the `MappingExtensions` class for every db type (and nullable). Use the existing methods as a reference.

### Factory methods

In the static `SQL` class (/src/Quib.Shared/SQL.Factories.cs), there should be the following factory methods to create 
expressions for each db type:

- Parameter: `SQL.Parameter<T>(T value)` where T is the corresponding .NET value type of the db type.
- Nullable parameter: `SQL.NullableParameter<T>(T? value)` where T is the corresponding .NET value type of the db type.
- Constant: For numeric and boolean types only, create a `SQL.Constant<T>(T value)` where T is the corresponding .NET value type of the db type. The expression should be evaluated to the constant value in the SQL query like `1` or `true`.

### Expression extensions

For every type (and its nullable counterpart), there should be extensions in the static `ExpressionExtensions` class. 
Extensions for each type (and nullable) should be placed in individual files like `ExpressionExtensions.DbText.cs` or 
`ExpressionExtensions.DbTextNullable.cs`.

> **Note:** if expression implementations in /src/Quib.Shared/Expressions are missing, you can create them (see above). 
But check first if an existing implementation can be used.

**Cast methods:**

- Not-null db types should have a `AsNullable()` extension method to convert to the nullable counterpart.

**Comparison operators:**

All types should have comparison operators (`<`, `>`, `<=`, `>=`). If you think that a comparison operator is not 
feasible for a specific type, please ask for confirmation.

**Equality operators:**

The equality operators against db type expressions (`QueryExpression`) are not needed as they are implemented in the 
base class for all types.

But all types should have equality operators against the corresponding .NET value type (always the non-nullable .NET type, 
no matter if the DB type is nullable).

**Arithmetic operators:**

Implement arithmetic operator overloads that are feasible for the type. Numeric types should have `+`, `-`, `*`, `/` and `%`. 
Boolean types should have `&`, `|` and `!` operators. 

String types should have no arithemtic operators. Concatenation will be handeled otherwise and should not be done with 
the `+` operator

**For all operators:**

All binary operators should have overloads with the left-value always beeing the db type and variations for the right value: 

- The same db type (e.g. `DbText` or `DbTextNullable`)
- The nullable or not-null counterpart of the db type
- The corresponding non-nullable .NET value type (should be converted to parameter). Accepting the nullable type here would make no sense because the operator always evaluates NULL in that case. If you still want to do it, use an explicit cast.

### Unit tests

For every db type (including nullable), there should be a unit test file in /src/Quib.Shared.UnitTEsts/ExpressionTests. 
Examples are `DbTextExpressionTest` for `DbText`, `DbTextNullableExpressionTest` for `DbTextNullable` or 
`DbIntExpressionTest` for `DbInt`. You can use all existing unit tests as reference.

The unit tests should cover: 

- All factory methods. Always test with all input variants. E.g. for `DbTextNullable` the input could be either a text or null.
- All expression extensions. Always test with all input variants. E.g. `DbTextNullable` the input could be either a text or null.
- For nullable DB types, test the existing `SQL.Null.As<T>()` cast method.

Make sure the test methods are named and ordered in a comprehensive and consistent way. When ordering operator tests, 
use strict grouping by operator and then parameter kind in this exact order: not-null parameter, nullable parameter with 
value, nullable parameter with null, .NET value type.