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

public static partial class SQL
{

    public static readonly DbNull NULL = DbNull.Value;

    public static IQueryExpression<DbInt> Constant(int value)
    {
        return new Constant<DbInt>(value.ToString());
    }

    public static IQueryExpression<DbText> Parameter(string value)
    {
        return new QueryParameter<DbText>(value);
    }

    public static IQueryExpression<DbTextNullable> NullableParameter(string? value)
    {
        return new QueryParameter<DbTextNullable>(value);
    }

    public static IQueryExpression<DbInt> Parameter(int value)
    {
        return new QueryParameter<DbInt>(value);
    }

    public static IQueryExpression<DbIntNullable> NullableParameter(int? value)
    {
        return new QueryParameter<DbIntNullable>(value);
    }

}