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

    extension(QueryExpression<DbBool> value)
    {

        public QueryExpression<DbBoolNullable> AsNullable()
        {
            return new ImplicitCast<DbBoolNullable>(value);
        }


        public static QueryExpression<DbBool> operator ==(QueryExpression<DbBool> left, bool right)
        {
            return new BinaryOperator<DbBool>(left, "=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBool> operator !=(QueryExpression<DbBool> left, bool right)
        {
            return new BinaryOperator<DbBool>(left, "!=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBoolNullable> operator ==(QueryExpression<DbBool> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "=", right);
        }

        public static QueryExpression<DbBoolNullable> operator !=(QueryExpression<DbBool> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "!=", right);
        }

        public static QueryExpression<DbBool> operator <(QueryExpression<DbBool> left, QueryExpression<DbBool> right)
        {
            return new BinaryOperator<DbBool>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbBool> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBool> operator <(QueryExpression<DbBool> left, bool right)
        {
            return left < SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator >(QueryExpression<DbBool> left, QueryExpression<DbBool> right)
        {
            return new BinaryOperator<DbBool>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbBool> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBool> operator >(QueryExpression<DbBool> left, bool right)
        {
            return left > SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator <=(QueryExpression<DbBool> left, QueryExpression<DbBool> right)
        {
            return new BinaryOperator<DbBool>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbBool> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBool> operator <=(QueryExpression<DbBool> left, bool right)
        {
            return left <= SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator >=(QueryExpression<DbBool> left, QueryExpression<DbBool> right)
        {
            return new BinaryOperator<DbBool>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbBool> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBool> operator >=(QueryExpression<DbBool> left, bool right)
        {
            return left >= SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator &(QueryExpression<DbBool> left, QueryExpression<DbBool> right)
        {
            return new BinaryOperator<DbBool>(left, "AND", right);
        }

        public static QueryExpression<DbBoolNullable> operator &(QueryExpression<DbBool> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left.AsNullable(), "AND", right);
        }

        public static QueryExpression<DbBool> operator &(QueryExpression<DbBool> left, bool right)
        {
            return left & SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator |(QueryExpression<DbBool> left, QueryExpression<DbBool> right)
        {
            return new BinaryOperator<DbBool>(left, "OR", right);
        }

        public static QueryExpression<DbBoolNullable> operator |(QueryExpression<DbBool> left, QueryExpression<DbBoolNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left.AsNullable(), "OR", right);
        }

        public static QueryExpression<DbBool> operator |(QueryExpression<DbBool> left, bool right)
        {
            return left | SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator !(QueryExpression<DbBool> operand)
        {
            return new UnaryOperator<DbBool>("NOT", operand);
        }

    }

}
