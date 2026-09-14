# Select Queries — Strongly Typed API

Start building SELECT queries based on a strongly typed schema class (see [SchemaDefinition.md](SchemaDefinition.md)).

Key interfaces: 

- `QueryFactory.SelectFromTable`
- `QueryFactory.SelectFrom<T>`
- `Quib.*.QueryBuilder` namespace


With this example schema...

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

You can start building a query in one of the following styles: 

```csharp
public void Demo(QueryFactory db)
{
    db.SelectFrom<UsersTable>();  // Requires IDbTableFactory<UsersTable> with Factory method

    db.SelectFrom(UsersTable.Factory);  // Use any method you like

    db.SelectFrom(x => new UsersTable(x));  // Use a lambda to create the table instance
}
```


## Filtering (WHERE clause)

```csharp
db.SelectFrom<UsersTable>()
    .Where(x => x.City == "New York")
    .Where(x => x.Lastname == "Jones")

db.SelectFrom<UsersTable>()
    .Where(x => x.City == "New York" | x.Lastname == "Jones" | x.Name == "Charlie")
```

You can apply multiple filters. They will be combined with `AND` in the SQL query. 

Read [Expressions.md](Expressions.md) for more information about building the filter expressions.


## Sorting (ORDER BY clause)

A few examples:

```csharp
global using static Quib.Postgres18.Schema.Sort;

db.SelectFrom<UsersTable>().OrderBy(x => [ x.Lastname ]);
db.SelectFrom<UsersTable>().OrderBy(x => [ SQL.Sort(x.Lastname, ASC) ]);
db.SelectFrom<UsersTable>().OrderBy(x => [ SQL.Sort(x.Lastname, DESC) ]);
db.SelectFrom<UsersTable>().OrderBy(x => [ x.Lastname, x.Firstname ]);
```

Of course, you may use any [expression](Expressions.md) for the sorting.

> Note: sort order depends on the database and collation (also with NULL ordering). Currently, there's no support for 
> Postgres NULL FIRST and NULL LAST clause. MySQL doesn't support it anyways.


## Next Steps

- **[ResultMappingTyped.md](ResultMappingTyped.md)** — execute the query and map results to domain objects


## See also

- **[Expressions.md](Expressions.md)** — learn more about the SQL expressions for filtering