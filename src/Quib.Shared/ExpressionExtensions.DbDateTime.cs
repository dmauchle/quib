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

    extension<TTimeZone>(NotNullExpression<DbDateTime<TTimeZone>> value) where TTimeZone : IDateTimeStorage
    {

        public static NotNullExpression<DbBool> operator ==(NotNullExpression<DbDateTime<TTimeZone>> left, DateTime right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter<TTimeZone>(right).SQL));
        }

        public static NotNullExpression<DbBool> operator !=(NotNullExpression<DbDateTime<TTimeZone>> left, DateTime right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter<TTimeZone>(right).SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbDateTime<TTimeZone>> left, NotNullExpression<DbDateTime<TTimeZone>> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbDateTime<TTimeZone>> left, DateTime right)
        {
            return left < SQL.Parameter<TTimeZone>(right);
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbDateTime<TTimeZone>> left, NotNullExpression<DbDateTime<TTimeZone>> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbDateTime<TTimeZone>> left, DateTime right)
        {
            return left > SQL.Parameter<TTimeZone>(right);
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbDateTime<TTimeZone>> left, NotNullExpression<DbDateTime<TTimeZone>> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbDateTime<TTimeZone>> left, DateTime right)
        {
            return left <= SQL.Parameter<TTimeZone>(right);
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbDateTime<TTimeZone>> left, NotNullExpression<DbDateTime<TTimeZone>> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbDateTime<TTimeZone>> left, DateTime right)
        {
            return left >= SQL.Parameter<TTimeZone>(right);
        }

    }

    extension<TTimeZone>(NullableExpression<DbDateTime<TTimeZone>> value) where TTimeZone : IDateTimeStorage
    {

        public static NullableExpression<DbBool> operator ==(NullableExpression<DbDateTime<TTimeZone>> left, DateTime right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter<TTimeZone>(right).SQL));
        }

        public static NullableExpression<DbBool> operator !=(NullableExpression<DbDateTime<TTimeZone>> left, DateTime right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter<TTimeZone>(right).SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbDateTime<TTimeZone>> left, NullableExpression<DbDateTime<TTimeZone>> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbDateTime<TTimeZone>> left, DateTime right)
        {
            return left < SQL.Parameter<TTimeZone>(right);
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbDateTime<TTimeZone>> left, NullableExpression<DbDateTime<TTimeZone>> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbDateTime<TTimeZone>> left, DateTime right)
        {
            return left > SQL.Parameter<TTimeZone>(right);
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbDateTime<TTimeZone>> left, NullableExpression<DbDateTime<TTimeZone>> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbDateTime<TTimeZone>> left, DateTime right)
        {
            return left <= SQL.Parameter<TTimeZone>(right);
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbDateTime<TTimeZone>> left, NullableExpression<DbDateTime<TTimeZone>> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbDateTime<TTimeZone>> left, DateTime right)
        {
            return left >= SQL.Parameter<TTimeZone>(right);
        }

    }

}
