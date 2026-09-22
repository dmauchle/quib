
# Installer API

The Installer API provides a small, fluent DDL surface for creating and evolving your database schema from C# code. It 
is thin wrapper over runtime SQL generation.

It's a purely dynamic API and doesn't have any dependencies on your [Schema](SchemaDefinition.md). This is intentional
and avoids breaking migrations when you change your (current) schema definitions.

Key interfaces: 

- `DbInstaller`
- `Quib.*.Installer` namespace


## Create a table

The following example creates a simple "users" table with an integer primary key and two text columns.

```csharp
using (var connection = new Npgsql.NpgsqlConnection(_dbContainer.ConnectionString))
{
    await connection.OpenAsync();

    using (var installer = new DbInstaller(connection))
    {
        // Simple creation with primary key and additional columns
        installer.Table("users")    
            .CreateWithPrimaryKey("id", ColumnType.Int())
            .CreateColumn("username", ColumnType.VarChar(32).NotNull())
            .CreateColumn("email", ColumnType.VarChar(255).Nullable());
    }
}
```

More details on column types: [ColumnTypes.md](ColumnTypes.md)


**Multi column primary keys:**

The `CreateWithPrimaryKey` overload also accepts a PrimaryKey builder when you need composite keys:

```csharp
var pk = new PrimaryKey().AddColumn("tenant_id", ColumnType.Int()).AddColumn("id", ColumnType.Int());
installer.Table("users").CreateWithPrimaryKey(pk);
```


## Modify tables

**Rename a table:**

```csharp
installer.Table("users").RenameTo("app_users");
```

**Drop a table:**

```csharp
installer.Table("old_table").Drop();
```


## Modify columns

**Add a new column to an existing table:**

```csharp
installer.Table("users").CreateColumn("bio", ColumnType.VarChar(255).Nullable());
```

**Change a column's type:**

```csharp
installer.Table("users").Column("email").ChangeType(ColumnType.VarChar(512).Nullable());
```

**Rename a column**

```csharp
installer.Table("users").Column("username").RenameTo("user_name");
```

Note: MySQL 5.6 and 5.7 do not support the RENAME COLUMN syntax. Use ChangeTo instead (renames and re-declares the column in a single statement):

```csharp
installer.Table("users").Column("username").ChangeTo("user_name", ColumnType.VarChar(64).NotNull());
```

**Drop a column:**

```csharp
installer.Table("users").Column("temp_col").Drop();
```


## Foreign keys

**Create a foreign key:**

```csharp
// Create fk_orders_user on orders(user_id) referencing users(id)
installer.Table("orders").CreateForeignKey("fk_orders_user", new[] { "user_id" }, "users", new[] { "id" });
```

**Drop a foreign key:**

```csharp
installer.Table("orders").DropForeignKey("fk_orders_user");
```


## Indexes

**Create an index:**

```csharp
installer.Table("users").CreateIndex("idx_users_username", "username");
```

**Drop an index:**

```csharp
installer.Table("users").DropIndex("idx_users_username");
```
