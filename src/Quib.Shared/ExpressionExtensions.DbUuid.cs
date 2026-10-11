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

    extension(NotNullExpression<DbUuid> value)
    {

        public static NotNullExpression<DbBool> operator ==(NotNullExpression<DbUuid> left, Guid right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator !=(NotNullExpression<DbUuid> left, Guid right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbUuid> left, NotNullExpression<DbUuid> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbUuid> left, Guid right)
        {
            return left < SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbUuid> left, NotNullExpression<DbUuid> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbUuid> left, Guid right)
        {
            return left > SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbUuid> left, NotNullExpression<DbUuid> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbUuid> left, Guid right)
        {
            return left <= SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbUuid> left, NotNullExpression<DbUuid> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbUuid> left, Guid right)
        {
            return left >= SQL.Parameter(right);
        }

    }

    extension(NullableExpression<DbUuid> value)
    {

        public static NullableExpression<DbBool> operator ==(NullableExpression<DbUuid> left, Guid right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator !=(NullableExpression<DbUuid> left, Guid right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbUuid> left, NullableExpression<DbUuid> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbUuid> left, Guid right)
        {
            return left < SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbUuid> left, NullableExpression<DbUuid> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbUuid> left, Guid right)
        {
            return left > SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbUuid> left, NullableExpression<DbUuid> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbUuid> left, Guid right)
        {
            return left <= SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbUuid> left, NullableExpression<DbUuid> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbUuid> left, Guid right)
        {
            return left >= SQL.Parameter(right);
        }

    }

}
