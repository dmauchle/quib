# Getting Started with Quib

## Choose the right database package

Quib provides separate packages per database and version (see [SupportedDatabases.md](SupportedDatabases.md)). Reference
the package that matches your database dialect and server version.

Add the desired NuGet package to your project, for example

- Quib.MySql80
- Quib.Postgres18


## Install (migrate) your database

Quib provides a fluent API for database migrations. So you don't have to use raw (and error-prone)
SQL files.

For each module of your application, define a migration class that contains all updates of the database schema for that
module: 

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

                // You may also update data in the migration: 
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

The installer will keep track of the installed migrations in a database table (e.g. `dbversions`). The version's 
install script will only run when the particular version has not been installed yet.

Run the migration with all modules of your application:

```csharp
using var connection = new NpgsqlConnection("...");
await connection.OpenAsync();

using (var installer = new DbInstaller(connection))
{
    var migration = new DbMigration("dbversions");
    migration.Add<Migrations.CatalogModule>();
    migration.Add<Migrations.OrderModule>();
    await installer.RunMigration(migration);
}
```

Read [Installer.md](Installer.md) for more details.


## Configure your database schema and mapping to your domain model

Define a schema class that describes your table structure. 

```csharp
using Quib.Postgres18.Schema;

internal class ProductsTable(DbTable table) : IDbTable, IDbTable<ProductsTable>
{
    public static string TableName => "products";
    public static ProductsTable CreateFrom(DbTable table) => new ProductsTable(table);

    public DbColumn<DbInt> ID => field ??= table.Column<DbInt>("id");
    public DbColumn<DbText> ProductName => field ??= table.Column<DbText>("productname");
    public DbColumn<DbInt> CategoryId => field ??= table.Column<DbInt>("categoryid");
    public DbColumn<DbInt> Price => field ??= table.Column<DbInt>("price");
}

```

Read [SchemaDefinition.md](SchemaDefinition.md) for more details.


## Query the database (SELECT)

In order to deserialize the data/rows into our domain object, we need to have a mapping class.

```csharp
using Quib.Postgres18.ORMapper;
using Quib.Postgres18.QueryBuilder;
using Quib.Postgres18.Schema;

internal class ProductsReader(IProjectionBuilder query, ProductsTable product) : IReaderMapping<Product>, IReaderMapping<ProductsTable, Product>
{
    static IReaderMapping<Product> IReaderMapping<ProductsTable, Product>.CreateFrom(IProjectionBuilder rs, ProductsTable query) => new ProductsReader(rs, query);

    private readonly ProjectedValue<DbInt> _id = query.Get(product.ID);
    private readonly ProjectedValue<DbText> _productName = query.Get(product.ProductName);
    private readonly ProjectedValue<DbInt> _categoryId = query.Get(product.CategoryId);
    private readonly ProjectedValue<DbInt> _price = query.Get(product.Price);

    IProjectedValue[] IReaderMapping<Product>.ObjectIdentity => [ _id ];

    Product IReaderMapping<Product>.Map(ResultReader rs)
    {
        return new Product
        {
            ID = new ProductId(rs.Get(_id)),
            ProductName = rs.Get(_productName),
            CategoryId = new CategoryId(rs.Get(_categoryId)),
            Price = rs.Get(_price)
        };
    }

}
```

Then use it to query the database and map the results into our domain object:

```csharp
var rs1 = await db.SelectFrom<ProductsTable>()
    .Where(x => x.CategoryId == 1 | x.CategoryId == 2)
    .Where(x => x.Price < 1000)
    .OrderBy(x => [SQL.Sort(x.ProductName, ASC)])
    .Map((query, x) => new ProductsReader(query, x))
    .FetchArray();
```

When you expect only a single row, you can use `FetchOne()` instead of `FetchArray()`.

```csharp
var rs2 = await db.SelectFrom<ProductsTable>()
    .Where(x => x.ID == 7265)
    .Map((query, x) => new ProductsReader(query, x))
    .FetchOne();
```

Read [SelectQueries.md](SelectQueries.md), [ResultMapping.md](ResultMapping.md) and [ResultFetching.md](ResultFetching.md) 
for more details.

For a query with JOINs: 

```csharp
var rs3 = await db
    .Select(query =>
    {
        var product = query.From<ProductsTable>();
        var category = query.InnerJoin<CategoriesTable>().On(x => x.ID == product.CategoryId);
        return new { product, category };
    })
    .Project((rs, query) => new
    {
        product = new ProductsReader(rs, query.product),
        category = new CategoriesReader(rs, query.category)
    })
    .FetchArray((rs, x) => new
    {
        product = rs.Map(x.product),
        category = rs.Map(x.category)
    });
```

Read [Joins.md](Joins.md) for more details.


## Writing to the database (INSERT, UPDATE, DELETE)

Assume the following domain model we want to insert into the database:

```csharp
internal class Product
{
    public ProductId ID { get; set; }
    public string ProductName { get; set; }
    public CategoryId CategoryId { get; set; }
    public int Price { get; set; }
}

```

We need to declare a writer mapping from the class to the database table:

```csharp
using Quib.Postgres18.ORMapper;

internal class ProductsWriter : IWriterMapping<ProductsTable, Model.Product>
{
    public static void Write(IUpdateQueryBuilder query, ProductsTable table, Model.Product product)
    {
        query.Set(table.ID, product.ID.Value);
        query.Set(table.ProductName, product.ProductName);
        query.Set(table.CategoryId, product.CategoryId.Value);
        query.Set(table.Price, product.Price);
    }
}
```

Insert a new product record:

```csharp
var product = new Product
{
    ID = new ProductId(6253),
    ProductName = "Nike Air Max 90",
    CategoryId = new CategoryId(1),
    Price = 129
};

await db.InsertInto<ProductsTable>()
    .AddRow<ProductsWriter, Product>(product)
    .Execute();
```

Updating uses the same writer mapping:

```csharp
await db.Update<ProductsTable>()
    .Where(x => x.ID == 7265)
    .With<ProductsWriter, Product>(product)
    .Execute();
```

You can also dynamically update only certain columns, either in all rows or a subset of rows:

```csharp
await db.Update<ProductsTable>()
    .AllRows()
    .With((query, table) =>
    {
        query.Set(table.Price, table.Price + 5);
    })
    .Execute();
```

Deleting rows is similar:

```csharp
await db.DeleteFrom<ProductsTable>()
    .Where(x => x.ID == 5329)
    .Execute();
```

Read [UpdateQueries.md](UpdateQueries.md) for more details.
