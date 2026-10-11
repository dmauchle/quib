using Quib.Postgres18.Schema;
using Quib.Postgres18.Types;

namespace CheatSheet.Schema;

internal class CategoriesTable(DbTable table) : IDbTable, IDbTable<CategoriesTable>
{
    public static string TableName => "categories";
    public static CategoriesTable CreateFrom(DbTable table) => new CategoriesTable(table);

    public NotNullColumn<DbInt> ID => field ??= table.Column<DbInt>("id");
    public NotNullColumn<DbText> CategoryName => field ??= table.Column<DbText>("categoryname");
    public NotNullColumn<DbText> Description => field ??= table.Column<DbText>("description");
}
