# Result Mapping

Retrieving results of SELECT queries consists of three steps:

- You need to build the projection for the query (SELECT clause)
- Quib will fetch the result set from the database
- You need to map the result to domain objects 

Key interfaces: 

- `IReaderMapping<TObj>`
- `IReaderMapping<TQuery, TObj>`
- `Quib.*.ORMapper` namespace


## Simple example

Simple mapping from a users table to domain model: 

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

// The mapping from table schema to domain model, consists of two parts
public class UserReader(ProjectionBuilder query, UsersTable user) : IReaderMapping<UsersTable, User>
{
    // 1. Projection (builds the SELECT clause)
    public static IReaderMapping<UsersTable, User> CreateFrom(ProjectionBuilder query, UsersTable user) => new UserReader(query, user);

    private readonly ProjectedValue<DbInt> _id = query.Get(user.Id);
    private readonly ProjectedValue<DbText> _username = query.Get(user.Username);
    private readonly ProjectedValue<DbText> _email = query.Get(user.Email);

    // 2. Mapping the result (projections) to the domain model after fetching from the database
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

Then you can fetch the results in one of the following styles: 

```csharp
var users = await db.SelectFrom<UsersTable>().Map<UserReader, User>().FetchArray();
```


## Mapping to anonymous types

You need to explicitely perform the projection and mapping step using two functions: 

```csharp
var users = await db.SelectFromTable("users")
    .Project((rs, x) => new {
        Id = rs.Get(x.Column<DbInt>("id")),
        Username = rs.Get(x.Column<DbText>("username")),
        Email = rs.Get(x.Column<DbText>("email"))
    })
    .FetchArray((rs, x) => new {
        Id = rs.Get(x.Id),
        Username = rs.Get(x.Username),
        Email = rs.Get(x.Email)
    });
```


## See also

- [ResultFetching.md](ResultFetching.md) — about the available variants for fetching your result set.