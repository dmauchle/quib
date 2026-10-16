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

    // Only equality operators, as ordering binary data (<, >, ...) makes no sense

    extension(NotNullExpression<DbBinary> value)
    {

        public static NotNullExpression<DbBool> operator ==(NotNullExpression<DbBinary> left, byte[] right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NotNullExpression<DbBool> operator !=(NotNullExpression<DbBinary> left, byte[] right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

    }

    extension(NullableExpression<DbBinary> value)
    {

        public static NullableExpression<DbBool> operator ==(NullableExpression<DbBinary> left, byte[] right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "=", SQL.Parameter(right).SQL));
        }

        public static NullableExpression<DbBool> operator !=(NullableExpression<DbBinary> left, byte[] right)
        {
            ArgumentNullException.ThrowIfNull(left);

            return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "!=", SQL.Parameter(right).SQL));
        }

    }

}
