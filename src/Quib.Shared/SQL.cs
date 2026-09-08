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

public static class SQL
{

    public static IQueryExpression<T> Null<T>()
        where T : struct, INullableExpressionType
    {
        return DbNull<T>.Value;
    }

}