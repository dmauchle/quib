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
- `IQueryExpression` implementations should be classes, as they would have to be boxed in expression trees anyway.


## SQL expression builder

SQL expressions are parts of a query that can be evaluated to a value. Like `users.name` or `users.age + 1`. The implementations
of SQL expressions reside in `Quib.Shared.Expressions`. Usage is typically through `ExpressionExtensions` extensions class
or `SQL` factory class.


## Column types vs. DB types

Quib distinguishes between two kinds of datatypes:

- **Column types**: The SQL types of the columns in the db schema like `VARCHAR(50)`, `TEXT` or `INTEGER`. They are 
  only used by the Installer API (schema installation and migration). See "Column types" below.
- **DB types**: The .NET marker types used by the query builder to make expressions type safe like `DbText` or `DbInt`. 
  They reside in the `Quib.*.Types` namespace. See "DB types for expressions" below.

The mapping is not one-to-one: one DB type can be used for multiple column types. E.g. `DbText` is used for `VARCHAR`, 
`TEXT`, `MEDIUMTEXT`, `LONGTEXT` etc. When adding a new datatype, check both sides and add the column type and/or DB type 
as needed. Prefer reusing an existing DB type if it fits the column type.


## Column types

Column types are created by factory methods in `ColumnTypeFactory` (/src/Quib.Shared/Installer/ColumnTypeFactory.cs) and 
used like `ColumnType.VarChar(50)`. Use the existing methods as a reference.

Where a column type differs between databases (type name, options, limits, availability), the factory method is 
distinguished per database with compiler directives (`#if MYSQL` / `#elif POSTGRES`, or a version-specific symbol like 
`MYSQL56` if needed).

### Unit tests

For every column type (or group of column types, see below), there should be a unit test file in 
/test/Quib.Shared.UnitTests/SchemaTests like `VarCharTest`, `TextTest` or `IntTest`. The tests should cover:

- The column type itself: creating the column, limits and database-specific behavior.
- Writing and reading values with the corresponding DB type (not-null and nullable, including `NULL`): the setters in 
  `UpdateQueryExtensions`, the `Get` methods in `ResultReader` and the fetch methods in `MappingExtensions`. Generic 
  methods like `SetNull` don't need to be tested per type.

A DB type used by multiple column types (e.g. `DbText` for `VARCHAR` and `TEXT`) is therefore tested in each of these 
files. This duplication is intended. However, column types that only differ in size (e.g. `ShortText`, `MediumText` and 
`LongText`) share a test file, and it is sufficient to test the read and write methods with one of them (e.g. `ShortText`).


## DB types for expressions

DB types are marker types used to represent the SQL types in expressions like `DbText` or `DbInt`. They are used to ensure 
type safety. They implement `IExpressionType`. All DB type structs reside in /src/Quib.Shared/Types/DbTypes.cs.

DB types don't denote nullability. It's denoted by the expression class instead:

- `NotNullExpression<T>`: expression that never evaluates to NULL, e.g. `NotNullExpression<DbInt>`.
- `NullableExpression<T>`: expression that may evaluate to NULL, e.g. `NullableExpression<DbInt>`.

Note that not-null expressions may still become nullable for example when used in a LEFT JOIN.

DB types have a corresponding .NET value type like `string` for `DbText` or `int` for `DbInt`. Nullable expressions 
correspond to the nullable .NET type (`string?`, `int?`).

You can take `DbInt`and `DbText` as reference implementations. Assume these are correct. But they could be lacking features.

### Generic DB types

A DB type may have a type parameter if the same column type can be interpreted in different ways. Example: 
`DbDateTime<TTimeZone>`, where `TTimeZone` implements `IDateTimeStorage` (`Utc` or `LocalTime`) and defines the time zone of the 
stored values. The type parameter converts the values (static abstract interface members), so expressions with 
different type arguments can't be mixed. For generic DB types:

- Expression extensions use generic extension blocks, e.g. `extension<TTimeZone>(NotNullExpression<DbDateTime<TTimeZone>> value)`.
- `Get` methods, fetch methods and setters are generic methods with the same type parameter.
- Factory methods need the type argument explicitly, e.g. `SQL.Parameter<Utc>(value)`, as it can't be inferred from 
  the .NET value.
- Unit tests cover all type arguments for conversions and reading/writing. Type independent behavior (operators) may be 
  tested with one of them.

### Expression implementations

The expression implementations in /src/Quib.Shared/Expressions (like `BinaryOperator`, `QueryParameter` or `Constant`) 
are untyped and internal (implementing `IQueryExpression`). They are wrapped into a typed expression by passing them to 
the constructor of `NotNullExpression<T>` or `NullableExpression<T>`. The untyped expression of a typed one is accessible 
by its `SQL` property, e.g. `new NotNullExpression<DbInt>(new BinaryOperator(left.SQL, "+", right.SQL))`. So every 
implementation exists only once, no matter the type and nullability.

### Fetch, mapping and update methods

- Add `Get` methods to the `ResultReader` class for every db type (not-null and nullable). Use the existing methods as a reference.
- Add fetch methods to the `MappingExtensions` class for every db type (not-null and nullable). Use the existing methods as a reference.
- Add setters to the `UpdateQueryExtensions` class for every db type (not-null and nullable column). Use the existing methods as a reference.

These classes reside in /src/Quib.Shared/ORMapper. They are tested per column type (see "Column types" above).

### Factory methods

In the static `SQL` class (/src/Quib.Shared/SQL.Factories.cs), there should be the following factory methods to create 
expressions for each db type:

- Parameter: `SQL.Parameter<T>(T value)` where T is the corresponding .NET value type of the db type. Returns a not-null expression.
- Nullable parameter: `SQL.NullableParameter<T>(T? value)` where T is the corresponding .NET value type of the db type. Returns a nullable expression.
- Constant: For numeric and boolean types only, create a `SQL.Constant<T>(T value)` where T is the corresponding .NET value type of the db type. The expression should be evaluated to the constant value in the SQL query like `1` or `true`.

### Expression extensions

For every type, there should be extensions in the static `ExpressionExtensions` class. Extensions for each type should 
be placed in individual files like `ExpressionExtensions.DbText.cs`, with one extension block for `NotNullExpression<T>` 
and one for `NullableExpression<T>`. New files must be added to /src/Quib.Shared/Quib.Shared.projitems, otherwise they 
are not compiled.

> **Note:** if expression implementations in /src/Quib.Shared/Expressions are missing, you can create them (see above). 
But check first if an existing implementation can be used.

**Comparison operators:**

All types should have comparison operators (`<`, `>`, `<=`, `>=`). If you think that a comparison operator is not 
feasible for a specific type, please ask for confirmation.

**Equality operators:**

Equality operators between expressions of the same db type are implemented once in `NotNullExpression<T>` and 
`NullableExpression<T>` and must not be added per type. They're declared on the class (not as extension operators), 
because class operators take precedence over extension operators and C# would fall back to reference equality otherwise. 
Comparing different db types (e.g. `DbInt == DbText`) therefore doesn't compile, which is intended (it wouldn't make any 
sense and Postgres would also throw an exception).

All types should have equality operators against the corresponding .NET value type (always the non-nullable .NET type, 
no matter if the expression is nullable).

**Arithmetic operators:**

Implement arithmetic operator overloads that are feasible for the type. Numeric types should have `+`, `-`, `*`, `/` and `%`. 
Boolean types should have `&`, `|` and `!` operators. 

String types should have no arithmetic operators. Concatenation will be handled otherwise and should not be done with 
the `+` operator

**For all operators:**

All binary operators should have overloads in both extension blocks (not-null and nullable), with the left value always 
being the expression of the block and variations for the right value: 

- The same expression type (`NotNullExpression<T>` in the not-null block, `NullableExpression<T>` in the nullable block). 
  The result is not-null in the not-null block and nullable in the nullable block.
- The corresponding non-nullable .NET value type (should be converted to parameter). Accepting the nullable type here would make no sense because the operator always evaluates NULL in that case. If you still want to do it, use an explicit cast.

Operators mixing not-null and nullable operands must not be added: C# picks the nullable operator, as the not-null 
operand converts to its base class `NullableExpression<T>`. So the result is nullable, as expected.

### Unit tests

For every db type, there should be a unit test file for not-null and one for nullable expressions in 
/test/Quib.Shared.UnitTests/ExpressionTests. Examples are `DbTextExpressionTest` and `DbTextNullableExpressionTest` for 
`DbText`, or `DbIntExpressionTest` for `DbInt`. You can use all existing unit tests as reference.

The unit tests should cover: 

- All factory methods. Always test with all input variants. E.g. for `SQL.NullableParameter` the input could be either a text or null.
- All expression extensions. Always test with all input variants. E.g. for nullable expressions the input could be either a text or null.
- For nullable expressions, test the existing `SQL.Null<T>()` factory method.

Make sure the test methods are named and ordered in a comprehensive and consistent way. When ordering operator tests, 
use strict grouping by operator and then parameter kind in this exact order: not-null parameter, nullable parameter with 
value, nullable parameter with null, .NET value type.