# Select Queries — Dynamic API

Start building SELECT queries without the need of a schema definition or domain model.

Key interfaces: 

- `QueryFactory.SelectFromTable`
- `Quib.*.QueryBuilder` namespace


You can start building a query as follows: 

```csharp
public void Demo(QueryFactory db)
{
    db.SelectFromTable("users");
}
```


## Filtering (WHERE clause)

Will follow soon...


## Sorting (ORDER BY clause)

Will follow soon...


## Next Steps

- **[ResultMappingDynamic.md](ResultMappingDynamic.md)** — execute the query and fetch results
