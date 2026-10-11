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

    extension(NotNullExpression<DbDecimal> value)
    {

        public static NotNullExpression<DbDecimal> operator +(NotNullExpression<DbDecimal> left, NotNullExpression<DbDecimal> right)
        {
            return new NotNullExpression<DbDecimal>(new BinaryOperator(left.SQL, "+", right.SQL));
        }

        public static NotNullExpression<DbDecimal> operator +(NotNullExpression<DbDecimal> left, decimal right)
        {
            return left + SQL.Parameter(right);
        }

        public static NotNullExpression<DbDecimal> operator -(NotNullExpression<DbDecimal> left, NotNullExpression<DbDecimal> right)
        {
            return new NotNullExpression<DbDecimal>(new BinaryOperator(left.SQL, "-", right.SQL));
        }

        public static NotNullExpression<DbDecimal> operator -(NotNullExpression<DbDecimal> left, decimal right)
        {
            return left - SQL.Parameter(right);
        }

        public static NotNullExpression<DbDecimal> operator *(NotNullExpression<DbDecimal> left, NotNullExpression<DbDecimal> right)
        {
            return new NotNullExpression<DbDecimal>(new BinaryOperator(left.SQL, "*", right.SQL));
        }

        public static NotNullExpression<DbDecimal> operator *(NotNullExpression<DbDecimal> left, decimal right)
        {
            return left * SQL.Parameter(right);
        }

        public static NotNullExpression<DbDecimal> operator /(NotNullExpression<DbDecimal> left, NotNullExpression<DbDecimal> right)
        {
            return new NotNullExpression<DbDecimal>(new BinaryOperator(left.SQL, "/", right.SQL));
        }

        public static NotNullExpression<DbDecimal> operator /(NotNullExpression<DbDecimal> left, decimal right)
        {
            return left / SQL.Parameter(right);
        }

        public static NotNullExpression<DbDecimal> operator %(NotNullExpression<DbDecimal> left, NotNullExpression<DbDecimal> right)
        {
            return new NotNullExpression<DbDecimal>(new BinaryOperator(left.SQL, "%", right.SQL));
        }

        public static NotNullExpression<DbDecimal> operator %(NotNullExpression<DbDecimal> left, decimal right)
        {
            return left % SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator ==(NotNullExpression<DbDecimal> left, decimal right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator !=(NotNullExpression<DbDecimal> left, decimal right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbDecimal> left, NotNullExpression<DbDecimal> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbDecimal> left, decimal right)
        {
            return left < SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbDecimal> left, NotNullExpression<DbDecimal> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbDecimal> left, decimal right)
        {
            return left > SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbDecimal> left, NotNullExpression<DbDecimal> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbDecimal> left, decimal right)
        {
            return left <= SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbDecimal> left, NotNullExpression<DbDecimal> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbDecimal> left, decimal right)
        {
            return left >= SQL.Parameter(right);
        }

    }

    extension(NullableExpression<DbDecimal> value)
    {

        public static NullableExpression<DbDecimal> operator +(NullableExpression<DbDecimal> left, NullableExpression<DbDecimal> right)
        {
            return new NullableExpression<DbDecimal>(new BinaryOperator(left.SQL, "+", right.SQL));
        }

        public static NullableExpression<DbDecimal> operator +(NullableExpression<DbDecimal> left, decimal right)
        {
            return left + SQL.Parameter(right);
        }

        public static NullableExpression<DbDecimal> operator -(NullableExpression<DbDecimal> left, NullableExpression<DbDecimal> right)
        {
            return new NullableExpression<DbDecimal>(new BinaryOperator(left.SQL, "-", right.SQL));
        }

        public static NullableExpression<DbDecimal> operator -(NullableExpression<DbDecimal> left, decimal right)
        {
            return left - SQL.Parameter(right);
        }

        public static NullableExpression<DbDecimal> operator *(NullableExpression<DbDecimal> left, NullableExpression<DbDecimal> right)
        {
            return new NullableExpression<DbDecimal>(new BinaryOperator(left.SQL, "*", right.SQL));
        }

        public static NullableExpression<DbDecimal> operator *(NullableExpression<DbDecimal> left, decimal right)
        {
            return left * SQL.Parameter(right);
        }

        public static NullableExpression<DbDecimal> operator /(NullableExpression<DbDecimal> left, NullableExpression<DbDecimal> right)
        {
            return new NullableExpression<DbDecimal>(new BinaryOperator(left.SQL, "/", right.SQL));
        }

        public static NullableExpression<DbDecimal> operator /(NullableExpression<DbDecimal> left, decimal right)
        {
            return left / SQL.Parameter(right);
        }

        public static NullableExpression<DbDecimal> operator %(NullableExpression<DbDecimal> left, NullableExpression<DbDecimal> right)
        {
            return new NullableExpression<DbDecimal>(new BinaryOperator(left.SQL, "%", right.SQL));
        }

        public static NullableExpression<DbDecimal> operator %(NullableExpression<DbDecimal> left, decimal right)
        {
            return left % SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator ==(NullableExpression<DbDecimal> left, decimal right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator !=(NullableExpression<DbDecimal> left, decimal right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbDecimal> left, NullableExpression<DbDecimal> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbDecimal> left, decimal right)
        {
            return left < SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbDecimal> left, NullableExpression<DbDecimal> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbDecimal> left, decimal right)
        {
            return left > SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbDecimal> left, NullableExpression<DbDecimal> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbDecimal> left, decimal right)
        {
            return left <= SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbDecimal> left, NullableExpression<DbDecimal> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbDecimal> left, decimal right)
        {
            return left >= SQL.Parameter(right);
        }

    }

}
