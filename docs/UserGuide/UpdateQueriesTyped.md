# Update queries (INSERT, UPDATE, DELETE) — Strongly Typed API

Start building update queries on a strongly typed schema class (types that implement `IDbTable`, see [SchemaDefinition](SchemaDefinition.md)).

For both the INSERT and UPDATE query, we need the table schema, domain model and a mapping. 

Key interfaces: 

- `IWriterMapping<TQuery, TObj>`
- `Quib.*.ORMapper` namespace

Simple example: 

```csharp
// The table schema
public class UsersTable(DbTable table) : IDbTable, IDbTableFactory<UsersTable>
{
    public static string TableName => "users";
    public static UsersTable Factory(DbTable table) => new UsersTable(table);

    public DbColumn<DbInt> Id => table.Column<DbInt>("id");
    public DbColumn<DbText> Username => table.Column<DbText>("username");
    public DbColumn<DbText> Email => table.Column<DbText>("email");
}

// The domain model
public class User
{
    public int Id { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
}

// The mapping from the domain model to the table schema
public class UserWriter : IWriterMapping<UsersTable, User>
{
    public static void Write(IUpdateQueryBuilder query, UsersTable table, User user)
    {
        query.Set(table.Id, user.Id);
        query.Set(table.Username, user.Username);
        query.Set(table.Email, user.Email);
    }
}
```


## INSERT query

Key interfaces: 

- `QueryFactory.InsertInto`
- `QueryFactory.InsertInto<T>`


Example:

```csharp
public async Task Demo(QueryFactory db)
{
    var alice = new User { Id = 42, Username = "ajones", Email = "alice@example.com" };
    var bob = new User { Id = 43, Username = "bob", Email = "bob@example.com" };

    var rowsInserted = await db.InsertInto<UsersTable>()
        .AddRow<UserWriter, User>(alice)
        .AddRow<UserWriter, User>(bob)  // You may add multiple rows in a single query
        .Execute();
}
```

The method will return the number of affected rows.


## UPDATE query

Key interfaces: 

- `QueryFactory.Update`
- `QueryFactory.Update<T>`


Example:

```csharp
public async Task Demo(QueryFactory db)
{
    var alice = new User { Id = 42, Username = "ajones", Email = "jones@example.com" };

    var rowsUpdated = await db.Update<UsersTable>().With<UserWriter, User>(alice).Execute();
}
```

The method will return the number of affected rows.

**Attention**: this will update all rows in the table! Filter function will follow soon...


## DELETE query

Will follow soon...


## Next Steps

- **[SelectQueriesTyped.md](SelectQueriesTyped.md)** — query the results of your modifications using your schema
- **[ResultMappingTyped.md](ResultMappingTyped.md)** — read back results and map to domain objects
