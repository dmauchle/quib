#if MYSQL56
namespace Quib.MySql56;
#elif MYSQL57
namespace Quib.MySql57;
#elif MYSQL80
namespace Quib.MySql80;
#elif MYSQL84
namespace Quib.MySql84;
#elif POSTGRES18
namespace Quib.Postgres18;
#endif


public static partial class ExpressionExtensions
{

    extension(QueryExpression<DbBoolNullable> value)
    {

        public static QueryExpression<DbBoolNullable> operator ==(QueryExpression<DbBoolNullable> left, bool right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBoolNullable> operator !=(QueryExpression<DbBoolNullable> left, bool right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "!=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbBoolNullable> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbBoolNullable> left, QueryExpression<DbBool> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbBoolNullable> left, bool right)
        {
            return left < SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbBoolNullable> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbBoolNullable> left, QueryExpression<DbBool> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbBoolNullable> left, bool right)
        {
            return left > SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbBoolNullable> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbBoolNullable> left, QueryExpression<DbBool> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbBoolNullable> left, bool right)
        {
            return left <= SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbBoolNullable> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbBoolNullable> left, QueryExpression<DbBool> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbBoolNullable> left, bool right)
        {
            return left >= SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator &(QueryExpression<DbBoolNullable> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "AND", right);
        }

        public static QueryExpression<DbBoolNullable> operator &(QueryExpression<DbBoolNullable> left, QueryExpression<DbBool> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "AND", right);
        }

        public static QueryExpression<DbBoolNullable> operator &(QueryExpression<DbBoolNullable> left, bool right)
        {
            return left & SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator |(QueryExpression<DbBoolNullable> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "OR", right);
        }

        public static QueryExpression<DbBoolNullable> operator |(QueryExpression<DbBoolNullable> left, QueryExpression<DbBool> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "OR", right);
        }

        public static QueryExpression<DbBoolNullable> operator |(QueryExpression<DbBoolNullable> left, bool right)
        {
            return left | SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator !(QueryExpression<DbBoolNullable> operand)
        {
            return new UnaryOperator<DbBoolNullable>("NOT", operand);
        }

    }

}
