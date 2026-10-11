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

    extension(NotNullExpression<DbInt> value)
    {

        public static NotNullExpression<DbInt> operator +(NotNullExpression<DbInt> left, NotNullExpression<DbInt> right)
        {
            return new NotNullExpression<DbInt>(new BinaryOperator(left.SQL, "+", right.SQL));
        }

        public static NotNullExpression<DbInt> operator +(NotNullExpression<DbInt> left, int right)
        {
            return left + SQL.Parameter(right);
        }

        public static NotNullExpression<DbInt> operator -(NotNullExpression<DbInt> left, NotNullExpression<DbInt> right)
        {
            return new NotNullExpression<DbInt>(new BinaryOperator(left.SQL, "-", right.SQL));
        }

        public static NotNullExpression<DbInt> operator -(NotNullExpression<DbInt> left, int right)
        {
            return left - SQL.Parameter(right);
        }

        public static NotNullExpression<DbInt> operator *(NotNullExpression<DbInt> left, NotNullExpression<DbInt> right)
        {
            return new NotNullExpression<DbInt>(new BinaryOperator(left.SQL, "*", right.SQL));
        }

        public static NotNullExpression<DbInt> operator *(NotNullExpression<DbInt> left, int right)
        {
            return left * SQL.Parameter(right);
        }

        public static NotNullExpression<DbInt> operator /(NotNullExpression<DbInt> left, NotNullExpression<DbInt> right)
        {
            return new NotNullExpression<DbInt>(new BinaryOperator(left.SQL, "/", right.SQL));
        }

        public static NotNullExpression<DbInt> operator /(NotNullExpression<DbInt> left, int right)
        {
            return left / SQL.Parameter(right);
        }

        public static NotNullExpression<DbInt> operator %(NotNullExpression<DbInt> left, NotNullExpression<DbInt> right)
        {
            return new NotNullExpression<DbInt>(new BinaryOperator(left.SQL, "%", right.SQL));
        }

        public static NotNullExpression<DbInt> operator %(NotNullExpression<DbInt> left, int right)
        {
            return left % SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator ==(NotNullExpression<DbInt> left, int right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator !=(NotNullExpression<DbInt> left, int right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbInt> left, NotNullExpression<DbInt> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbInt> left, int right)
        {
            return left < SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbInt> left, NotNullExpression<DbInt> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbInt> left, int right)
        {
            return left > SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbInt> left, NotNullExpression<DbInt> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbInt> left, int right)
        {
            return left <= SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbInt> left, NotNullExpression<DbInt> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbInt> left, int right)
        {
            return left >= SQL.Parameter(right);
        }

    }

    extension(NullableExpression<DbInt> value)
    {

        public static NullableExpression<DbInt> operator +(NullableExpression<DbInt> left, NullableExpression<DbInt> right)
        {
            return new NullableExpression<DbInt>(new BinaryOperator(left.SQL, "+", right.SQL));
        }

        public static NullableExpression<DbInt> operator +(NullableExpression<DbInt> left, int right)
        {
            return left + SQL.Parameter(right);
        }

        public static NullableExpression<DbInt> operator -(NullableExpression<DbInt> left, NullableExpression<DbInt> right)
        {
            return new NullableExpression<DbInt>(new BinaryOperator(left.SQL, "-", right.SQL));
        }

        public static NullableExpression<DbInt> operator -(NullableExpression<DbInt> left, int right)
        {
            return left - SQL.Parameter(right);
        }

        public static NullableExpression<DbInt> operator *(NullableExpression<DbInt> left, NullableExpression<DbInt> right)
        {
            return new NullableExpression<DbInt>(new BinaryOperator(left.SQL, "*", right.SQL));
        }

        public static NullableExpression<DbInt> operator *(NullableExpression<DbInt> left, int right)
        {
            return left * SQL.Parameter(right);
        }

        public static NullableExpression<DbInt> operator /(NullableExpression<DbInt> left, NullableExpression<DbInt> right)
        {
            return new NullableExpression<DbInt>(new BinaryOperator(left.SQL, "/", right.SQL));
        }

        public static NullableExpression<DbInt> operator /(NullableExpression<DbInt> left, int right)
        {
            return left / SQL.Parameter(right);
        }

        public static NullableExpression<DbInt> operator %(NullableExpression<DbInt> left, NullableExpression<DbInt> right)
        {
            return new NullableExpression<DbInt>(new BinaryOperator(left.SQL, "%", right.SQL));
        }

        public static NullableExpression<DbInt> operator %(NullableExpression<DbInt> left, int right)
        {
            return left % SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator ==(NullableExpression<DbInt> left, int right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator !=(NullableExpression<DbInt> left, int right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbInt> left, NullableExpression<DbInt> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbInt> left, int right)
        {
            return left < SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbInt> left, NullableExpression<DbInt> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbInt> left, int right)
        {
            return left > SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbInt> left, NullableExpression<DbInt> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbInt> left, int right)
        {
            return left <= SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbInt> left, NullableExpression<DbInt> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbInt> left, int right)
        {
            return left >= SQL.Parameter(right);
        }

    }

}
