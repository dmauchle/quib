# Architecture

## Project structure

Each database has its own project and Nuget package like:

- Quib.MySql
- Quib.Postgres

Each project also uses its own namespace to allow usage of multiple databases in the same application. Most of the code 
resides in a shared project (Quib.Shared). In order to assign the shared code to the correct database project, we use 
compiler directives and declare the namespace in each file like in this example: 

```csharp
#if MYSQL
namespace Quib.MySql.QueryBuilder;
#elif POSTGRES
namespace Quib.Postgres.QueryBuilder;
#endif
```

Same goes for the unit test projects. Each database has its own unit test project like: 

- Quib.MySql.UnitTests
- Quib.Postgres.UnitTests

Again, most of the code resides in a shared project (Quib.Shared.UnitTests). But in this case they all use the same 
namespaces, so no compiler directives are needed.


## Namespaces

Namespaces are structured from the consumers perspective, grouped by typical use cases or tasks that the consumers will 
perform.

- `Quib.MySql`: Root namespace with main entry into the library.
- `Quib.MySql.Schema`: Types used to describe the db schema with tables, columns and datatypes. Used to build the metadata for your db queries.
- `Quib.MySql.ORMapper`: Types used to process query results and map them to .NET objects and types.


## Types and Inheritance

- Whenever feasible, use structs to avoid heap allocations. But be aware of boxing. So in many cases, classes are the better choice.
- When the type holds other classes, especially lists, heap allocation is anyway required, so struct might not be appropriate.
- `IQueryPart` implementations may be structs if feasible, and no other interface prevents it.
- `IQueryExpression<T>` implementations should be classes, as they would have to be boxed in expression trees anyway.