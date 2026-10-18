using Quib.Postgres18.Installer;

namespace CheatSheet.Migrations;

internal sealed class CatalogModule : IModuleMigration
{

    public static string ModuleName => "catalog";

    public static void Configure(ModuleMigration module)
    {
        module.AddVersion(1)
            .With((install, _) =>
            {
                install.Table("products")
                    .CreateWithPrimaryKey("id", ColumnType.Int())
                    .CreateColumn("product_name", ColumnType.VarChar(32).NotNull())
                    .CreateColumn("category_id", ColumnType.Int().NotNull())
                    .CreateColumn("price", ColumnType.Int().NotNull());
            });

        module.AddVersion(2)
            .With((install, _) =>
            {
                var categories = install.Table("categories");
                categories.CreateColumn("description", ColumnType.VarChar(128).NotNull());
                categories.Column("price").RenameTo("unit_price");
            });

    }
}
