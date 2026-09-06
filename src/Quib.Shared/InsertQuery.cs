#if MYSQL
namespace Quib.MySql;
#elif POSTGRES
namespace Quib.Postgres;
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

    public InsertQuery<TResult> Select<TResult>(Func<TQuery, TResult> selector)
    {
        return new InsertQuery<TResult>(_con, _builder, selector(_accessor));
    }

    public InsertQuery<TQuery> AddRow(Action<InsertQueryBuilder, TQuery> run)
    {
        run(_builder, _accessor);
        _builder.NextRow();
        return this;
    }

    public InsertQuery<TQuery> AddRow<TObj>(Action<InsertQueryBuilder, TQuery, TObj> run, TObj obj)
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

    public async Task<int> Execute()
    {
        var sql = ToSql();
        var parameters = _builder.GetParameters();
        var affected = _con.ExecuteNonQuery(sql, parameters);
        return affected;
    }

    internal string ToSql()
    {
        var writer = new QueryWriter();
        writer.Write(this);
        return writer.ToString();
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        _builder.ToSql(query);
    }

}
