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

internal interface ISelectQuery : IQueryPart
{

}

public sealed class SelectQuery<TQuery> : ISelectQuery
{

    private readonly QueryFactory _con;
    private readonly SelectQueryBuilder _builder;
    private readonly TQuery _accessor;

    internal SelectQuery(QueryFactory con, SelectQueryBuilder builder, TQuery accessor)
    {
        _con = con;
        _builder = builder;
        _accessor = accessor;
    }

    public SelectQuery<TResult> Select<TResult>(Func<TQuery, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return new SelectQuery<TResult>(_con, _builder, selector(_accessor));
    }

    public SelectQuery<TResult> Select<TResult>(Func<SelectQueryBuilder, TQuery, TResult> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return new SelectQuery<TResult>(_con, _builder, selector(_builder, _accessor));
    }

    public SelectQuery<TQuery> Where(Func<TQuery, NullableExpression<DbBool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        _builder.AddFilter(predicate(_accessor).SQL);
        return this;
    }

    public SelectQuery<TQuery> OrderBy(Func<TQuery, ISortExpression[]> sort)
    {
        ArgumentNullException.ThrowIfNull(sort);

        var order = sort(_accessor);
        // Otherwise the ORDER BY clause would silently be dropped (null array) or fail later when building the SQL
        if (order == null || Array.Exists(order, x => x == null))
            throw new ArgumentException("Sort expressions must not be null.", nameof(sort));

        _builder.SetOrder(order);
        return this;
    }

    /// <summary>
    /// Limits the number of returned rows (LIMIT clause), optionally skipping the first <paramref name="offset"/> rows.
    /// Use together with <see cref="OrderBy"/>, otherwise the returned rows are unpredictable.
    /// </summary>
    public SelectQuery<TQuery> Limit(int limit, long offset = 0)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(limit);
        ArgumentOutOfRangeException.ThrowIfNegative(offset);

        _builder.SetLimit(limit, offset);
        return this;
    }

    public ProjectedQuery<TProj> Project<TProj>(Func<ProjectionBuilder, TQuery, TProj> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        var rs = new ProjectionBuilder();
        var proj = selector(rs, _accessor);
        return new ProjectedQuery<TProj>(_con, proj, this, rs.Build());
    }

    public ProjectedQuery<NotNullProjection<TProj>> Project<TProj>(Func<TQuery, NotNullExpression<TProj>> selector)
        where TProj : struct, IExpressionType
    {
        ArgumentNullException.ThrowIfNull(selector);

        return this.Project((rs, x) => rs.Get(selector(x)));
    }

    public ProjectedQuery<NullableProjection<TProj>> Project<TProj>(Func<TQuery, NullableExpression<TProj>> selector)
        where TProj : struct, IExpressionType
    {
        ArgumentNullException.ThrowIfNull(selector);

        return this.Project((rs, x) => rs.Get(selector(x)));
    }

    public ProjectedMap<TQuery, TObj> Map<TMapper, TObj>()
        where TMapper : IReaderMapping<TQuery, TObj>
        where TObj : notnull
    {
        var rs = new ProjectionBuilder();
        var proj = this.Project(TMapper.CreateFrom);
        return new ProjectedMap<TQuery, TObj>(proj);
    }

    public ProjectedMap<TQuery, TObj> Map<TObj>(Func<ProjectionBuilder, TQuery, IReaderMapping<TObj>> factory)
        where TObj : notnull
    {
        ArgumentNullException.ThrowIfNull(factory);

        var rs = new ProjectionBuilder();
        var proj = this.Project(factory);
        return new ProjectedMap<TQuery, TObj>(proj);
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        _builder.ToSql(query);
    }


}
