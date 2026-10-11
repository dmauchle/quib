using CheatSheet.Model;
using Quib.Postgres18.ORMapper;
using Quib.Postgres18.Types;

namespace CheatSheet.Schema;

internal class ProductsReader(IProjectionBuilder query, ProductsTable product) : IReaderMapping<ProductsTable, Product>
{

    static IReaderMapping<Product> IReaderMapping<ProductsTable, Product>.CreateFrom(IProjectionBuilder rs, ProductsTable query) => new ProductsReader(rs, query);

    private readonly NotNullProjection<DbInt> _id = query.Get(product.ID);
    private readonly NotNullProjection<DbText> _productName = query.Get(product.ProductName);
    private readonly NotNullProjection<DbInt> _categoryId = query.Get(product.CategoryId);
    private readonly NotNullProjection<DbInt> _price = query.Get(product.Price);

    IProjectionField[] IReaderMapping<Product>.ObjectIdentity => [ _id ];

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
