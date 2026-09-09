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


internal sealed class ConcatOperator<T> : QueryExpression<T>
    where T : struct, IExpressionType, IDbText
{

    private readonly QueryExpression _left;
    private readonly QueryExpression _right;

    public ConcatOperator(QueryExpression left, QueryExpression right)
    {
        _left = left;
        _right = right;
    }

    internal override void ToSql(QueryWriter query)
    {
#if POSTGRES
        // PostgreSQL uses || operator for string concatenation
        query.Write("(");
        query.Write(_left);
        query.Write(" || ");
        query.Write(_right);
        query.Write(")");
#elif MYSQL
        // MySQL uses CONCAT function
        query.Write("CONCAT(");
        query.Write(_left);
        query.Write(", ");
        query.Write(_right);
        query.Write(")");
#endif
    }

}
