# Result Mapping — Strongly Typed API

Retrieving results of SELECT queries consists of three steps:

- You need to build the projection for the query (SELECT clause)
- Quib will fetch the result set from the database
- You need to map the result to domain objects 

Key interfaces: 

- `IReaderMapping<TQuery, TObj>`
- `IReaderMappingFactory<TQuery, TObj>`
- `Quib.*.ORMapper` namespace


## Simple example

Simple mapping from a users table to domain model: 

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

// The mapping from table schema to domain model, consists of two parts
public class UserReader(ProjectionBuilder query, UsersTable user) : IReaderMappingFactory<UsersTable, User>
{
    // 1. Projection (builds the SELECT clause)
    public static IReaderMapping<UsersTable, User> Factory(ProjectionBuilder query, UsersTable user) => new UserReader(query, user);

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
public async Task Demo(QueryFactory db)
{
    // Use the factory type, requires IReaderMappingFactory
    users = await db.SelectFrom<UsersTable>().Map<UserReader, User>().FetchArray();

    // Use the factory method, only requires IReaderMapping
    users = await db.SelectFrom<UsersTable>().Map(UserReader.Factory).FetchArray();

    // Using a dynamic lambda, doesn't require any interface at all
    users = await db.SelectFrom<UsersTable>().Project((rs, x) => new UserReader(rs, x)).FetchArray((rs, x) => x.Map(rs));
}
```

