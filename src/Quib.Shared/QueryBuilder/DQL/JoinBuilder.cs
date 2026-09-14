#if MYSQL56
namespace Quib.MySql56.QueryBuilder;
#elif MYSQL57
namespace Quib.MySql57.QueryBuilder;
#elif MYSQL80
namespace Quib.MySql80.QueryBuilder;
#elif MYSQL84
namespace Quib.MySql84.QueryBuilder;
#elif POSTGRES18
namespace Quib.Postgres18.QueryBuilder;
#endif


public class JoinBuilder<TQuery>
{

    private readonly JoinClause _join;
    private readonly TQuery _accessor;

    internal JoinBuilder(JoinClause join, TQuery accessor)
    {
        _join = join;
        _accessor = accessor;
    }

    internal JoinBuilder<T> Select<T>(Func<TQuery, T> factory)
    {
        var newAccessor = factory(_accessor);
        return new JoinBuilder<T>(_join, newAccessor);
    }

    public TQuery On<TBool>(Func<TQuery, QueryExpression<TBool>> condition)
        where TBool : struct, IExpressionType, IDbBool
    {
        _join.On(condition(_accessor));
        return _accessor;
    }

}