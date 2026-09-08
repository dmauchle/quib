# Getting Started

## Choose the right database package

Quib provides separate packages per database and version (see [SupportedDatabases.md](SupportedDatabases.md)). Reference
the package that matches your database dialect and server version.

Add the desired NuGet package to your project, for example

- Quib.MySql80
- Quib.Postgres18


## Install (migrate) your database

Quib provides a fluent API for database migrations. So you don't have to use raw (and error prone)
SQL files.

```csharp
using (var connection = new Npgsql.NpgsqlConnection(_dbContainer.ConnectionString))
{
    await connection.OpenAsync();

    using (var installer = new DbInstaller(connection))
    {
        installer.Table("users")
            .CreateWithPrimaryKey("id", ColumnType.Int)
            .AddColumn("username", ColumnType.VarChar(32).NotNull())
            .AddColumn("email", ColumnType.VarChar(255).Nullable());
    }
}
```

Read [Installer.md](Installer.md) for more details.


## Configure your database schema and mapping to your domain model

Define a schema class that describes your table structure. 

```csharp
public class UsersTable(DbTable table) : IDbTable, IDbTableFactory<UsersTable>
{
    public static string TableName => "users";
    public static UsersTable Factory(DbTable table) => new UsersTable(table);

    public DbColumn<DbInt> Id => table.Column<DbInt>("id");
    public DbColumn<DbText> Username => table.Column<DbText>("username");
    public DbColumn<DbText> Email => table.Column<DbText>("email");
}
```

Read [SchemaDefinition.md](SchemaDefinition.md) for more details.


## Insert data

Assume the following domain model we want to insert into the database:

```csharp
public class User
{
    public int Id { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
}
```

We need to declare a writer mapping from the class to the database table:

```csharp
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

Insert a new user record:

```csharp
using (var connection = new Npgsql.NpgsqlConnection(_dbContainer.ConnectionString))
{
    await connection.OpenAsync();

    var db = new QueryFactory(connection);

    var newUser = new User { Id = 1, Username = "alice", Email = "alice@example.com" };

    await db.InsertInto<UsersTable>()
        .AddRow<UserWriter, User>(newUser)
        .Execute();
}
```

Read [UpdateQueriesTyped.md](UpdateQueriesTyped.md) for more details.


## Query the database

In order to deserialize the data/rows into our domain object, we need to have a mapping class.

```csharp
public class UserReader(ProjectionBuilder query, UsersTable user) : IReaderMappingFactory<UsersTable, User>
{

    public static IReaderMapping<UsersTable, User> Factory(ProjectionBuilder query, UsersTable user) => new UserReader(query, user);

    private readonly ProjectedValue<DbInt> _id = query.Get(user.Id);
    private readonly ProjectedValue<DbText> _username = query.Get(user.Username);
    private readonly ProjectedValue<DbText> _email = query.Get(user.Email);

    public User Map(ResultReader rs)
    {
        return new User
        {
            Id = rs.Get(_id),
            Username = rs.Get(_username),
            Email = rs.Get(_email)
        };
    }
}
```

Then use it to query the database and map the results into our domain object:

```csharp
using (var connection = new Npgsql.NpgsqlConnection(_dbContainer.ConnectionString))
{
    await connection.OpenAsync();
    var db = new QueryFactory(connection);

    // A simple query would look like this:
    var users = await db.SelectFrom<UsersTable>().Map<UserReader, User>().FetchArray();
}
```

Read [SelectQueriesTyped.md](SelectQueriesTyped.md), [ResultMappingTyped.md](ResultMappingTyped.md) and
[ResultFetching.md](ResultFetching.md) for more details.

