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

public static class UpdateQueryExtensions
{

    public static void SetNull<TQuery, TDbType>(this TQuery query, DbColumn<TDbType> column)
        where TQuery : IUpdateQueryBuilder
        where TDbType : struct, INullableExpressionType
    {
        query.Set(column, SQL.Null.As<TDbType>());
    }

    public static void Set<TQuery>(this TQuery query, DbColumn<DbText> column, string value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.Parameter(value));
    }

    public static void Set<TQuery>(this TQuery query, DbColumn<DbTextNullable> column, string? value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.NullableParameter(value));
    }

}