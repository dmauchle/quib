# Result Fetching Methods

After building a projected SELECT query (see [ResultMappingTyped.md](ResultMappingTyped.md)), you can fetch results from 
the database using one of three methods: 


## FetchArray — Collecting All Results

With `FetchArray` the entire result set is loaded into memory and the reader is closed. So you could run another query
e.g. inside the for loop.

```csharp
public async Task Demo(QueryFactory db)
{
    var users = await db.SelectFrom<UsersTable>().Map<UserReader, User>().FetchArray();
    foreach (var user in users) 
    {
        Console.WriteLine(user.Username);
    }
}
```


## FetchOne — Retrieve a single result row (or none)

Use `FetchOne` when you expect at most one result row from your query. It returns a `SingleQueryResult<T>` that indicates whether a result exists.

```csharp
public async Task FetchSingleRow(QueryFactory db)
{
    var user = await db.SelectFrom<UsersTable>().Map<UserReader, User>().FetchOne();

    if (user.HasResult)
    {
        Console.WriteLine(user.Result.Username);
    }
}
```

Always check `HasResult` before accessing `Result` to determine if a row was found. If no rows are returned from the 
database, `HasResult` will be `false`.


## FetchStream — Streaming Results

Use `FetchStream` when you need to process rows one at a time as they arrive from the database. This is the most memory-efficient 
approach for large result sets. The data reader behind the result will remain open until you iterate through the full 
result set. So you can't run another query at the same time.

```csharp
public async Task Demo(QueryFactory db)
{
    var users = db.SelectFrom<UsersTable>().Map<UserReader, User>().FetchStream();

    await foreach (var user in users)
    {
        Console.WriteLine(user.Username);
    }
}
```
