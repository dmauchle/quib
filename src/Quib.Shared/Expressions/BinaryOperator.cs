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


internal sealed class BinaryOperator<T> : QueryExpression<T>
    where T : struct, IExpressionType
{

    private readonly QueryExpression _left;
    private readonly QueryExpression _right;
    private readonly string _sqlOperator;

    public BinaryOperator(QueryExpression left, string sqlOperator, QueryExpression right)
    {
        _left = left;
        _right = right;
        _sqlOperator = sqlOperator;
    }

    internal override void ToSql(QueryWriter query)
    {
        query.Write("(");
        query.Write(_left);
        query.Write(" ");
        query.Write(_sqlOperator);
        query.Write(" ");
        query.Write(_right);
        query.Write(")");
    }

}