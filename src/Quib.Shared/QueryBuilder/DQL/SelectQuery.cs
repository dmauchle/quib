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
        return new SelectQuery<TResult>(_con, _builder, selector(_accessor));
    }

    public SelectQuery<TQuery> Where<TBool>(Func<TQuery, QueryExpression<TBool>> predicate)
        where TBool : struct, IExpressionType, IDbBool
    {
        _builder.Filter(predicate(_accessor));
        return this;
    }

    public SelectQuery<TQuery> OrderBy(Func<TQuery, ISortExpression[]> sort)
    {
        _builder.OrderBy(sort(_accessor));
        return this;
    }

    public ProjectedQuery<TProj> Project<TProj>(Func<ProjectionBuilder, TQuery, TProj> selector)
    {
        var rs = new ProjectionBuilder();
        var proj = selector(rs, _accessor);
        return new ProjectedQuery<TProj>(_con, proj, this, rs.Build());
    }

    public ProjectedQuery<ProjectedValue<TProj>> Project<TProj>(Func<TQuery, QueryExpression<TProj>> selector)
        where TProj : struct, IExpressionType
    {
        return this.Project((rs, x) => rs.Get(selector(x)));
    }

    public ProjectedMap<TQuery, TObj> Map<TMapper, TObj>()
        where TMapper : IReaderMappingFactory<TQuery, TObj>
    {
        var rs = new ProjectionBuilder();
        var proj = this.Project(TMapper.Factory);
        return new ProjectedMap<TQuery, TObj>(proj);
    }

    public ProjectedMap<TQuery, TObj> Map<TObj>(Func<ProjectionBuilder, TQuery, IReaderMapping<TQuery, TObj>> factory)
    {
        var rs = new ProjectionBuilder();
        var proj = this.Project(factory);
        return new ProjectedMap<TQuery, TObj>(proj);
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        _builder.ToSql(query);
    }

}
