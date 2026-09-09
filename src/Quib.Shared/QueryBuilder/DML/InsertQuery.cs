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

internal interface IInsertQuery : IQueryPart
{

}

public sealed class InsertQuery<TQuery> : IInsertQuery
{

    private readonly QueryFactory _con;
    private readonly InsertQueryBuilder _builder;
    private readonly TQuery _accessor;

    internal InsertQuery(QueryFactory con, InsertQueryBuilder builder, TQuery accessor)
    {
        _con = con;
        _builder = builder;
        _accessor = accessor;
    }

    public InsertQuery<TQuery> AddRow(Action<IUpdateQueryBuilder, TQuery> run)
    {
        run(_builder, _accessor);
        _builder.NextRow();
        return this;
    }

    public InsertQuery<TQuery> AddRow<TObj>(Action<IUpdateQueryBuilder, TQuery, TObj> run, TObj obj)
    {
        run(_builder, _accessor, obj);
        _builder.NextRow();
        return this;
    }

    public InsertQuery<TQuery> AddRow<TMapper, TObj>(TObj obj)
        where TMapper : IWriterMapping<TQuery, TObj>
    {
        return this.AddRow(TMapper.Write, obj);
    }

    public async Task<int> Execute(CancellationToken cancellationToken = default)
    {
        var query = new QueryWriter();
        query.Write(this);
        var affected = await _con.ExecuteNonQueryAsync(query, cancellationToken);
        return affected;
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        _builder.ToSql(query);
    }

}
