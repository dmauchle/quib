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

    extension(NotNullExpression<DbDateTime> value)
    {

        public static NotNullExpression<DbBool> operator ==(NotNullExpression<DbDateTime> left, DateTime right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator !=(NotNullExpression<DbDateTime> left, DateTime right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbDateTime> left, NotNullExpression<DbDateTime> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbDateTime> left, DateTime right)
        {
            return left < SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbDateTime> left, NotNullExpression<DbDateTime> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbDateTime> left, DateTime right)
        {
            return left > SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbDateTime> left, NotNullExpression<DbDateTime> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbDateTime> left, DateTime right)
        {
            return left <= SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbDateTime> left, NotNullExpression<DbDateTime> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbDateTime> left, DateTime right)
        {
            return left >= SQL.Parameter(right);
        }

    }

    extension(NullableExpression<DbDateTime> value)
    {

        public static NullableExpression<DbBool> operator ==(NullableExpression<DbDateTime> left, DateTime right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator !=(NullableExpression<DbDateTime> left, DateTime right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbDateTime> left, NullableExpression<DbDateTime> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbDateTime> left, DateTime right)
        {
            return left < SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbDateTime> left, NullableExpression<DbDateTime> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbDateTime> left, DateTime right)
        {
            return left > SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbDateTime> left, NullableExpression<DbDateTime> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbDateTime> left, DateTime right)
        {
            return left <= SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbDateTime> left, NullableExpression<DbDateTime> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbDateTime> left, DateTime right)
        {
            return left >= SQL.Parameter(right);
        }

    }

}
