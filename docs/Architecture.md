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
- `IQueryExpression<T>` implementations should be classes, as they would have to be boxed in expression trees anyway.