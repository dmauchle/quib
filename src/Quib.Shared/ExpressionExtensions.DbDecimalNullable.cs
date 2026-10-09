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

    extension(QueryExpression<DbDecimalNullable> value)
    {
        public static QueryExpression<DbDecimalNullable> operator +(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbDecimalNullable>(left, "+", right);
        }

        public static QueryExpression<DbDecimalNullable> operator +(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimal> right)
        {
            return left + right.AsNullable();
        }

        public static QueryExpression<DbDecimalNullable> operator +(QueryExpression<DbDecimalNullable> left, decimal right)
        {
            return left + SQL.NullableParameter(right);
        }

        public static QueryExpression<DbDecimalNullable> operator -(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbDecimalNullable>(left, "-", right);
        }

        public static QueryExpression<DbDecimalNullable> operator -(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimal> right)
        {
            return left - right.AsNullable();
        }

        public static QueryExpression<DbDecimalNullable> operator -(QueryExpression<DbDecimalNullable> left, decimal right)
        {
            return left - SQL.NullableParameter(right);
        }

        public static QueryExpression<DbDecimalNullable> operator *(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbDecimalNullable>(left, "*", right);
        }

        public static QueryExpression<DbDecimalNullable> operator *(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimal> right)
        {
            return left * right.AsNullable();
        }

        public static QueryExpression<DbDecimalNullable> operator *(QueryExpression<DbDecimalNullable> left, decimal right)
        {
            return left * SQL.NullableParameter(right);
        }

        public static QueryExpression<DbDecimalNullable> operator /(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbDecimalNullable>(left, "/", right);
        }

        public static QueryExpression<DbDecimalNullable> operator /(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimal> right)
        {
            return left / right.AsNullable();
        }

        public static QueryExpression<DbDecimalNullable> operator /(QueryExpression<DbDecimalNullable> left, decimal right)
        {
            return left / SQL.NullableParameter(right);
        }

        public static QueryExpression<DbDecimalNullable> operator %(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbDecimalNullable>(left, "%", right);
        }

        public static QueryExpression<DbDecimalNullable> operator %(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimal> right)
        {
            return left % right.AsNullable();
        }

        public static QueryExpression<DbDecimalNullable> operator %(QueryExpression<DbDecimalNullable> left, decimal right)
        {
            return left % SQL.NullableParameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator ==(QueryExpression<DbDecimalNullable> left, decimal right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBoolNullable> operator !=(QueryExpression<DbDecimalNullable> left, decimal right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "!=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBoolNullable> operator ==(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "=", right);
        }

        public static QueryExpression<DbBoolNullable> operator !=(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "!=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbDecimalNullable> left, decimal right)
        {
            return left < SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbDecimalNullable> left, decimal right)
        {
            return left > SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbDecimalNullable> left, decimal right)
        {
            return left <= SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimalNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbDecimalNullable> left, QueryExpression<DbDecimal> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbDecimalNullable> left, decimal right)
        {
            return left >= SQL.Parameter(right);
        }

    }


}