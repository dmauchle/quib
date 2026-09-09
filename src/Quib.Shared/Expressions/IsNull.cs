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


internal sealed class IsNull : QueryExpression<DbBool>
{

    private readonly QueryExpression _value;

    public IsNull(QueryExpression value)
    {
        _value = value;
    }

    internal override void ToSql(QueryWriter query)
    {
        query.Write("(");
        query.Write(_value);
        query.Write(" IS NULL)");
    }

}
