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


## Null Checks

```csharp
db.SelectFrom<UsersTable>().Where(x => x.Email.IsNull());
db.SelectFrom<UsersTable>().Where(x => x.Email.IsNotNull());
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

On numerical types, you can use the following operators: `+`, `-`, `*`, `/`, `%` (`%` not on `DbDouble`).

```csharp
db.Update<UsersTable>().AllRows().With((row, x) => row.Set(x.Credits, x.Credits + 10)).Execute();
```

Division of integer types (`DbInt`, `DbLong`) is an integer division truncating toward zero, like in C# 
(`7 / 2` = `3`, `-7 / 2` = `-3`), on all databases. On MySQL, it's translated to `DIV`, as MySQL's `/` would return 
a decimal (`7 / 2` = `3.5`). Use `DbDecimal` if you need the fractional part.

Division (and modulo) by zero behaves differently: Postgres raises an error, MySQL returns `NULL`.

On boolean types, you can use the following operators: `&`, `|`, `!`.

```csharp
db.SelectFrom<UsersTable>().Where(x => x.IsActive & !x.IsDeleted);
```

String concatenation is not supported using an operator (`+`). Use `SQL.Concat()` instead.


## Using numeric expressions as a different type

The `As...()` methods use a numeric expression as a wider numeric type. They never fail, as every value fits into the 
target type:

| From | Methods |
| --- | --- |
| `DbInt` | `AsLong()`, `AsDouble()`, `AsDecimal()` |
| `DbLong` | `AsDouble()`, `AsDecimal()` |
| `DbDecimal` | `AsDouble()` |

```csharp
db.SelectFrom<OrdersTable>().Project((rs, x) => rs.Get(x.Quantity.AsDecimal()));  // read the INT column as decimal
```

`AsDouble()` may lose precision, as a double only has about 15-17 significant digits: on `DbLong` for values beyond 
2^53 (about 9 * 10^15), on `DbDecimal` for values with more digits. That's the same as converting `long` or `decimal` 
to `double` in C#.

Use cases:

- Combining expressions of different types where an operator or method expects the same type, e.g. comparisons 
  (`x.TotalViews > x.Quantity.AsLong()`).
- Changing how a calculation is done, e.g. avoiding an integer overflow (`x.Quantity.AsLong() * 1000000`) or the 
  integer division (`x.Quantity.AsDouble() / 2` = `3.5` instead of `3`).


## Text functions

Text expressions (on `DbText`) support searching and comparing with `Contains()`, `StartsWith()`, `EndsWith()` and 
`Equals()`:

```csharp
db.SelectFrom<UsersTable>().Where(x => x.Lastname.Contains("Mül", TextMatch.Exact));  // case- and accent-sensitive
db.SelectFrom<UsersTable>().Where(x => x.Lastname.Contains("muller", TextMatch.IgnoreCaseAndAccents)); // finds "Müller"
db.SelectFrom<UsersTable>().Where(x => x.Lastname.StartsWith("Mü", TextMatch.ColumnCollation));
db.SelectFrom<UsersTable>().Where(x => x.Lastname.EndsWith("ller", TextMatch.Exact));
db.SelectFrom<UsersTable>().Where(x => x.Lastname.Equals("MULLER", TextMatch.IgnoreCaseAndAccents));
```

The `TextMatch` parameter defines how text is compared:

- `TextMatch.ColumnCollation`: with the collation of the column. For columns created by Quib, it's case- 
  and accent-sensitive (except MySQL 5.x columns with special settings. Without a column (e.g. only 
  parameters), the collation of the connection is used (MySQL: case-insensitive, Postgres: case-sensitive). Indexes can
  be used in some cases (`Equals()` and `StartsWith()`).
- `TextMatch.Exact`: always case- and accent-sensitive, no matter the collation of the column or connection. On MySQL, 
  indexes can't be used.
- `TextMatch.IgnoreCaseAndAccents`: case- and accent-insensitive, e.g. `"muller"` matches `"Müller"` and `"MULLER"`.
  Indexes can't be used.

Notes:

- Wildcards in the search term (`%` and `_`) are matched literally, so user input can be passed as is.
- **Postgres** requires a collation for `TextMatch.IgnoreCaseAndAccents`, created once per database by 
  `installer.InstallTextSearchExtensions()` (e.g. in a migration, see [Installer](Installer.md)). Otherwise the query 
  fails. MySQL doesn't need it (the collations are built in), so the method only exists in the Postgres package.
- Some special characters behave differently: `"ß"` matches `"ss"` (and ligatures like `"æ"` match `"ae"`) on Postgres, 
  but not on MySQL. Umlauts and accents behave the same on all databases.
- `Contains()` and `EndsWith()` can't use indexes, so the whole table is scanned. That's fine for small and medium 
  tables. `Equals()` and `StartsWith()` may use indexes, depending on the `TextMatch` setting.
