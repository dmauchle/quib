#if MYSQL56
namespace Quib.MySql56.Expressions;
#elif MYSQL57
namespace Quib.MySql57.Expressions;
#elif MYSQL80
namespace Quib.MySql80.Expressions;
#elif MYSQL84
namespace Quib.MySql84.Expressions;
#elif POSTGRES18
namespace Quib.Postgres18.Expressions;
#endif


internal sealed record DbNull<T> : IQueryExpression<T>
    where T : struct, IExpressionType
{

    public static readonly DbNull<T> Value = new DbNull<T>();

    private DbNull()
    {

    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("NULL");
    }
}