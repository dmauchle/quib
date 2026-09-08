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


internal interface IQueryParameter : IQueryPart
{
    public string Name { get; }
    public object? Value { get; }
}

internal sealed class QueryParameter<T>(int index, object? value) : IQueryParameter, IQueryExpression<T>
    where T : struct, IExpressionType
{

    public string Name => $"p{index}";
    public object? Value => value;

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("@p");
        query.Write(index);
    }
}