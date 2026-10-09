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

    extension(QueryExpression<DbDecimal> value)
    {

        public QueryExpression<DbDecimalNullable> AsNullable()
        {
            return new ImplicitCast<DbDecimalNullable>(value);
        }

        public static QueryExpression<DbDecimal> operator +(QueryExpression<DbDecimal> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbDecimal>(left, "+", right);
        }

        public static QueryExpression<DbDecimalNullable> operator +(QueryExpression<DbDecimal> left, QueryExpression<DbDecimalNullable> right)
        {
            return left.AsNullable() + right;
        }

        public static QueryExpression<DbDecimal> operator +(QueryExpression<DbDecimal> left, decimal right)
        {
            return left + SQL.Parameter(right);
        }

        public static QueryExpression<DbDecimal> operator -(QueryExpression<DbDecimal> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbDecimal>(left, "-", right);
        }

        public static QueryExpression<DbDecimalNullable> operator -(QueryExpression<DbDecimal> left, QueryExpression<DbDecimalNullable> right)
        {
            return left.AsNullable() - right;
        }

        public static QueryExpression<DbDecimal> operator -(QueryExpression<DbDecimal> left, decimal right)
        {
            return left - SQL.Parameter(right);
        }

        public static QueryExpression<DbDecimal> operator *(QueryExpression<DbDecimal> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbDecimal>(left, "*", right);
        }

        public static QueryExpression<DbDecimalNullable> operator *(QueryExpression<DbDecimal> left, QueryExpression<DbDecimalNullable> right)
        {
            return left.AsNullable() * right;
        }

        public static QueryExpression<DbDecimal> operator *(QueryExpression<DbDecimal> left, decimal right)
        {
            return left * SQL.Parameter(right);
        }

        public static QueryExpression<DbDecimal> operator /(QueryExpression<DbDecimal> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbDecimal>(left, "/", right);
        }

        public static QueryExpression<DbDecimalNullable> operator /(QueryExpression<DbDecimal> left, QueryExpression<DbDecimalNullable> right)
        {
            return left.AsNullable() / right;
        }

        public static QueryExpression<DbDecimal> operator /(QueryExpression<DbDecimal> left, decimal right)
        {
            return left / SQL.Parameter(right);
        }

        public static QueryExpression<DbDecimal> operator %(QueryExpression<DbDecimal> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbDecimal>(left, "%", right);
        }

        public static QueryExpression<DbDecimalNullable> operator %(QueryExpression<DbDecimal> left, QueryExpression<DbDecimalNullable> right)
        {
            return left.AsNullable() % right;
        }

        public static QueryExpression<DbDecimal> operator %(QueryExpression<DbDecimal> left, decimal right)
        {
            return left % SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator ==(QueryExpression<DbDecimal> left, decimal right)
        {
            return new BinaryOperator<DbBool>(left, "=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBool> operator !=(QueryExpression<DbDecimal> left, decimal right)
        {
            return new BinaryOperator<DbBool>(left, "!=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBoolNullable> operator ==(QueryExpression<DbDecimal> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "=", right);
        }

        public static QueryExpression<DbBoolNullable> operator !=(QueryExpression<DbDecimal> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "!=", right);
        }

        public static QueryExpression<DbBool> operator <(QueryExpression<DbDecimal> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbBool>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbDecimal> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBool> operator <(QueryExpression<DbDecimal> left, decimal right)
        {
            return left < SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator >(QueryExpression<DbDecimal> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbBool>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbDecimal> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBool> operator >(QueryExpression<DbDecimal> left, decimal right)
        {
            return left > SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator <=(QueryExpression<DbDecimal> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbBool>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbDecimal> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBool> operator <=(QueryExpression<DbDecimal> left, decimal right)
        {
            return left <= SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator >=(QueryExpression<DbDecimal> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbBool>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbDecimal> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBool> operator >=(QueryExpression<DbDecimal> left, decimal right)
        {
            return left >= SQL.Parameter(right);
        }

    }



}