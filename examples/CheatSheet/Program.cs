using CheatSheet.Model;
using CheatSheet.Schema;
using Npgsql;
using Quib.Postgres18;
using Quib.Postgres18.Installer;
using Migrations = CheatSheet.Migrations;
using static Quib.Postgres18.QueryBuilder.Sort;


using var connection = new NpgsqlConnection("...");
await connection.OpenAsync();

using (var installer = new DbInstaller(connection))
{
    var migration = new DbMigration("dbversions");
    migration.Add<Migrations.CatalogModule>();
    migration.Add<Migrations.OrderModule>();
    await installer.RunMigration(migration);
}

var db = new QueryFactory(connection);

// Query a table
var rs1 = await db
    .SelectFrom<ProductsTable>()
    .Where(x => x.CategoryId == 1 | x.CategoryId == 2)
    .Where(x => x.Price < 1000)
    .OrderBy(x => [SQL.Sort(x.ProductName, ASC)])
    .Map<ProductsReader, Product>()
    .FetchArray();

var rs2 = await db.SelectFrom<ProductsTable>()
    .Where(x => x.ID == 7265)
    .Map<ProductsReader, Product>()
    .FetchOne();

// Join's
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



var product = new Product
{
    ID = new ProductId(6253),
    ProductName = "Nike Air Max 90",
    CategoryId = new CategoryId(1),
    Price = 129
};

// Insert a row into a table
await db.InsertInto<ProductsTable>()
    .AddRow<ProductsWriter, Product>(product)
    .Execute();

// Update a table row
await db.Update<ProductsTable>()
    .Where(x => x.ID == 7265)
    .With<ProductsWriter, Product>(product)
    .Execute();

// Update multiple rows
await db.Update<ProductsTable>()
    .AllRows()
    .With((query, table) =>
    {
        query.Set(table.Price, table.Price + 5);
    })
    .Execute();

// Delete a row
await db.DeleteFrom<ProductsTable>()
    .Where(x => x.ID == 5329)
    .Execute();
