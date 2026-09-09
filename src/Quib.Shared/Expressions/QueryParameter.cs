#if MYSQL56
namespace Quib.MySql56.Expressions;
#elif MYSQL57
namespace Quib.MySql57.Expressions;
#elif MYSQL80
namespace Quib.MySql80.Expressions;
#elif MYSQL84
namespace Quib.MySql84.Expressions;
#elif POSTGRES18
namespace Quib.Postgres18.Expressions;
#endif


internal interface IQueryParameter
{
    public object? Value { get; }
}

internal sealed class QueryParameter<T> : QueryExpression<T>, IQueryParameter
    where T : struct, IExpressionType
{

    private readonly object? _value;

    public QueryParameter(object? value)
    {
        _value = value;
    }

    object? IQueryParameter.Value => _value;

    internal override void ToSql(QueryWriter query)
    {
        query.WriteParameter(this);
    }

}