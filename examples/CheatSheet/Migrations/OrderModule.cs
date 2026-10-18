using Quib.Postgres18.Installer;
using Quib.Postgres18.ORMapper;
using Quib.Postgres18.Types;

namespace CheatSheet.Migrations;

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
                    .CreateColumn("comment", ColumnType.VarChar(265).Nullable());

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
