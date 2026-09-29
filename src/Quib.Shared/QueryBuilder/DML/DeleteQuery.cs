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


public interface IDeleteQuery<TQuery> : IDeleteFilterable<TQuery>, IQueryPart
{

    public IDeleteUnfiltered<TQuery> AllRows();

}

public interface IDeleteFiltered<TQuery> : IDeleteFilterable<TQuery>, IDeleteExecutable<TQuery>, IQueryPart
{
}

public interface IDeleteUnfiltered<TQuery> : IDeleteExecutable<TQuery>, IQueryPart
{
}

public interface IDeleteFilterable<TQuery> : IQueryPart
{
    public IDeleteFiltered<TQuery> Where<TBool>(Func<TQuery, QueryExpression<TBool>> predicate) where TBool : struct, IExpressionType, IDbBool;
}

public interface IDeleteExecutable<TQuery> : IQueryPart
{
    public Task<int> Execute(CancellationToken cancellationToken = default);
}


internal sealed class DeleteQuery<TQuery> : IDeleteQuery<TQuery>, IDeleteFiltered<TQuery>, IDeleteUnfiltered<TQuery>, IQueryPart
{

    private readonly QueryFactory _con;
    private readonly DeleteQueryBuilder _builder;
    private readonly TQuery _accessor;

    internal DeleteQuery(QueryFactory con, DeleteQueryBuilder queryBuilder, TQuery accessor)
    {
        _con = con;
        _builder = queryBuilder;
        _accessor = accessor;
    }

    public IDeleteUnfiltered<TQuery> AllRows()
    {
        return this;
    }

    public IDeleteFiltered<TQuery> Where<TBool>(Func<TQuery, QueryExpression<TBool>> predicate)
        where TBool : struct, IExpressionType, IDbBool
    {
        _builder.AddFilter(predicate(_accessor));
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
