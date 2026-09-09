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

internal interface IUpdateQuery : IQueryPart
{

}

public sealed class UpdateQuery<TQuery> : IUpdateQuery
{

    private readonly QueryFactory _con;
    private readonly UpdateQueryBuilder _builder;
    private readonly TQuery _accessor;

    internal UpdateQuery(QueryFactory con, UpdateQueryBuilder queryBuilder, TQuery accessor)
    {
        _con = con;
        _builder = queryBuilder;
        _accessor = accessor;
    }

    public UpdateQuery<TQuery> With(Action<UpdateQueryBuilder, TQuery> run)
    {
        run(_builder, _accessor);
        return this;
    }

    public UpdateQuery<TQuery> With<TObj>(Action<UpdateQueryBuilder, TQuery, TObj> run, TObj obj)
    {
        run(_builder, _accessor, obj);
        return this;
    }

    public UpdateQuery<TQuery> With<TMapper, TObj>(TObj obj)
        where TMapper : IWriterMapping<TQuery, TObj>
    {
        return this.With(TMapper.Write, obj);
    }

    public Task<int> Execute(CancellationToken cancellationToken = default)
    {
        var query = new QueryWriter();
        query.Write(this);
        return _con.ExecuteNonQueryAsync(query, cancellationToken);
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        _builder.ToSql(query);
    }

}
