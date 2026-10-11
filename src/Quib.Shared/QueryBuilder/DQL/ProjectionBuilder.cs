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


/// <summary>
/// Builder for the SELECT clause of a query. 
/// </summary>
public sealed class ProjectionBuilder : IProjectionBuilder
{

    private readonly List<IProjectionClause> _projections = [];

    internal ProjectionBuilder()
    {
    }

    /// <summary>
    /// Create and add a new SELECT field. Use the return value e.g. to retrieve the value from the result set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public NotNullProjection<T> Get<T>(NotNullExpression<T> value)
        where T : struct, IExpressionType
    {
        var field = new NotNullProjection<T>(_projections.Count);
        _projections.Add(new ProjectionClause(value.SQL, field.Alias));
        return field;
    }

    /// <inheritdoc cref="Get{T}(NotNullExpression{T})"/>
    public NullableProjection<T> Get<T>(NullableExpression<T> value)
        where T : struct, IExpressionType
    {
        var field = new NullableProjection<T>(_projections.Count);
        _projections.Add(new ProjectionClause(value.SQL, field.Alias));
        return field;
    }

    internal IProjectionClause[] Build()
    {
        return _projections.ToArray();
    }

}