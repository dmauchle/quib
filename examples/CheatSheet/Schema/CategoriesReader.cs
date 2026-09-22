using Quib.Postgres18.ORMapper;
using Quib.Postgres18.Types;

namespace CheatSheet.Schema;

internal class CategoriesReader(IProjectionBuilder rs, CategoriesTable table) : IReaderMapping<CategoriesTable, Model.Category>
{
    static IReaderMapping<Model.Category> IReaderMapping<CategoriesTable, Model.Category>.CreateFrom(IProjectionBuilder rs, CategoriesTable query) => new CategoriesReader(rs, query);

    private readonly ProjectedValue<DbInt> _id = rs.Get(table.ID);
    private readonly ProjectedValue<DbText> _categoryName = rs.Get(table.CategoryName);
    private readonly ProjectedValue<DbText> _description = rs.Get(table.Description);

    IProjectedValue[] IReaderMapping<Model.Category>.ObjectIdentity => [_id];

    Model.Category IReaderMapping<Model.Category>.Map(ResultReader rs)
    {
        return new Model.Category
        {
            ID = new Model.CategoryId(rs.Get(_id)),
            CategoryName = rs.Get(_categoryName),
            Description = rs.Get(_description)
        };
    }
}
