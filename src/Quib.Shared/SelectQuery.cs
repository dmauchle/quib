#if MYSQL
namespace Quib.MySql;
#elif POSTGRES
namespace Quib.Postgres;
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

    public ProjectedQuery<TProj> Project<TProj>(Func<ProjectionBuilder, TQuery, TProj> selector)
    {
        var rs = new ProjectionBuilder();
        var proj = selector(rs, _accessor);
        return new ProjectedQuery<TProj>(_con, proj, this, rs.Build());
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
