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
- `BigInt()` - 64-bit integer (.NET `long`)
- `Decimal(precision, scale)` - exact fixed-point number
- `Double()` - double-precision floating point number (.NET `double`), without NaN and infinity
- `Bool()` - boolean, stored as `TINYINT(1)` (`BOOLEAN` is an alias).
- `Uuid()` - UUID (.NET `Guid`), stored as `CHAR(36)` with `ascii` charset and `ascii_bin` collation (see below)
- `DateOnly()` - date without time, stored as `DATE`, years 1000 - 9999
- `DateTime()` - date and time with whole seconds, stored as `DATETIME`, years 1000 - 9999, without time zone
- `VarChar(length)` - variable-length string (number of characters, not number of bytes), max. 65,535 bytes (row size limit)
- `ShortText()` - up to 65,535 bytes of text data. Separate from row size limit
- `MediumText()` - up to 16 MB of text data
- `LongText()` - up to 4 GB of text data
- `VarBinary(length)` - variable-length binary data (number of bytes), max. 65,535 bytes (row size limit)
- `ShortBinary()` - up to 65,535 bytes of binary data (`BLOB`). Separate from row size limit.
- `MediumBinary()` - up to 16 MB of binary data (`MEDIUMBLOB`). Separate from row size limit.
- `LongBinary()` - up to 4 GB of binary data (`LONGBLOB`). Separate from row size limit.

**Postgres**

- `Int()` - 32-bit integer
- `BigInt()` - 64-bit integer (.NET `long`)
- `Decimal(precision, scale)` - exact fixed-point number
- `Double()` - double-precision floating point number (.NET `double`), without NaN and infinity
- `Bool()` - native boolean
- `Uuid()` - native uuid (.NET `Guid`, 16 bytes)
- `DateOnly()` - date without time, stored as `date`, years 1000 - 9999
- `DateTime()` - date and time with whole seconds, stored as `timestamp(0)` (without time zone), years 1000 - 9999
- `VarChar(length)` - variable-length string (number of characters, not number of bytes), max 1 GB (value limit)
- `ShortText()` - supposed to be in the KB-range, but Postgres only supports 1 GB text.
- `MediumText()` - supposed to be in the MB-range, but Postgres only supports 1 GB text.
- `LongText()` - up to 1 GB of text data (value limit)
- `ShortBinary()` - supposed to be in the KB-range, but Postgres only supports 1 GB binary data.
- `MediumBinary()` - supposed to be in the MB-range, but Postgres only supports 1 GB binary data.
- `LongBinary()` - up to 1 GB of binary data (`BYTEA`, value limit)


## Decimal

Use `DbDecimal` in queries, the .NET type is `decimal`. `DECIMAL(precision, scale)` stores exact 
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


## Double

Use `DbDouble` in queries, the .NET type is `double`. `Double()` stores 64-bit floating point numbers (`DOUBLE` on 
MySQL, `DOUBLE PRECISION` on Postgres). Values aren't exact (`0.1 + 0.2` = `0.30000000000000004`), so use `Decimal()` 
for money.

- NaN and infinity are not supported, as MySQL can't store them. Passing them as parameter or constant throws an 
  `ArgumentOutOfRangeException`. On Postgres, such values written by others (bypassing Quib) are still read as 
  `double.NaN`, `double.PositiveInfinity` and `double.NegativeInfinity`.
- Calculations exceeding the range (e.g. `double.MaxValue * 2`) raise a database error instead of returning infinity.
- There's no modulo operator (`%`), as Postgres doesn't support it for floating point numbers.


## UUID

Use `DbUuid` in queries, the .NET type is `Guid`. Values are compared and sorted in their standard 
text/byte order, so version 7 UUIDs (`Guid.CreateVersion7()`) sort by creation time.

**MySQL** has no native UUID type. Quib stores UUIDs as text in the standard lowercase format 
(`0192f3a1-5c2e-7a00-8000-0000000000ab`), using the compact `ascii` charset (36 bytes) and the binary, case-sensitive 
`ascii_bin` collation. Values are human-readable in SQL tools. `BINARY(16)` storage is currently not supported.


## DateOnly and DateTime

Use `DbDateOnly` for `DateOnly()` columns, the .NET type is `DateOnly`. Use `DbDateTime<TTimeZone>` for `DateTime()` 
columns, the .NET type is `DateTime`.

Date-times are stored without time zone and with whole seconds. Fractional seconds are truncated (not rounded) when 
writing, e.g. `12:00:00.9` → `12:00:00`.

As the column has no time zone, `TTimeZone` defines which time zone the stored values are in:

- `DbDateTime<Utc>`: values are stored as UTC. `DateTimeKind.Local` values are converted to UTC, `Utc` and 
  `Unspecified` values are stored as is. Values are read with `DateTimeKind.Utc`. Prefer it for points in time like 
  `created_at`, as the stored values don't depend on the time zone of the application.
- `DbDateTime<LocalTime>`: values are stored as local time of the application. `DateTimeKind.Utc` values are converted 
  to local time, `Local` and `Unspecified` values are stored as is. Values are read with `DateTimeKind.Local`. So the 
  values only represent the same point in time as long as all applications run in the same time zone.

Values compared with a date-time expression (e.g. `x.CreatedAt > DateTime.Now`) are converted according to the time 
zone of the expression. Expressions of different time zones (e.g. `DbDateTime<Utc>` and `DbDateTime<LocalTime>`) can't be 
compared (compiler error). The time zone is not stored in the database: reading a column with a different time zone 
than it was written with returns wrong values.

Time zones themselves aren't supported, as MySQL has no date-time type with time zone. If you need them, store the 
time zone in a separate column.

Value range:

- Years 1000 - 9999 are supported on all databases (MySQL's official range). Smaller values (e.g. `DateOnly.MinValue`, 
  `DateTime.MinValue`) throw an `ArgumentOutOfRangeException` when used as parameter (writing or comparing), although 
  the databases would accept them (Npgsql would store `MinValue` as `-infinity`).
- `DateTime.MaxValue` is stored as `9999-12-31 23:59:59` (fractional seconds truncated).
- Smaller values written by others (bypassing Quib) are still read without exception, as long as they fit into 
  `DateOnly` / `DateTime` (years 1 - 9999).

Values written by others that don't fit into `DateOnly` / `DateTime` at all can't be read (the driver throws):

- **MySQL**: zero dates like `0000-00-00`. Strict mode (default since 5.7) prevents them from being stored. Otherwise, 
  add `Convert Zero Datetime=True` to the connection string to read them as `MinValue`.
- **Postgres**: BC dates and years after 9999. The special values `infinity` / `-infinity` are read as `MaxValue` / 
  `MinValue` by Npgsql.


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


## Binary data

Use `DbBinary` for `VarBinary()`, `ShortBinary()`, `MediumBinary()` and `LongBinary()` columns, the .NET type is 
`byte[]`. Values are stored as is, without padding or trimming (e.g. trailing zero bytes are kept).
