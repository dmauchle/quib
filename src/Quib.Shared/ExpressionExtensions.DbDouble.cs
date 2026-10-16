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

    extension(NotNullExpression<DbDouble> value)
    {

        public static NotNullExpression<DbDouble> operator +(NotNullExpression<DbDouble> left, NotNullExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbDouble>(new BinaryOperator(left.SQL, "+", right.SQL));
        }

        public static NotNullExpression<DbDouble> operator +(NotNullExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left + SQL.Parameter(right);
        }

        public static NotNullExpression<DbDouble> operator +(NotNullExpression<DbDouble> left, NotNullExpression<DbInt> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbDouble>(new BinaryOperator(left.SQL, "+", right.SQL));
        }

        public static NotNullExpression<DbDouble> operator -(NotNullExpression<DbDouble> left, NotNullExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbDouble>(new BinaryOperator(left.SQL, "-", right.SQL));
        }

        public static NotNullExpression<DbDouble> operator -(NotNullExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left - SQL.Parameter(right);
        }

        public static NotNullExpression<DbDouble> operator -(NotNullExpression<DbDouble> left, NotNullExpression<DbInt> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbDouble>(new BinaryOperator(left.SQL, "-", right.SQL));
        }

        public static NotNullExpression<DbDouble> operator *(NotNullExpression<DbDouble> left, NotNullExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbDouble>(new BinaryOperator(left.SQL, "*", right.SQL));
        }

        public static NotNullExpression<DbDouble> operator *(NotNullExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left * SQL.Parameter(right);
        }

        public static NotNullExpression<DbDouble> operator *(NotNullExpression<DbDouble> left, NotNullExpression<DbInt> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbDouble>(new BinaryOperator(left.SQL, "*", right.SQL));
        }

        public static NotNullExpression<DbDouble> operator /(NotNullExpression<DbDouble> left, NotNullExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbDouble>(new BinaryOperator(left.SQL, "/", right.SQL));
        }

        public static NotNullExpression<DbDouble> operator /(NotNullExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left / SQL.Parameter(right);
        }

        public static NotNullExpression<DbDouble> operator /(NotNullExpression<DbDouble> left, NotNullExpression<DbInt> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbDouble>(new BinaryOperator(left.SQL, "/", right.SQL));
        }

        public static NotNullExpression<DbBool> operator ==(NotNullExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator !=(NotNullExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbDouble> left, NotNullExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left < SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbDouble> left, NotNullExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left > SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbDouble> left, NotNullExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left <= SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbDouble> left, NotNullExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left >= SQL.Parameter(right);
        }

    }

    extension(NullableExpression<DbDouble> value)
    {

        public static NullableExpression<DbDouble> operator +(NullableExpression<DbDouble> left, NullableExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbDouble>(new BinaryOperator(left.SQL, "+", right.SQL));
        }

        public static NullableExpression<DbDouble> operator +(NullableExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left + SQL.Parameter(right);
        }

        public static NullableExpression<DbDouble> operator +(NullableExpression<DbDouble> left, NullableExpression<DbInt> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbDouble>(new BinaryOperator(left.SQL, "+", right.SQL));
        }

        public static NullableExpression<DbDouble> operator -(NullableExpression<DbDouble> left, NullableExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbDouble>(new BinaryOperator(left.SQL, "-", right.SQL));
        }

        public static NullableExpression<DbDouble> operator -(NullableExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left - SQL.Parameter(right);
        }

        public static NullableExpression<DbDouble> operator -(NullableExpression<DbDouble> left, NullableExpression<DbInt> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbDouble>(new BinaryOperator(left.SQL, "-", right.SQL));
        }

        public static NullableExpression<DbDouble> operator *(NullableExpression<DbDouble> left, NullableExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbDouble>(new BinaryOperator(left.SQL, "*", right.SQL));
        }

        public static NullableExpression<DbDouble> operator *(NullableExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left * SQL.Parameter(right);
        }

        public static NullableExpression<DbDouble> operator *(NullableExpression<DbDouble> left, NullableExpression<DbInt> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbDouble>(new BinaryOperator(left.SQL, "*", right.SQL));
        }

        public static NullableExpression<DbDouble> operator /(NullableExpression<DbDouble> left, NullableExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbDouble>(new BinaryOperator(left.SQL, "/", right.SQL));
        }

        public static NullableExpression<DbDouble> operator /(NullableExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left / SQL.Parameter(right);
        }

        public static NullableExpression<DbDouble> operator /(NullableExpression<DbDouble> left, NullableExpression<DbInt> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbDouble>(new BinaryOperator(left.SQL, "/", right.SQL));
        }

        public static NullableExpression<DbBool> operator ==(NullableExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator !=(NullableExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbDouble> left, NullableExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left < SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbDouble> left, NullableExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left > SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbDouble> left, NullableExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left <= SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbDouble> left, NullableExpression<DbDouble> right)
        {
            ArgumentNullException.ThrowIfNull(left);
            ArgumentNullException.ThrowIfNull(right);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbDouble> left, double right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return left >= SQL.Parameter(right);
        }

    }

}
