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

public sealed class ProjectionBuilder
{

    private readonly List<IProjectionClause> _projections = [];

    internal ProjectionBuilder()
    {
    }

    /// <summary>
    /// Create and add a new projection clause.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public ProjectedValue<T> Get<T>(IQueryExpression<T> value)
        where T : struct, IExpressionType
    {
        var fieldIndex = _projections.Count;
        var alias = new ProjectedValue<T>(fieldIndex);
        var proj = new ProjectionClause<T>(value, alias);
        _projections.Add(proj);
        return alias;
    }

    internal IProjectionClause[] Build()
    {
        return _projections.ToArray();
    }

}