# Update queries (INSERT, UPDATE, DELETE) — Dynamic API

Start building update queries without the need of a schema definition or domain model.


## INSERT query

Key interfaces: 

- `QueryFactory.InsertIntoTable`


Example:

```csharp
public async Task Demo(QueryFactory db)
{
    var rowsInserted = await db.InsertInto("users")
        .AddRow((query, table) => {
            query.Set(table.Column<DbInt>("id"), 42);
            query.Set(table.Column<DbText>("username"), "ajones");
            query.Set(table.Column<DbText>("email"), "alice@example.com");
        })
        .AddRow((query, table) => {
            query.Set(table.Column<DbInt>("id"), 43);
            query.Set(table.Column<DbText>("username"), "bob");
            query.Set(table.Column<DbText>("email"), "bob@example.com");
        })
        .Execute();
}
```

- You may add multiple rows with a single query execution.
- The method will return the number of affected rows.


## UPDATE query

Key interfaces: 

- `QueryFactory.UpdateTable`


Example:

```csharp
public async Task Demo(QueryFactory db)
{
    var rowsUpdated = await db.UpdateTable("users")
        .With((query, table) => {
            query.Set(table.Column<DbText>("email"), "alice.jones@example.com");
        })
        .Execute();
}
```

The method will return the number of affected rows.

**Attention**: this will update all rows in the table! Filter function will follow soon...


## Next Steps

After modifying data, you can:

- **[SelectQueriesDynamic.md](SelectQueriesDynamic.md)** — query the results of your modifications
- **[ResultMappingDynamic.md](ResultMappingDynamic.md)** — read back results