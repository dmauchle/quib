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


    public static QueryExpression<DbText> Concat(QueryExpression<DbText> left, QueryExpression<DbText> right)
    {
        return new ConcatOperator<DbText>(left, right);
    }

    public static QueryExpression<DbTextNullable> Concat(QueryExpression<DbText> left, QueryExpression<DbTextNullable> right)
    {
        return new ConcatOperator<DbTextNullable>(left, right);
    }

    public static QueryExpression<DbText> Concat(QueryExpression<DbText> left, string right)
    {
        return Concat(left, SQL.Parameter(right));
    }

    public static QueryExpression<DbTextNullable> Concat(QueryExpression<DbTextNullable> left, QueryExpression<DbTextNullable> right)
    {
        return new ConcatOperator<DbTextNullable>(left, right);
    }

    public static QueryExpression<DbTextNullable> Concat(QueryExpression<DbTextNullable> left, QueryExpression<DbText> right)
    {
        return Concat(left, right.AsNullable());
    }

    public static QueryExpression<DbTextNullable> Concat(QueryExpression<DbTextNullable> left, string right)
    {
        return Concat(left, SQL.NullableParameter(right));
    }


}
