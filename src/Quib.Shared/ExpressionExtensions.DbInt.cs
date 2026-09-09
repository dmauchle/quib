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

    extension(QueryExpression<DbInt> value)
    {

        public QueryExpression<DbIntNullable> AsNullable()
        {
            return new ImplicitCast<DbIntNullable>(value);
        }

        public static QueryExpression<DbInt> operator +(QueryExpression<DbInt> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbInt>(left, "+", right);
        }

        public static QueryExpression<DbIntNullable> operator +(QueryExpression<DbInt> left, QueryExpression<DbIntNullable> right)
        {
            return left.AsNullable() + right;
        }

        public static QueryExpression<DbInt> operator +(QueryExpression<DbInt> left, int right)
        {
            return left + SQL.Parameter(right);
        }

        public static QueryExpression<DbInt> operator -(QueryExpression<DbInt> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbInt>(left, "-", right);
        }

        public static QueryExpression<DbIntNullable> operator -(QueryExpression<DbInt> left, QueryExpression<DbIntNullable> right)
        {
            return left.AsNullable() - right;
        }

        public static QueryExpression<DbInt> operator -(QueryExpression<DbInt> left, int right)
        {
            return left - SQL.Parameter(right);
        }

        public static QueryExpression<DbInt> operator *(QueryExpression<DbInt> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbInt>(left, "*", right);
        }

        public static QueryExpression<DbIntNullable> operator *(QueryExpression<DbInt> left, QueryExpression<DbIntNullable> right)
        {
            return left.AsNullable() * right;
        }

        public static QueryExpression<DbInt> operator *(QueryExpression<DbInt> left, int right)
        {
            return left * SQL.Parameter(right);
        }

        public static QueryExpression<DbInt> operator /(QueryExpression<DbInt> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbInt>(left, "/", right);
        }

        public static QueryExpression<DbIntNullable> operator /(QueryExpression<DbInt> left, QueryExpression<DbIntNullable> right)
        {
            return left.AsNullable() / right;
        }

        public static QueryExpression<DbInt> operator /(QueryExpression<DbInt> left, int right)
        {
            return left / SQL.Parameter(right);
        }

        public static QueryExpression<DbInt> operator %(QueryExpression<DbInt> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbInt>(left, "%", right);
        }

        public static QueryExpression<DbIntNullable> operator %(QueryExpression<DbInt> left, QueryExpression<DbIntNullable> right)
        {
            return left.AsNullable() % right;
        }

        public static QueryExpression<DbInt> operator %(QueryExpression<DbInt> left, int right)
        {
            return left % SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator ==(QueryExpression<DbInt> left, int right)
        {
            return new BinaryOperator<DbBool>(left, "=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBool> operator !=(QueryExpression<DbInt> left, int right)
        {
            return new BinaryOperator<DbBool>(left, "!=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBool> operator <(QueryExpression<DbInt> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbBool>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbInt> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBool> operator <(QueryExpression<DbInt> left, int right)
        {
            return left < SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator >(QueryExpression<DbInt> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbBool>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbInt> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBool> operator >(QueryExpression<DbInt> left, int right)
        {
            return left > SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator <=(QueryExpression<DbInt> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbBool>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbInt> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBool> operator <=(QueryExpression<DbInt> left, int right)
        {
            return left <= SQL.Parameter(right);
        }

        public static QueryExpression<DbBool> operator >=(QueryExpression<DbInt> left, QueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbBool>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbInt> left, QueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBool> operator >=(QueryExpression<DbInt> left, int right)
        {
            return left >= SQL.Parameter(right);
        }

    }

   

}