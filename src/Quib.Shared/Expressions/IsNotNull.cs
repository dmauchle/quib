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


internal sealed class IsNotNull : IQueryExpression
{

    private readonly IQueryExpression _value;

    public IsNotNull(IQueryExpression value)
    {
        _value = value;
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("(");
        query.Write(_value);
        query.Write(" IS NOT NULL)");
    }

}
