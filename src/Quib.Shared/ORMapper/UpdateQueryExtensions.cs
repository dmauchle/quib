#if MYSQL56
namespace Quib.MySql56.ORMapper;
#elif MYSQL57
namespace Quib.MySql57.ORMapper;
#elif MYSQL80
namespace Quib.MySql80.ORMapper;
#elif MYSQL84
namespace Quib.MySql84.ORMapper;
#elif POSTGRES18
namespace Quib.Postgres18.ORMapper;
#endif

public static class UpdateQueryExtensions
{

    public static void SetNull<TQuery, TDbType>(this TQuery query, NullableColumn<TDbType> column)
        where TQuery : IUpdateQueryBuilder
        where TDbType : struct, IExpressionType
    {
        query.Set(column, SQL.Null<TDbType>());
    }

    public static void Set<TQuery>(this TQuery query, NotNullColumn<DbText> column, string value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.Parameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NullableColumn<DbText> column, string? value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.NullableParameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NotNullColumn<DbInt> column, int value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.Parameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NullableColumn<DbInt> column, int? value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.NullableParameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NotNullColumn<DbLong> column, long value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.Parameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NullableColumn<DbLong> column, long? value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.NullableParameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NotNullColumn<DbBool> column, bool value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.Parameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NullableColumn<DbBool> column, bool? value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.NullableParameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NotNullColumn<DbUuid> column, Guid value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.Parameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NullableColumn<DbUuid> column, Guid? value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.NullableParameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NotNullColumn<DbDecimal> column, decimal value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.Parameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NullableColumn<DbDecimal> column, decimal? value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.NullableParameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NotNullColumn<DbDouble> column, double value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.Parameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NullableColumn<DbDouble> column, double? value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.NullableParameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NotNullColumn<DbDateOnly> column, DateOnly value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.Parameter(value));
    }

    public static void Set<TQuery>(this TQuery query, NullableColumn<DbDateOnly> column, DateOnly? value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, SQL.NullableParameter(value));
    }

    public static void Set<TQuery, TTimeZone>(this TQuery query, NotNullColumn<DbDateTime<TTimeZone>> column, DateTime value)
        where TQuery : IUpdateQueryBuilder
        where TTimeZone : IDateTimeStorage
    {
        query.Set(column, SQL.Parameter<TTimeZone>(value));
    }

    public static void Set<TQuery, TTimeZone>(this TQuery query, NullableColumn<DbDateTime<TTimeZone>> column, DateTime? value)
        where TQuery : IUpdateQueryBuilder
        where TTimeZone : IDateTimeStorage
    {
        query.Set(column, SQL.NullableParameter<TTimeZone>(value));
    }

}