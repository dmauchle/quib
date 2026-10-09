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

    extension(QueryExpression<DbDateOnly> value)
    {

        public QueryExpression<DbDateOnlyNullable> AsNullable()
        {
            return new ImplicitCast<DbDateOnlyNullable>(value);
        }

        public static QueryExpression<DbBool> operator ==(QueryExpression<DbDateOnly> left, DateOnly right)
        {
            return new BinaryOperator<DbBool>(left, "=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBool> operator !=(QueryExpression<DbDateOnly> left, DateOnly right)
        {
            return new BinaryOperator<DbBool>(left, "!=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBoolNullable> operator ==(QueryExpression<DbDateOnly> left, QueryExpression<DbDateOnlyNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "=", right);
        }

        public static QueryExpression<DbBoolNullable> operator !=(QueryExpression<DbDateOnly> left, QueryExpression<DbDateOnlyNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "!=", right);
        }

        public static QueryExpression<DbBool> operator <(QueryExpression<DbDateOnly> left, QueryExpression<DbDateOnly> right)
        {
            return new BinaryOperator<DbBool>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbDateOnly> left, QueryExpression<DbDateOnlyNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBool> operator <(QueryExpression<DbDateOnly> left, DateOnly right)
        {
            return left < SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator >(QueryExpression<DbDateOnly> left, QueryExpression<DbDateOnly> right)
        {
            return new BinaryOperator<DbBool>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbDateOnly> left, QueryExpression<DbDateOnlyNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBool> operator >(QueryExpression<DbDateOnly> left, DateOnly right)
        {
            return left > SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator <=(QueryExpression<DbDateOnly> left, QueryExpression<DbDateOnly> right)
        {
            return new BinaryOperator<DbBool>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbDateOnly> left, QueryExpression<DbDateOnlyNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBool> operator <=(QueryExpression<DbDateOnly> left, DateOnly right)
        {
            return left <= SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator >=(QueryExpression<DbDateOnly> left, QueryExpression<DbDateOnly> right)
        {
            return new BinaryOperator<DbBool>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbDateOnly> left, QueryExpression<DbDateOnlyNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBool> operator >=(QueryExpression<DbDateOnly> left, DateOnly right)
        {
            return left >= SQL.Parameter(right);
        }

    }

}
