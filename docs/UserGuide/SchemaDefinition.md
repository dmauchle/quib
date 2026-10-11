# Schema Definition

Quib separates query construction from domain objects by providing a typed schema layer. Use the
schema layer when you want reusable, discoverable table/column definitions.

Key interfaces:

- `IDbTable` — wrapper around a query-time table instance that exposes typed column accessors
- `IDbTable<T>` — factory pattern used e.g. by `SelectFrom<T>()` to create typed table descriptors
- `Quib.*.Schema` namespace


Simple table declaration:

```csharp
private class UsersTable(DbTable table) : IDbTable, IDbTable<UsersTable>
{
    public static string TableName => "users";
    public static UsersTable CreateFrom(DbTable table) => new UsersTable(table);

    public NotNullColumn<DbInt> Id => field ??= table.Column<DbInt>("id");
    public NotNullColumn<DbText> Username => field ??= table.Column<DbText>("username");
    public NullableColumn<DbText> Email => field ??= table.NullableColumn<DbText>("email");
}
```

Note the use of the `field ??=` pattern. This allows the column accesses to be cached, which improves performance.

The DB type (`DbInt`, `DbText`, ...) denotes the type of the column, the column class denotes its nullability: 

- `NotNullColumn<T>` (created by `table.Column<T>()`) for NOT NULL columns
- `NullableColumn<T>` (created by `table.NullableColumn<T>()`) for nullable columns

The same applies to all expressions: `NotNullExpression<T>` and `NullableExpression<T>`. A not-null expression can be 
used wherever a nullable expression is expected.


## See also

- **[Expressions.md](Expressions.md)** — build WHERE clauses and other expressions
- **[SelectQueries.md](SelectQueries.md)** — use your schema to build SELECT queries
- **[UpdateQueries.md](UpdateQueries.md)** — use your schema to perform INSERT, UPDATE and DELETE operations