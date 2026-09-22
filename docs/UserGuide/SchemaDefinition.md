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

    public DbColumn<DbInt> Id => field ??= table.Column<DbInt>("id");
    public DbColumn<DbText> Username => field ??= table.Column<DbText>("username");
    public DbColumn<DbText> Email => field ??= table.Column<DbText>("email");
}
```

Note the use of the `field ??=` pattern. This allows the column accesses to be cached and optimize performance.


## See also

- **[Expressions.md](Expressions.md)** — build WHERE clauses and other expressions
- **[SelectQueries.md](SelectQueries.md)** — use your schema to build SELECT queries
- **[UpdateQueries.md](UpdateQueries.md)** — use your schema to perform INSERT, UPDATE and DELETE operations