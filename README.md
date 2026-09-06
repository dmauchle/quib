# Quib - SQL Query Builder for .NET Core

Yet another O/R mapper? Not exactly. This library follows a different approach. The main goal is type safety and 
catching as many errors at compile time as possible. To achieve this, Quib provides a type safe, fluent API to build SQL 
queries. This API abstracts database tables, columns, SQL types, SQL functions, etc., not domain objects. After fetching 
the rows from the database, another Quib API layer is used to map the results to .NET objects.


## How does it compare to other O/R mappers?

Traditional O/R mappers like EF Core use expressions on the .NET domain objects to generate SQL queries. But you never 
know if an expression can be translated to SQL until you run the query. For example, the custom method in 
`.Where(user => user.IsInRole("Admin"))` cannot be converted to SQL and will throw an exception at runtime. The compiler
won't complain. 

Other popular approaches let you write the SQL query as plain string. This isn't type safe at all and very error prone.
And again, the compiler has no chance to check your query for errors.


## Features

Supported databases: 

- MySQL
- Postgres

Each database has its own Nuget package. This allows the compiler to check if your specific query functions are 
supported by the database you are using.

At the moment, there's no general abstraction. A Nuget package that can target multiple/all databases might be added in 
the future, but it will be limited to the common features of all supported databases.


## Contributing

Please follow the [Architecture](docs/Architecture.md) and [Coding Guidelines](docs/CodingGuidelines.md).