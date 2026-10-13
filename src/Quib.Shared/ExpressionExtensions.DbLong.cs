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

    extension(NotNullExpression<DbLong> value)
    {

        public static NotNullExpression<DbLong> operator +(NotNullExpression<DbLong> left, NotNullExpression<DbLong> right)
        {
            return new NotNullExpression<DbLong>(new BinaryOperator(left.SQL, "+", right.SQL));
        }

        public static NotNullExpression<DbLong> operator +(NotNullExpression<DbLong> left, long right)
        {
            return left + SQL.Parameter(right);
        }

        public static NotNullExpression<DbLong> operator -(NotNullExpression<DbLong> left, NotNullExpression<DbLong> right)
        {
            return new NotNullExpression<DbLong>(new BinaryOperator(left.SQL, "-", right.SQL));
        }

        public static NotNullExpression<DbLong> operator -(NotNullExpression<DbLong> left, long right)
        {
            return left - SQL.Parameter(right);
        }

        public static NotNullExpression<DbLong> operator *(NotNullExpression<DbLong> left, NotNullExpression<DbLong> right)
        {
            return new NotNullExpression<DbLong>(new BinaryOperator(left.SQL, "*", right.SQL));
        }

        public static NotNullExpression<DbLong> operator *(NotNullExpression<DbLong> left, long right)
        {
            return left * SQL.Parameter(right);
        }

        public static NotNullExpression<DbLong> operator /(NotNullExpression<DbLong> left, NotNullExpression<DbLong> right)
        {
            return new NotNullExpression<DbLong>(new BinaryOperator(left.SQL, "/", right.SQL));
        }

        public static NotNullExpression<DbLong> operator /(NotNullExpression<DbLong> left, long right)
        {
            return left / SQL.Parameter(right);
        }

        public static NotNullExpression<DbLong> operator %(NotNullExpression<DbLong> left, NotNullExpression<DbLong> right)
        {
            return new NotNullExpression<DbLong>(new BinaryOperator(left.SQL, "%", right.SQL));
        }

        public static NotNullExpression<DbLong> operator %(NotNullExpression<DbLong> left, long right)
        {
            return left % SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator ==(NotNullExpression<DbLong> left, long right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator !=(NotNullExpression<DbLong> left, long right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbLong> left, NotNullExpression<DbLong> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbLong> left, long right)
        {
            return left < SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbLong> left, NotNullExpression<DbLong> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbLong> left, long right)
        {
            return left > SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbLong> left, NotNullExpression<DbLong> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbLong> left, long right)
        {
            return left <= SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbLong> left, NotNullExpression<DbLong> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbLong> left, long right)
        {
            return left >= SQL.Parameter(right);
        }

    }

    extension(NullableExpression<DbLong> value)
    {

        public static NullableExpression<DbLong> operator +(NullableExpression<DbLong> left, NullableExpression<DbLong> right)
        {
            return new NullableExpression<DbLong>(new BinaryOperator(left.SQL, "+", right.SQL));
        }

        public static NullableExpression<DbLong> operator +(NullableExpression<DbLong> left, long right)
        {
            return left + SQL.Parameter(right);
        }

        public static NullableExpression<DbLong> operator -(NullableExpression<DbLong> left, NullableExpression<DbLong> right)
        {
            return new NullableExpression<DbLong>(new BinaryOperator(left.SQL, "-", right.SQL));
        }

        public static NullableExpression<DbLong> operator -(NullableExpression<DbLong> left, long right)
        {
            return left - SQL.Parameter(right);
        }

        public static NullableExpression<DbLong> operator *(NullableExpression<DbLong> left, NullableExpression<DbLong> right)
        {
            return new NullableExpression<DbLong>(new BinaryOperator(left.SQL, "*", right.SQL));
        }

        public static NullableExpression<DbLong> operator *(NullableExpression<DbLong> left, long right)
        {
            return left * SQL.Parameter(right);
        }

        public static NullableExpression<DbLong> operator /(NullableExpression<DbLong> left, NullableExpression<DbLong> right)
        {
            return new NullableExpression<DbLong>(new BinaryOperator(left.SQL, "/", right.SQL));
        }

        public static NullableExpression<DbLong> operator /(NullableExpression<DbLong> left, long right)
        {
            return left / SQL.Parameter(right);
        }

        public static NullableExpression<DbLong> operator %(NullableExpression<DbLong> left, NullableExpression<DbLong> right)
        {
            return new NullableExpression<DbLong>(new BinaryOperator(left.SQL, "%", right.SQL));
        }

        public static NullableExpression<DbLong> operator %(NullableExpression<DbLong> left, long right)
        {
            return left % SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator ==(NullableExpression<DbLong> left, long right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator !=(NullableExpression<DbLong> left, long right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbLong> left, NullableExpression<DbLong> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbLong> left, long right)
        {
            return left < SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbLong> left, NullableExpression<DbLong> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbLong> left, long right)
        {
            return left > SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbLong> left, NullableExpression<DbLong> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbLong> left, long right)
        {
            return left <= SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbLong> left, NullableExpression<DbLong> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbLong> left, long right)
        {
            return left >= SQL.Parameter(right);
        }

    }

}
