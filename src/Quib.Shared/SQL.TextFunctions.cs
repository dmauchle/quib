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


    public static NotNullExpression<DbText> Concat(NotNullExpression<DbText> left, NotNullExpression<DbText> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new NotNullExpression<DbText>(new ConcatOperator(left.SQL, right.SQL));
    }

    public static NotNullExpression<DbText> Concat(NotNullExpression<DbText> left, string right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return Concat(left, SQL.Parameter(right));
    }

    public static NullableExpression<DbText> Concat(NullableExpression<DbText> left, NullableExpression<DbText> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new NullableExpression<DbText>(new ConcatOperator(left.SQL, right.SQL));
    }

    public static NullableExpression<DbText> Concat(NullableExpression<DbText> left, string right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return Concat(left, SQL.Parameter(right));
    }


}
