# Building SQL Expressions for Query and Schema

## Use cases 

Some use cases for SQL expressions (of course you may use them anywhere you would expect them):

```csharp
public class UsersTable(DbTable table) : IDbTable
{
    public static string TableName => "users";

    public NullableColumn<DbText> Email => table.NullableColumn<DbText>("email");
    public NotNullExpression<DbBool> HasEmail => this.Email.IsNotNull();  // Expression on schemas, use it like a "view"
}
```

```csharp
db.SelectFrom<UsersTable>()
    .Where(x => x.Credits > 100 & x.Country == "Switzerland")  // Filter expressions
    .Project((rs, x) => new { GoodCustomer = rs.Get(x.TotalOrders > 1000) });  // Projection expressions

db.Update<UsersTable>().AllRows().With((row, x) => row.Set(x.Credits, x.Credits + 10)).Execute();  // Update expressions
```


## Comparison operators

You can use the standard comparison operators in your expressions: `==`, `!=`, `<`, `>`, `<=`, `>=`.

```csharp
db.SelectFrom<UsersTable>().Where(x => x.Credits > 100 & x.Company == "Google");
```

Comparing to null is not supported due to ambiguity. You could achieve it with an explicit parameter call if you want. 
That translates to `x.Company = NULL` in SQL, which is not the same as `x.Company IS NULL`!

```csharp
db.SelectFrom<UsersTable>().Project((rs, x) => rs.Get(x.Company == null));  // Compiler error or runtime exception!
db.SelectFrom<UsersTable>().Project((rs, x) => rs.Get(x.Company == SQL.NullableParameter((string?)null)));  // => null 
db.SelectFrom<UsersTable>().Project((rs, x) => rs.Get(x.Company.IsNull()));  // => true/false
```

> Note: The compiler error for `x.Company == null` relies on nullable reference types (CS8625). It is only an error if 
> your project enables `<Nullable>enable</Nullable>` and treats nullable warnings as errors 
> (`<WarningsAsErrors>Nullable</WarningsAsErrors>`). Otherwise it is just a warning, or with nullable checks disabled, 
> not reported at all. In those cases the code compiles, but throws an `ArgumentNullException` when the expression is 
> built.


## Arithmetic operators

On numerical types, you can use the following operators: `+`, `-`, `*`, `/`, `%`.

```csharp
db.Update<UsersTable>().AllRows().With((row, x) => row.Set(x.Credits, x.Credits + 10)).Execute();
```

On boolean types, you can use the following operators: `&`, `|`, `!`.

```csharp
db.SelectFrom<UsersTable>().Where(x => x.IsActive & !x.IsDeleted);
```

String concatenation is not supported using an operator (`+`). Use `SQL.Concat()` instead.


## Null Checks

```csharp
db.SelectFrom<UsersTable>().Where(x => x.Email.IsNull());
db.SelectFrom<UsersTable>().Where(x => x.Email.IsNotNull());
```
