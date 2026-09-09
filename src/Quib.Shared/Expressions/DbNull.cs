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


public sealed class DbNull : IQueryExpression
{

    public static readonly DbNull Value = new DbNull();

    private DbNull()
    {

    }

    public IQueryExpression<T> As<T>()
        where T : struct, INullableExpressionType
    {
        return DbNull<T>.Value;
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("NULL");
    }
}

internal sealed class DbNull<T> : IQueryExpression<T>
    where T : struct, INullableExpressionType
{

    public static readonly DbNull<T> Value = new DbNull<T>();

    private DbNull()
    {

    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write(DbNull.Value);
    }
}