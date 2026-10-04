# Update queries (INSERT, UPDATE, DELETE)

Start building update queries on a strongly typed schema class (types that implement `IDbTable`, see [SchemaDefinition](SchemaDefinition.md)).

For both the INSERT and UPDATE query, we need the table schema, domain model and a mapping. 

Key interfaces: 

- `IWriterMapping<TQuery, TObj>`
- `Quib.*.ORMapper` namespace

Simple example: 

```csharp
// The table schema
public class UsersTable(DbTable table) : IDbTable, IDbTable<UsersTable>
{
    public static string TableName => "users";
    public static UsersTable CreateFrom(DbTable table) => new UsersTable(table);

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

- `QueryFactory.InsertIntoTable(string)`
- `QueryFactory.InsertInto<T>()`


Example:

```csharp
var alice = new User { Id = 42, Username = "ajones", Email = "alice@example.com" };
var bob = new User { Id = 43, Username = "bob", Email = "bob@example.com" };

var rowsInserted = await db.InsertInto<UsersTable>()
    .AddRow<UserWriter, User>(alice)
    .AddRow<UserWriter, User>(bob)  // You may add multiple rows in a single query
    .Execute();
```

The method will return the number of affected rows.


## UPDATE query

Key interfaces: 

- `QueryFactory.UpdateTable(string)`
- `QueryFactory.Update<T>()`


Example of an update using the declared mapping from the schema:

```csharp
var alice = new User { Id = 42, Username = "ajones", Email = "jones@example.com" };

var rowsUpdated = await db.Update<UsersTable>()
    .Where(x => x.Id == alice.Id)
    .With<UserWriter, User>(alice)
    .Execute();
```

Example of an inline update. This is especially useful when you don't want to update all the columns:

```csharp
var rowsUpdated = await db.Update<UsersTable>()
    .Where(x => x.Id == 42)
    .With((row, x) => {
        row.Set(x.Email, "jones@example.com");
    })
    .Execute();
```

Updating all rows in the table: 

```csharp
var rowsUpdated = await db.Update<UsersTable>()
    .AllRows()
    .With((row, x) => {
        row.Set(x.Country, "Switzerland");
    })
    .Execute();
```

You can also use complex expressions for the update values, e.g. to increment a column value:

```csharp
var rowsUpdated = await db.Update<UsersTable>()
    .Where(x => x.Id == 42)
    .With((row, x) => {
        row.Set(x.LoginCount, x.LoginCount + 1);
    })
    .Execute();
```

It is mandatory to either filter the rows to update (`Where()`) or explicitly use `AllRows()`. This avoids accidental 
updates of all rows in the table if you forget the filter.

The method will return the number of affected rows.


## DELETE query

Key interfaces: 

- `QueryFactory.DeleteFromTable(string)`
- `QueryFactory.DeleteFrom<T>()`


Example of a delete query using the schema:

```csharp
var rowsDeleted = await db.DeleteFrom<UsersTable>()
    .Where(x => x.Id == 42)
    .Execute();
```

Deleting all rows from a table:

```csharp
var rowsDeleted = await db.DeleteFrom<UsersTable>().AllRows().Execute();
```

It is mandatory to either filter the rows to delete (`Where()`) or explicitly use `AllRows()`. This avoids accidental 
deletes of all rows in the table if you forget the filter.

The method will return the number of affected rows.


## Working with dynamic data without a schema or domain model

Insert: 

```csharp
var rowsInserted = await db.InsertIntoTable("users")
    .AddRow((query, table) => {
        query.Set(table.Column<DbInt>("id"), 42);
        query.Set(table.Column<DbText>("username"), "ajones");
        query.Set(table.Column<DbText>("email"), "alice@example.com");
    })
    .Execute();
```

Update: 

```csharp
var rowsUpdated = await db.UpdateTable("users")
    .Where(table => table.Column<DbInt>("id") == 42)
    .With((query, table) => {
        query.Set(table.Column<DbText>("email"), "alice.jones@example.com");
    })
    .Execute();
```

Delete: 

```csharp
var rowsDeleted = await db.DeleteFromTable("users")
    .Where(table => table.Column<DbInt>("id") == 42)
    .Execute();
```


## See also

- **[Expressions.md](Expressions.md)** — learn more about the SQL expressions for filtering and updating