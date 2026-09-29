# Column Types

Column types for use in table installers. Example: 

```csharp
install.Table("users")    
    .CreateWithPrimaryKey("id", ColumnType.Int())
    .CreateColumn("username", ColumnType.VarChar(32).NotNull())
    .CreateColumn("email", ColumnType.VarChar(255).Nullable());
```


## Available Column Types

- `Int()` - 32-bit integer
- `VarChar(length)` - Variable-length string with a maximum length of `length` (number of characters, not number of bytes)


## Character Set and Collation

When creating tables through the Quib library, all text-based columns always support UTF-8 (full character support, 
including emojis). A case-insensitive collation is used by default for string comparisons. This means that when you 
perform string comparisons, the comparison will ignore case differences (e.g., "abc" will be considered equal to "ABC").

To ensure full compatibility and proper character collation, database-specific configuration is required:

**MySQL**

Columns explicitely configure the `utf8mb4` character set and `utf8mb4_unicode_ci` collation by default.

To ensure full UTF-8 support in MySQL, you **must** configure your connection string with the `utf8mb4` charset. This is 
essential for proper character handling:

```csharp
// Example MySQL connection string with utf8mb4 charset
var connectionString = "Server=localhost;Database=MyDatabase;User=root;Password=password;CharSet=utf8mb4;";
using var connection = new MySqlConnection(connectionString);
```

**Postgres**

Columns explicitely configure the `en_US.utf8` collation by default. It's also case-insensitive for string comparisons. 

