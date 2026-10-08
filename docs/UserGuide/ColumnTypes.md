# Column Types

Column types for use in table installers. Example: 

```csharp
install.Table("users")    
    .CreateWithPrimaryKey("id", ColumnType.Int())
    .CreateColumn("username", ColumnType.VarChar(32).NotNull())
    .CreateColumn("email", ColumnType.VarChar(255).Nullable());
```


## Available Column Types

**MySQL**

- `Int()` - 32-bit integer
- `Decimal(precision, scale)` - exact fixed-point number
- `Bool()` - boolean, stored as `TINYINT(1)` (`BOOLEAN` is an alias).
- `Uuid()` - UUID (.NET `Guid`), stored as `CHAR(36)` with `ascii` charset and `ascii_bin` collation (see below)
- `VarChar(length)` - variable-length string (number of characters, not number of bytes), max. 65,535 bytes (row size limit)
- `ShortText()` - up to 65,535 bytes of text data. Separate from row size limit
- `MediumText()` - up to 16 MB of text data
- `LongText()` - up to 4 GB of text data

**Postgres**

- `Int()` - 32-bit integer
- `Decimal(precision, scale)` - exact fixed-point number
- `Bool()` - native boolean
- `Uuid()` - native uuid (.NET `Guid`, 16 bytes)
- `VarChar(length)` - variable-length string (number of characters, not number of bytes), max 1 GB (value limit)
- `ShortText()` - supposed to be in the KB-range, but Postgres only supports 1 GB text.
- `MediumText()` - supposed to be in the MB-range, but Postgres only supports 1 GB text.
- `LongText()` - up to 1 GB of text data (value limit)


## Decimal

Use `DbDecimal` / `DbDecimalNullable` in queries, the .NET type is `decimal`. `DECIMAL(precision, scale)` stores exact 
values, so it's the right choice for money, e.g. `ColumnType.Decimal(19, 4)`.

- `precision`: total number of digits, 1 - 28. Limited to 28, so every value of the column fits into a .NET `decimal`.
- `scale`: number of digits after the decimal point, 0 - `precision`.

Writing values that don't fit the column:

- Too many decimals: the value is rounded to the scale (e.g. `1.235` → `1.24` in `DECIMAL(10,2)`).
- Integer part too large: the database raises an error. MySQL only does this in strict mode (`STRICT_TRANS_TABLES`, 
  default since 5.7). Otherwise it silently stores the largest value the column allows.

Values are read with the scale of the column: `1.5m` comes back as `1.50m` from `DECIMAL(10,2)`. The values are equal, 
but `ToString()` differs.

Calculations can produce values larger or with more decimals than the column type (e.g. `SUM` or division). Results with 
more than 28 decimals are rounded when read. Results exceeding the range of a .NET `decimal` throw an 
`OverflowException`. Also, the scale of a division result differs between databases (MySQL: 4 more decimals than the 
dividend, Postgres: at least 16 significant digits).


## UUID

Use `DbUuid` / `DbUuidNullable` in queries, the .NET type is `Guid`. Values are compared and sorted in their standard 
text/byte order, so version 7 UUIDs (`Guid.CreateVersion7()`) sort by creation time.

**MySQL** has no native UUID type. Quib stores UUIDs as text in the standard lowercase format 
(`0192f3a1-5c2e-7a00-8000-0000000000ab`), using the compact `ascii` charset (36 bytes) and the binary, case-sensitive 
`ascii_bin` collation. Values are human-readable in SQL tools. `BINARY(16)` storage is currently not supported.


## Character Set and Collation

When creating tables through the Quib library, all text-based columns always support UTF-8 (full character support, 
including emojis). A case-insensitive collation is used by default for string comparisons. This means that when you 
perform string comparisons, the comparison will ignore case differences (e.g., "abc" will be considered equal to "ABC").

To ensure full compatibility and proper character collation, database-specific configuration is required:

**MySQL**

Columns explicitly configure the `utf8mb4` character set and `utf8mb4_unicode_ci` collation by default.

To ensure full UTF-8 support in MySQL, you **must** configure your connection string with the `utf8mb4` charset. This is 
essential for proper character handling:

```csharp
// Example MySQL connection string with utf8mb4 charset
var connectionString = "Server=localhost;Database=MyDatabase;User=root;Password=password;CharSet=utf8mb4;";
using var connection = new MySqlConnection(connectionString);
```

**Postgres**

Columns explicitly configure the `en_US.utf8` collation by default. It's also case-insensitive for string comparisons. 

