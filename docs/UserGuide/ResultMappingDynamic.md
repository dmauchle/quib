# Result Mapping — Dynamic API

Retreiving results of SELECT queries consists of three steps: 

- You need to build the projection for the query (SELECT clause)
- Quib will fetch the result set from the database
- You need to map the result to domain objects 

Simple example projecting multiple columns into an anonymous type:

```csharp
public async Task Demo(QueryFactory db)
{
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
}
```

- `Project()` will build the SELECT clause
- `FetchArray()` will execute the query, read the result and put it into your mapping function.
