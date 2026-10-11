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

    public static NullableExpression<T> Null<T>()
        where T : struct, IExpressionType
    {
        return new NullableExpression<T>(DbNull.Value);
    }

    public static ISortExpression Sort<T>(NullableExpression<T> value, Sort direction)
        where T : struct, IExpressionType
    {
        return new SortClause(value.SQL, direction);
    }

    public static NotNullExpression<DbInt> Constant(int value)
    {
        return new NotNullExpression<DbInt>(new Constant(value.ToString()));
    }

    public static NotNullExpression<DbDecimal> Constant(decimal value)
    {
        return new NotNullExpression<DbDecimal>(new Constant(value.ToString(System.Globalization.CultureInfo.InvariantCulture)));
    }

    public static NotNullExpression<DbBool> Constant(bool value)
    {
        return new NotNullExpression<DbBool>(new Constant(value ? "true" : "false"));
    }

    public static NotNullExpression<DbText> Parameter(string value)
    {
        return new NotNullExpression<DbText>(new QueryParameter(value));
    }

    public static NullableExpression<DbText> NullableParameter(string? value)
    {
        return new NullableExpression<DbText>(new QueryParameter(value));
    }

    public static NotNullExpression<DbInt> Parameter(int value)
    {
        return new NotNullExpression<DbInt>(new QueryParameter(value));
    }

    public static NullableExpression<DbInt> NullableParameter(int? value)
    {
        return new NullableExpression<DbInt>(new QueryParameter(value));
    }

    public static NotNullExpression<DbDecimal> Parameter(decimal value)
    {
        return new NotNullExpression<DbDecimal>(new QueryParameter(value));
    }

    public static NullableExpression<DbDecimal> NullableParameter(decimal? value)
    {
        return new NullableExpression<DbDecimal>(new QueryParameter(value));
    }

    public static NotNullExpression<DbBool> Parameter(bool value)
    {
        return new NotNullExpression<DbBool>(new QueryParameter(value));
    }

    public static NullableExpression<DbBool> NullableParameter(bool? value)
    {
        return new NullableExpression<DbBool>(new QueryParameter(value));
    }


    public static NotNullExpression<DbUuid> Parameter(Guid value)
    {
#if MYSQL
        // MySQL stores UUIDs as CHAR(36): pass the lowercase "D" format to keep text comparison and V7 sort order consistent ("D" would be the default anyway)
        return new NotNullExpression<DbUuid>(new QueryParameter(value.ToString("D")));
#elif POSTGRES
        return new NotNullExpression<DbUuid>(new QueryParameter(value));
#endif
    }

    public static NullableExpression<DbUuid> NullableParameter(Guid? value)
    {
#if MYSQL
        // MySQL stores UUIDs as CHAR(36): pass the lowercase "D" format to keep text comparison and V7 sort order consistent ("D" would be the default anyway)
        return new NullableExpression<DbUuid>(new QueryParameter(value?.ToString("D")));
#elif POSTGRES
        return new NullableExpression<DbUuid>(new QueryParameter(value));
#endif
    }

    public static NotNullExpression<DbDateOnly> Parameter(DateOnly value)
    {
        return new NotNullExpression<DbDateOnly>(new QueryParameter(ToDbDateTime(value)));
    }

    public static NullableExpression<DbDateOnly> NullableParameter(DateOnly? value)
    {
        return new NullableExpression<DbDateOnly>(new QueryParameter(value.HasValue ? ToDbDateTime(value.Value) : null));
    }

    public static NotNullExpression<DbDateTime> Parameter(DateTime value)
    {
        return new NotNullExpression<DbDateTime>(new QueryParameter(ToDbDateTime(value)));
    }

    public static NullableExpression<DbDateTime> NullableParameter(DateTime? value)
    {
        return new NullableExpression<DbDateTime>(new QueryParameter(value.HasValue ? ToDbDateTime(value.Value) : null));
    }

    // Dates are passed as DateTime, as older drivers don't support DateOnly
    private static DateTime ToDbDateTime(DateOnly value)
    {
        return ToDbDateTime(value.ToDateTime(TimeOnly.MinValue));
    }

    // Values are stored as local time (read back as DateTimeKind.Local), so UTC values are converted.
    // Truncate to whole seconds (DBs would round instead) and drop the time zone (Npgsql rejects UTC values for timestamp columns)
    private static DateTime ToDbDateTime(DateTime value)
    {
        if (value.Kind == DateTimeKind.Utc)
            value = value.ToLocalTime();
        // MySQL officially supports years 1000 - 9999 only, but silently accepts smaller values.
        // Postgres is limited the same way for consistency (Npgsql would store DateTime.MinValue as -infinity).
        // In order to stay consistent and on the safe side, we always limit to year 1000. 
        if (value.Year < 1000)
            throw new ArgumentOutOfRangeException(nameof(value), value, "Dates are supported from year 1000 to 9999 only.");

        return new DateTime(value.Ticks - (value.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Unspecified);
    }

}