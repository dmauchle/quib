#if MYSQL
namespace Quib.MySql.QueryBuilder;
#elif POSTGRES
namespace Quib.Postgres.QueryBuilder;
#endif


internal interface IQueryParameter : IQueryPart
{
    public string Name { get; }
    public object? Value { get; }
}

internal sealed class QueryParameter<T>(int index, object? value) : IQueryParameter, IQueryExpression<T>
    where T : struct, IDbType
{

    public string Name => $"p{index}";
    public object? Value => value;

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("@p");
        query.Write(index);
    }
}