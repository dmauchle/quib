# SELECT Queries

Start building SELECT queries based on a strongly typed schema class (see [SchemaDefinition.md](SchemaDefinition.md)).

Key interfaces: 

- `QueryFactory.SelectFromTable(string)`
- `QueryFactory.SelectFrom<T>()`
- `Quib.*.QueryBuilder` namespace


With this example schema...

```csharp
public class UsersTable(DbTable table) : IDbTable, IDbTable<UsersTable>
{
    public static string TableName => "users";
    public static UsersTable CreateFrom(DbTable table) => new UsersTable(table);

    public NotNullColumn<DbInt> Id => table.Column<DbInt>("id");
    public NotNullColumn<DbText> Username => table.Column<DbText>("username");
    public NotNullColumn<DbText> Email => table.Column<DbText>("email");
}
```

You can start building a query like this: 

```csharp
db.SelectFrom<UsersTable>();
```


## Filtering (WHERE clause)

```csharp
db.SelectFrom<UsersTable>()
    .Where(x => x.City == "New York")
    .Where(x => x.Lastname == "Jones");

db.SelectFrom<UsersTable>()
    .Where(x => x.City == "New York" | x.Lastname == "Jones" | x.Name == "Charlie");
```

You can apply multiple filters. They will be combined with `AND` in the SQL query. 

Read [Expressions.md](Expressions.md) for more information about building the filter expressions.


## Sorting (ORDER BY clause)

A few examples:

```csharp
global using static Quib.Postgres18.QueryBuilder.Sort;

db.SelectFrom<UsersTable>().OrderBy(x => [ x.Lastname ]);
db.SelectFrom<UsersTable>().OrderBy(x => [ SQL.Sort(x.Lastname, ASC) ]);
db.SelectFrom<UsersTable>().OrderBy(x => [ SQL.Sort(x.Lastname, DESC) ]);
db.SelectFrom<UsersTable>().OrderBy(x => [ x.Lastname, x.Firstname ]);
```

Of course, you may use any [expression](Expressions.md) for the sorting.

> Note: Sort order depends on the database and collation (also with NULL ordering). See [ColumnTypes.md](ColumnTypes.md)
> for details. Currently, there's no support for Postgres NULLS FIRST and NULLS LAST clauses. MySQL doesn't support them 
> anyway.


## Paging (LIMIT clause)

```csharp
db.SelectFrom<UsersTable>().OrderBy(x => [ x.Lastname, x.Id ]).Limit(20);              // first 20 rows
db.SelectFrom<UsersTable>().OrderBy(x => [ x.Lastname, x.Id ]).Limit(20, offset: 40);  // rows 41 to 60
```


## Dynamic queries

You don't necessarily need a schema definition or domain model to build a query. You can also use the dynamic API:

```csharp
db.SelectFromTable("users")
    .Where(x => x.Column<DbText>("city") == "New York")
    .OrderBy(x => [ x.Column<DbText>("lastname") ]);
```


## See also

- **[ResultMapping.md](ResultMapping.md)** — execute the query and map results to domain objects
- **[Expressions.md](Expressions.md)** — learn more about the SQL expressions for filtering