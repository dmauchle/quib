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

    extension(QueryExpression<DbIntNullable> value)
    {
        public static QueryExpression<DbIntNullable> operator +(QueryExpression<DbIntNullable> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbIntNullable>(left, "+", right);
        }

        public static QueryExpression<DbIntNullable> operator +(QueryExpression<DbIntNullable> left, QueryExpression<DbInt> right)
        {
            return left + right.AsNullable();
        }

        public static QueryExpression<DbIntNullable> operator +(QueryExpression<DbIntNullable> left, int right)
        {
            return left + SQL.NullableParameter(right);
        }

        public static QueryExpression<DbIntNullable> operator -(QueryExpression<DbIntNullable> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbIntNullable>(left, "-", right);
        }

        public static QueryExpression<DbIntNullable> operator -(QueryExpression<DbIntNullable> left, QueryExpression<DbInt> right)
        {
            return left - right.AsNullable();
        }

        public static QueryExpression<DbIntNullable> operator -(QueryExpression<DbIntNullable> left, int right)
        {
            return left - SQL.NullableParameter(right);
        }

        public static QueryExpression<DbIntNullable> operator *(QueryExpression<DbIntNullable> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbIntNullable>(left, "*", right);
        }

        public static QueryExpression<DbIntNullable> operator *(QueryExpression<DbIntNullable> left, QueryExpression<DbInt> right)
        {
            return left * right.AsNullable();
        }

        public static QueryExpression<DbIntNullable> operator *(QueryExpression<DbIntNullable> left, int right)
        {
            return left * SQL.NullableParameter(right);
        }

        public static QueryExpression<DbIntNullable> operator /(QueryExpression<DbIntNullable> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbIntNullable>(left, "/", right);
        }

        public static QueryExpression<DbIntNullable> operator /(QueryExpression<DbIntNullable> left, QueryExpression<DbInt> right)
        {
            return left / right.AsNullable();
        }

        public static QueryExpression<DbIntNullable> operator /(QueryExpression<DbIntNullable> left, int right)
        {
            return left / SQL.NullableParameter(right);
        }

        public static QueryExpression<DbIntNullable> operator %(QueryExpression<DbIntNullable> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbIntNullable>(left, "%", right);
        }

        public static QueryExpression<DbIntNullable> operator %(QueryExpression<DbIntNullable> left, QueryExpression<DbInt> right)
        {
            return left % right.AsNullable();
        }

        public static QueryExpression<DbIntNullable> operator %(QueryExpression<DbIntNullable> left, int right)
        {
            return left % SQL.NullableParameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator ==(QueryExpression<DbIntNullable> left, int right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBoolNullable> operator !=(QueryExpression<DbIntNullable> left, int right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "!=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBoolNullable> operator ==(QueryExpression<DbIntNullable> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "=", right);
        }

        public static QueryExpression<DbBoolNullable> operator !=(QueryExpression<DbIntNullable> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "!=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbIntNullable> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbIntNullable> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbIntNullable> left, int right)
        {
            return left < SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbIntNullable> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbIntNullable> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbIntNullable> left, int right)
        {
            return left > SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbIntNullable> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbIntNullable> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbIntNullable> left, int right)
        {
            return left <= SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbIntNullable> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbIntNullable> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbIntNullable> left, int right)
        {
            return left >= SQL.Parameter(right);
        }

    }


}