using Quib.Postgres18.Schema;
using Quib.Postgres18.Types;

namespace CheatSheet.Schema;

internal class ProductsTable(DbTable table) : IDbTable, IDbTable<ProductsTable>
{

    public static string TableName => "products";
    public static ProductsTable CreateFrom(DbTable table) => new ProductsTable(table);

    public NotNullColumn<DbInt> ID => field ??= table.Column<DbInt>("id");
    public NotNullColumn<DbText> ProductName => field ??= table.Column<DbText>("productname");
    public NotNullColumn<DbInt> CategoryId => field ??= table.Column<DbInt>("categoryid");
    public NotNullColumn<DbInt> Price => field ??= table.Column<DbInt>("price");

}
