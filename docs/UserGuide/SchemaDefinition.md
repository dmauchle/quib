# Schema Definition

Quib separates query construction from domain objects by providing a typed schema layer. Use the
schema layer when you want reusable, discoverable table/column definitions.

Key interfaces:

- `IDbTable` — wrapper around a query-time table instance that exposes typed column accessors
- `IDbTableFactory<T>` — factory pattern used e.g. by `SelectFrom<T>()` to create typed table descriptors
- `Quib.*.Schema` namespace


Simple table declaration:

```csharp
private class UsersTable(DbTable table) : IDbTable, IDbTableFactory<UsersTable>
{
    public static string TableName => "users";
    public static UsersTable Factory(DbTable table) => new UsersTable(table);

    public DbColumn<DbInt> Id => table.Column<DbInt>("id");
    public DbColumn<DbText> Username => table.Column<DbText>("username");
    public DbColumn<DbText> Email => table.Column<DbText>("email");
}
```

You can use it in one of the following styles with any of the `QueryFactory` methods: 

```csharp
public void Demo(QueryFactory db)
{
    db.SelectFrom<UsersTable>();  // Requires IDbTableFactory<UsersTable> with Factory method

    db.SelectFrom(UsersTable.Factory);  // Use any method you like

    db.SelectFrom(x => new UsersTable(x));  // Use a lambda to create the table instance
}
```


## Next Steps

- **[SelectQueriesTyped.md](SelectQueriesTyped.md)** — use your schema to build SELECT queries
- **[UpdateQueriesTyped.md](UpdateQueriesTyped.md)** — use your schema to perform INSERT, UPDATE and DELETE operations