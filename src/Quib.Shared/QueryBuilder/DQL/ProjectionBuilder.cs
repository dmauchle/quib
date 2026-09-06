#if MYSQL
namespace Quib.MySql.QueryBuilder;
#elif POSTGRES
namespace Quib.Postgres.QueryBuilder;
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
        where T : struct, IDbType
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