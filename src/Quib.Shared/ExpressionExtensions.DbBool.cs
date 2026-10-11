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

    extension(NotNullExpression<DbBool> value)
    {

        public static NotNullExpression<DbBool> operator ==(NotNullExpression<DbBool> left, bool right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "=", new QueryParameter(right)));
        }

        public static NotNullExpression<DbBool> operator !=(NotNullExpression<DbBool> left, bool right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "!=", new QueryParameter(right)));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbBool> left, NotNullExpression<DbBool> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <(NotNullExpression<DbBool> left, bool right)
        {
            return left < SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbBool> left, NotNullExpression<DbBool> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >(NotNullExpression<DbBool> left, bool right)
        {
            return left > SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbBool> left, NotNullExpression<DbBool> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator <=(NotNullExpression<DbBool> left, bool right)
        {
            return left <= SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbBool> left, NotNullExpression<DbBool> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NotNullExpression<DbBool> operator >=(NotNullExpression<DbBool> left, bool right)
        {
            return left >= SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator &(NotNullExpression<DbBool> left, NotNullExpression<DbBool> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "AND", right.SQL));
        }

        public static NotNullExpression<DbBool> operator &(NotNullExpression<DbBool> left, bool right)
        {
            return left & SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator |(NotNullExpression<DbBool> left, NotNullExpression<DbBool> right)
        {
            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "OR", right.SQL));
        }

        public static NotNullExpression<DbBool> operator |(NotNullExpression<DbBool> left, bool right)
        {
            return left | SQL.Parameter(right);
        }

        public static NotNullExpression<DbBool> operator !(NotNullExpression<DbBool> operand)
        {
            return new NotNullExpression<DbBool>(new UnaryOperator("NOT", operand.SQL));
        }

    }

    extension(NullableExpression<DbBool> value)
    {

        public static NullableExpression<DbBool> operator ==(NullableExpression<DbBool> left, bool right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator !=(NullableExpression<DbBool> left, bool right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbBool> left, NullableExpression<DbBool> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<", right.SQL));
        }

        public static NullableExpression<DbBool> operator <(NullableExpression<DbBool> left, bool right)
        {
            return left < SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbBool> left, NullableExpression<DbBool> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">", right.SQL));
        }

        public static NullableExpression<DbBool> operator >(NullableExpression<DbBool> left, bool right)
        {
            return left > SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbBool> left, NullableExpression<DbBool> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "<=", right.SQL));
        }

        public static NullableExpression<DbBool> operator <=(NullableExpression<DbBool> left, bool right)
        {
            return left <= SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbBool> left, NullableExpression<DbBool> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, ">=", right.SQL));
        }

        public static NullableExpression<DbBool> operator >=(NullableExpression<DbBool> left, bool right)
        {
            return left >= SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator &(NullableExpression<DbBool> left, NullableExpression<DbBool> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "AND", right.SQL));
        }

        public static NullableExpression<DbBool> operator &(NullableExpression<DbBool> left, bool right)
        {
            return left & SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator |(NullableExpression<DbBool> left, NullableExpression<DbBool> right)
        {
            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "OR", right.SQL));
        }

        public static NullableExpression<DbBool> operator |(NullableExpression<DbBool> left, bool right)
        {
            return left | SQL.Parameter(right);
        }

        public static NullableExpression<DbBool> operator !(NullableExpression<DbBool> operand)
        {
            return new NullableExpression<DbBool>(new UnaryOperator("NOT", operand.SQL));
        }

    }

}
