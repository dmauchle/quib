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

- `QueryFactory.InsertIntoTable()`
- `QueryFactory.InsertInto<T>()`


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

- `QueryFactory.UpdateTable()`
- `QueryFactory.Update<T>()`


Example for an updates using the declared mapping from the schema:

```csharp
public async Task Demo(QueryFactory db)
{
    var alice = new User { Id = 42, Username = "ajones", Email = "jones@example.com" };

    var rowsUpdated = await db.Update<UsersTable>()
        .Where(x => x.Id == alice.Id)
        .With<UserWriter, User>(alice)
        .Execute();
}
```

Example for an inline update. This is especially useful when you don't want to update all the columns:

```csharp
public async Task Demo(QueryFactory db)
{
    var rowsUpdated = await db.Update<UsersTable>()
        .Where(x => x.Id == 42)
        .With((row, x) => {
            row.Set(x.Email, "jones@example.com");
        })
        .Execute();
}
```

Updating all rows in the table: 

```csharp
public async Task Demo(QueryFactory db)
{
    var rowsUpdated = await db.Update<UsersTable>()
        .AllRows()
        .With((row, x) => {
            row.Set(x.Country, "Switzerland");
        })
        .Execute();
}
```

It is mandatory to either filter the rows to update (`Where()`) or explicitly use `AllRows()`. This avoids accidental 
updates of all rows in the table if you forget the filter.

The method will return the number of affected rows.


## DELETE query

Key interfaces: 

- `QueryFactory.DeleteFromTable()`
- `QueryFactory.DeleteFrom<T>()`


Example for a delete query using the declared mapping from the schema:

```csharp
public async Task Demo(QueryFactory db)
{
    var rowsDeleted = await db.DeleteFrom<UsersTable>()
        .Where(x => x.Id == 42)
        .Execute();
}
```

Deleting all rows from a table:

```csharp
public async Task Demo(QueryFactory db)
{
    var rowsDeleted = await db.DeleteFrom<UsersTable>().AllRows().Execute();
}
```

It is mandatory to either filter the rows to delete (`Where()`) or explicitly use `AllRows()`. This avoids accidental 
deletes of all rows in the table if you forget the filter.

The method will return the number of affected rows.



## Next Steps

- **[SelectQueriesTyped.md](SelectQueriesTyped.md)** — query the results of your modifications using your schema
- **[ResultMappingTyped.md](ResultMappingTyped.md)** — read back results and map to domain objects


## See also

- **[Expressions.md](Expressions.md)** — learn more about the SQL expressions for filtering and updating