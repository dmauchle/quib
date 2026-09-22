# Overview

Quib is a type-safe SQL query builder and O/R mapper for .NET that prioritizes compile-time safety and explicit
mapping over runtime query translation. Unlike traditional ORMs that operate on domain objects
and rely on expression translation, Quib exposes a fluent, typed API for building SQL queries
directly against tables, columns and SQL types. Only after executing a query, the domain model comes into
play and the result set is mapped to your .NET classes.

**Get your hands dirty immediately:** [Getting Started](GettingStarted.md)


## Query API

Your explicit schema definitions and domain models provide compile-time checking and avoid runtime errors (preferred).

- [SchemaDefinition.md](SchemaDefinition.md) — describing tables and columns to ensure type safety and error checks
- [SelectQueries.md](SelectQueries.md) — run SELECT queries
    - [ResultMapping.md](ResultMapping.md) — bring the results of your SELECT query to your domain model
    - [ResultFetching.md](ResultFetching.md) — various ways to fetch the results of your SELECT query
    - [Joins.md](Joins.md) — JOIN tables in your SELECT queries
- [UpdateQueries.md](UpdateQueries.md) — run INSERT, UPDATE and DELETE queries


## Installer API

Update your database schema and run migrations.

- [Installer.md](Installer.md) — run db migrations
- [ColumnTypes.md](ColumnTypes.md) — overview over the available column/database types


## Supported Databases

A fine selection of **Postgres** and **MySql** versions are currently supported. Read [SupportedDatabases.md](SupportedDatabases.md) for more details.