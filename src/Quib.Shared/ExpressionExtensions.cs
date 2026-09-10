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

    public static QueryExpression<DbBool> IsNull(this QueryExpression value)
    {
        return new IsNull(value);
    }

    public static QueryExpression<DbBool> IsNotNull(this QueryExpression value)
    {
        return new IsNotNull(value);
    }

}
