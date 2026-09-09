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


internal sealed class UnaryOperator<T> : QueryExpression<T>
    where T : struct, IExpressionType
{

    private readonly QueryExpression<T> _operand;
    private readonly string _sqlOperator;

    public UnaryOperator(string sqlOperator, QueryExpression<T> operand)
    {
        _operand = operand;
        _sqlOperator = sqlOperator;
    }

    internal override void ToSql(QueryWriter query)
    {
        query.Write("(");
        query.Write(_sqlOperator);
        query.Write(" ");
        query.Write(_operand);
        query.Write(")");
    }

}
