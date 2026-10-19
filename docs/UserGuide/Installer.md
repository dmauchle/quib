# Installer and Migrations API

The Installer API provides a small, fluent DDL surface for creating and evolving your database schema from C# code. It 
is a thin wrapper over runtime SQL generation.

It's a purely dynamic API and doesn't have any dependencies on your [Schema](SchemaDefinition.md). This is intentional
and avoids breaking migrations when you change your (current) schema definitions.

Key interfaces: 

- `DbInstaller`
- `DbMigration`
- `Quib.*.Installer` namespace


## Create migrations

Migrations are versioned scripts. The installer will keep track of the installed migrations. Scripts for already 
installed versions won't be executed again on subsequent installations. 

**Modules for migrations:**

Migrations can be divided into modules like "Catalog" or "Orders". Each module can have multiple versions (installer
scripts). Module names must not be empty and are limited to 128 characters. Otherwise adding the module to the
migration throws a `MigrationException`.

A module can also depend on other modules. For example, the "Orders" module may depend on the "Catalog" module because 
it needs to create a foreign key to the products table. The installer will make sure that the scripts are executed in
the right order.

Note 1: When requiring e.g. a "Version 2" of a module, the module's version must and will be exactly at version 2 (not 
higher or lower). This is required because for example the products table may also be renamed in a later version. You
must make sure that migrations can be executed in your intended order. Otherwise the installer will throw an exception.
When adding an install script that has a dependency on another module, just look up the current version of that module.

Note 2: You may also query or change data in your install scripts. Use the `db` (`QueryFactory`) object.

```csharp
internal sealed class OrderModule : IModuleMigration
{
    public static string ModuleName => "orders";

    public static void Configure(ModuleMigration module)
    {
        module.AddVersion(1)
            .Requires<CatalogModule>(version: 2)  // Requires the catalog module at version 2 for creating the foreign key
            .With(async (install, db) =>
            {
                install.Table("orders")
                    .CreateWithPrimaryKey("id", ColumnType.Int())
                    .CreateColumn("product_id", ColumnType.Int().NotNull())
                    .CreateColumn("quantity", ColumnType.Int().NotNull())
                    .CreateForeignKey("order_product_fk", ["product_id"], "products", ["id"]);
            });

        module.AddVersion(2)
            .With(async (install, db) =>
            {
                install.Table("orders")
                    .CreateColumn("comment", ColumnType.VarChar(255).Nullable());

                await db.UpdateTable("orders")
                    .AllRows()
                    .With((query, table) =>
                    {
                        query.Set(table.Column<DbText>("comment"), "No comment");
                    })
                    .Execute();
            });
    }
}
```

**Run the migration with all modules of your application:**

```csharp
using (var connection = new Npgsql.NpgsqlConnection("..."))
{
    await connection.OpenAsync();

    using (var installer = new DbInstaller(connection))
    {
        var migration = new DbMigration("dbversions");
        migration.Add<Migrations.CatalogModule>();
        migration.Add<Migrations.OrderModule>();
        await installer.RunMigration(migration);
    }
}
```


## Create a table

The following example creates a simple "users" table with an integer primary key and two text columns.

```csharp
install.Table("users")    
    .CreateWithPrimaryKey("id", ColumnType.Int())
    .CreateColumn("username", ColumnType.VarChar(32).NotNull())
    .CreateColumn("email", ColumnType.VarChar(255).Nullable());
```

More details on column types: [ColumnTypes.md](ColumnTypes.md)


**Multi column primary keys:**

The `CreateWithPrimaryKey` overload also accepts a PrimaryKey builder when you need composite keys:

```csharp
var pk = new PrimaryKey().AddColumn("tenant_id", ColumnType.Int()).AddColumn("id", ColumnType.Int());
install.Table("users").CreateWithPrimaryKey(pk);
```


## Modify tables

**Rename a table:**

```csharp
install.Table("users").RenameTo("app_users");
```

**Drop a table:**

```csharp
install.Table("old_table").Drop();
```


## Modify columns

**Add a new column to an existing table:**

```csharp
install.Table("users").CreateColumn("bio", ColumnType.VarChar(255).Nullable());
```

**Change a column's type:**

```csharp
install.Table("users").Column("email").ChangeType(ColumnType.VarChar(512).Nullable());
```

**Rename a column**

```csharp
install.Table("users").Column("username").RenameTo("user_name");
```

Note: MySQL 5.6 and 5.7 do not support the `RENAME COLUMN` syntax. Use `ChangeTo` instead (renames and re-declares the column in a single statement):

```csharp
install.Table("users").Column("username").ChangeTo("user_name", ColumnType.VarChar(64).NotNull());
```

**Drop a column:**

```csharp
install.Table("users").Column("temp_col").Drop();
```


## Foreign keys

**Create a foreign key:**

```csharp
// Create fk_orders_user on orders(user_id) referencing users(id)
install.Table("orders").CreateForeignKey("fk_orders_user", new[] { "user_id" }, "users", new[] { "id" });
```

**Drop a foreign key:**

```csharp
install.Table("orders").DropForeignKey("fk_orders_user");
```


## Indexes

**Create an index:**

```csharp
install.Table("users").CreateIndex("idx_users_username", "username");
```

**Drop an index:**

```csharp
install.Table("users").DropIndex("idx_users_username");
```


## Text search

Case- and accent-insensitive text functions (`TextMatch.IgnoreCaseAndAccents`, see [Expressions](Expressions.md)) require 
a collation on Postgres. Create it once per database, e.g. in your first migration (it could be called multiple times,
nothing happens if the collation already exists):

```csharp
install.InstallTextSearchExtensions();  // Postgres only
```

MySQL doesn't need it (the collations are built in), so the method only exists in the Postgres package.


## See also

- [ColumnTypes.md](ColumnTypes.md) — overview of the available column/database types