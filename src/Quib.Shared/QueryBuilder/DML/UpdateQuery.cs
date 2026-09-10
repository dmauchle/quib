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


public interface IUpdateTable<TQuery> : IUpdateFilterable<TQuery>, IQueryPart
{

    public IUpdateUnfiltered<TQuery> AllRows();

}

public interface IUpdateFilterable<TQuery> : IQueryPart
{
    public IUpdateFiltered<TQuery> Where<TBool>(Func<TQuery, QueryExpression<TBool>> predicate) where TBool : struct, IExpressionType, IDbBool;
}

public interface IUpdateSettable<TQuery> : IQueryPart
{
    public IUpdateExecutable<TQuery> With(Action<UpdateQueryBuilder, TQuery> run);
    public IUpdateExecutable<TQuery> With<TObj>(Action<UpdateQueryBuilder, TQuery, TObj> run, TObj obj);
    public IUpdateExecutable<TQuery> With<TMapper, TObj>(TObj obj) where TMapper : IWriterMapping<TQuery, TObj>;
}

public interface IUpdateFiltered<TQuery> : IUpdateFilterable<TQuery>, IUpdateExecutable<TQuery>, IQueryPart
{
}

public interface IUpdateUnfiltered<TQuery> : IUpdateSettable<TQuery>, IQueryPart
{
}

public interface IUpdateExecutable<TQuery> : IUpdateSettable<TQuery>, IQueryPart
{
    public Task<int> Execute(CancellationToken cancellationToken = default);
}


internal sealed class UpdateQuery<TQuery> : IUpdateTable<TQuery>, IUpdateFiltered<TQuery>, IUpdateUnfiltered<TQuery>, IQueryPart
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

    public IUpdateUnfiltered<TQuery> AllRows()
    {
        return this;
    }

    public IUpdateFiltered<TQuery> Where<TBool>(Func<TQuery, QueryExpression<TBool>> predicate)
        where TBool : struct, IExpressionType, IDbBool
    {
        _builder.Filter(predicate(_accessor));
        return this;
    }

    public IUpdateExecutable<TQuery> With(Action<UpdateQueryBuilder, TQuery> run)
    {
        run(_builder, _accessor);
        return this;
    }

    public IUpdateExecutable<TQuery> With<TObj>(Action<UpdateQueryBuilder, TQuery, TObj> run, TObj obj)
    {
        run(_builder, _accessor, obj);
        return this;
    }

    public IUpdateExecutable<TQuery> With<TMapper, TObj>(TObj obj)
        where TMapper : IWriterMapping<TQuery, TObj>
    {
        this.With(TMapper.Write, obj);
        return this;
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
