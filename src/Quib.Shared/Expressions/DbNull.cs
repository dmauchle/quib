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


public sealed class DbNull : QueryExpression
{

    public static readonly DbNull Value = new DbNull();

    private DbNull()
    {

    }

    public QueryExpression<T> As<T>()
        where T : struct, INullableExpressionType
    {
        return DbNull<T>.Value;
    }

    internal override void ToSql(QueryWriter query)
    {
        query.Write("NULL");
    }
}

internal sealed class DbNull<T> : QueryExpression<T>
    where T : struct, INullableExpressionType
{

    public static readonly DbNull<T> Value = new DbNull<T>();

    private DbNull()
    {

    }

    internal override void ToSql(QueryWriter query)
    {
        query.Write(DbNull.Value);
    }
}