using Quib.Postgres18.ORMapper;

namespace CheatSheet.Schema;

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
