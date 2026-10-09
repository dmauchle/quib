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

    extension(QueryExpression<DbDateTime> value)
    {

        public QueryExpression<DbDateTimeNullable> AsNullable()
        {
            return new ImplicitCast<DbDateTimeNullable>(value);
        }

        public static QueryExpression<DbBool> operator ==(QueryExpression<DbDateTime> left, DateTime right)
        {
            return new BinaryOperator<DbBool>(left, "=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBool> operator !=(QueryExpression<DbDateTime> left, DateTime right)
        {
            return new BinaryOperator<DbBool>(left, "!=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBoolNullable> operator ==(QueryExpression<DbDateTime> left, QueryExpression<DbDateTimeNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "=", right);
        }

        public static QueryExpression<DbBoolNullable> operator !=(QueryExpression<DbDateTime> left, QueryExpression<DbDateTimeNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "!=", right);
        }

        public static QueryExpression<DbBool> operator <(QueryExpression<DbDateTime> left, QueryExpression<DbDateTime> right)
        {
            return new BinaryOperator<DbBool>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbDateTime> left, QueryExpression<DbDateTimeNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBool> operator <(QueryExpression<DbDateTime> left, DateTime right)
        {
            return left < SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator >(QueryExpression<DbDateTime> left, QueryExpression<DbDateTime> right)
        {
            return new BinaryOperator<DbBool>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbDateTime> left, QueryExpression<DbDateTimeNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBool> operator >(QueryExpression<DbDateTime> left, DateTime right)
        {
            return left > SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator <=(QueryExpression<DbDateTime> left, QueryExpression<DbDateTime> right)
        {
            return new BinaryOperator<DbBool>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbDateTime> left, QueryExpression<DbDateTimeNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBool> operator <=(QueryExpression<DbDateTime> left, DateTime right)
        {
            return left <= SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator >=(QueryExpression<DbDateTime> left, QueryExpression<DbDateTime> right)
        {
            return new BinaryOperator<DbBool>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbDateTime> left, QueryExpression<DbDateTimeNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBool> operator >=(QueryExpression<DbDateTime> left, DateTime right)
        {
            return left >= SQL.Parameter(right);
        }

    }

}
