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


    public static IQueryExpression<DbText> Concat(IQueryExpression<DbText> left, IQueryExpression<DbText> right)
    {
        return new ConcatOperator(left, right.AsNullable());
    }

    public static IQueryExpression<DbTextNullable> Concat(IQueryExpression<DbText> left, IQueryExpression<DbTextNullable> right)
    {
        return new ConcatOperator(left, right);
    }

    public static IQueryExpression<DbTextNullable> Concat(IQueryExpression<DbText> left, string right)
    {
        return Concat(left, SQL.NullableParameter(right));
    }

    public static IQueryExpression<DbTextNullable> Concat(IQueryExpression<DbTextNullable> left, IQueryExpression<DbTextNullable> right)
    {
        return new ConcatOperator(left, right);
    }

    public static IQueryExpression<DbTextNullable> Concat(IQueryExpression<DbTextNullable> left, IQueryExpression<DbText> right)
    {
        return Concat(left, right.AsNullable());
    }

    public static IQueryExpression<DbTextNullable> Concat(IQueryExpression<DbTextNullable> left, string right)
    {
        return Concat(left, SQL.NullableParameter(right));
    }


}
