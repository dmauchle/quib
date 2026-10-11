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

    public static NotNullExpression<DbBool> IsNull<T>(this NullableExpression<T> value)
        where T : struct, IExpressionType
    {
        return new NotNullExpression<DbBool>(new IsNull(value.SQL));
    }

    public static NotNullExpression<DbBool> IsNotNull<T>(this NullableExpression<T> value)
        where T : struct, IExpressionType
    {
        return new NotNullExpression<DbBool>(new IsNotNull(value.SQL));
    }

}
