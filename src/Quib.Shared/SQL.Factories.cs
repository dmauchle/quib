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

    public static NotNullExpression<DbLong> Constant(long value)
    {
        return new NotNullExpression<DbLong>(new Constant(value.ToString()));
    }

    public static NotNullExpression<DbDecimal> Constant(decimal value)
    {
        return new NotNullExpression<DbDecimal>(new Constant(value.ToString(System.Globalization.CultureInfo.InvariantCulture)));
    }

    public static NotNullExpression<DbDouble> Constant(double value)
    {
        var literal = ToDbDouble(value).ToString("R", System.Globalization.CultureInfo.InvariantCulture);
#if MYSQL
        // A literal without exponent would be DECIMAL, so the exponent is enforced
        return new NotNullExpression<DbDouble>(new Constant(literal.Contains('E') ? literal : $"{literal}E0"));
#elif POSTGRES
        // A literal with fraction or exponent would be NUMERIC
        return new NotNullExpression<DbDouble>(new Constant($"CAST({literal} AS DOUBLE PRECISION)"));
#endif
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

    public static NotNullExpression<DbBinary> Parameter(byte[] value)
    {
        // In case nullable compiler errors are turned off in the consuming project.
        ArgumentNullException.ThrowIfNull(value);

        return new NotNullExpression<DbBinary>(new QueryParameter(value));
    }

    public static NullableExpression<DbBinary> NullableParameter(byte[]? value)
    {
#if MYSQL
        // Depending on the driver state, a NULL parameter may be sent as text (utf8mb4). Comparing it with binary data
        // fails then ("Cannot convert string ... from binary to utf8mb4"), so NULL is explicitly typed as binary.
        // Non-null values are always sent as binary. Failure could have happened when running the full test suite.
        if (value == null)
            return new NullableExpression<DbBinary>(new Constant("CAST(NULL AS BINARY)"));
#endif
        return new NullableExpression<DbBinary>(new QueryParameter(value));
    }

    public static NotNullExpression<DbInt> Parameter(int value)
    {
        return new NotNullExpression<DbInt>(new QueryParameter(value));
    }

    public static NullableExpression<DbInt> NullableParameter(int? value)
    {
        return new NullableExpression<DbInt>(new QueryParameter(value));
    }

    public static NotNullExpression<DbLong> Parameter(long value)
    {
        return new NotNullExpression<DbLong>(new QueryParameter(value));
    }

    public static NullableExpression<DbLong> NullableParameter(long? value)
    {
        return new NullableExpression<DbLong>(new QueryParameter(value));
    }

    public static NotNullExpression<DbDecimal> Parameter(decimal value)
    {
        return new NotNullExpression<DbDecimal>(new QueryParameter(value));
    }

    public static NullableExpression<DbDecimal> NullableParameter(decimal? value)
    {
        return new NullableExpression<DbDecimal>(new QueryParameter(value));
    }

    public static NotNullExpression<DbDouble> Parameter(double value)
    {
        return new NotNullExpression<DbDouble>(DoubleParameter(ToDbDouble(value)));
    }

    public static NullableExpression<DbDouble> NullableParameter(double? value)
    {
        return new NullableExpression<DbDouble>(DoubleParameter(value.HasValue ? ToDbDouble(value.Value) : null));
    }

    private static IQueryExpression DoubleParameter(double? value)
    {
#if MYSQL
        // The driver inlines the value as literal without exponent (e.g. 0.1), which MySQL treats as DECIMAL.
        // Multiplying by a double constant enforces double arithmetic (e.g. for 0.1 + 0.2). NULL stays NULL.
        return new BinaryOperator(new QueryParameter(value), "*", new Constant("1E0"));
#elif POSTGRES
        return new QueryParameter(value);
#endif
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

    /// <summary>
    /// Date-time parameter. <typeparamref name="TTimeZone"/> defines the time zone the value is stored in, e.g.
    /// <see cref="Utc"/> or <see cref="LocalTime"/>.
    /// </summary>
    public static NotNullExpression<DbDateTime<TTimeZone>> Parameter<TTimeZone>(DateTime value)
        where TTimeZone : IDateTimeStorage
    {
        return new NotNullExpression<DbDateTime<TTimeZone>>(new QueryParameter(ToDbDateTime(TTimeZone.ToDatabase(value))));
    }

    /// <inheritdoc cref="Parameter{TTimeZone}(DateTime)"/>
    public static NullableExpression<DbDateTime<TTimeZone>> NullableParameter<TTimeZone>(DateTime? value)
        where TTimeZone : IDateTimeStorage
    {
        return new NullableExpression<DbDateTime<TTimeZone>>(new QueryParameter(value.HasValue ? ToDbDateTime(TTimeZone.ToDatabase(value.Value)) : null));
    }

    /// <summary>
    /// MySQL doesn't support NaN and infinity (Postgres does). Rejected on all databases for consistency.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static double ToDbDouble(double value)
    {
        if (!double.IsFinite(value))
            throw new ArgumentOutOfRangeException(nameof(value), value, "NaN and infinity are not supported.");

        return value;
    }

    /// <summary>
    /// Dates are passed as DateTime, as older drivers don't support DateOnly
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static DateTime ToDbDateTime(DateOnly value)
    {
        return ToDbDateTime(value.ToDateTime(TimeOnly.MinValue));
    }

    /// <summary>
    /// Expects the value in the time zone it's stored in (see <see cref="IDateTimeStorage">).
    /// Truncate to whole seconds (DBs would round instead) and drop the time zone (Npgsql rejects UTC values for timestamp columns)
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static DateTime ToDbDateTime(DateTime value)
    {
        // MySQL officially supports years 1000 - 9999 only, but silently accepts smaller values.
        // Postgres is limited the same way for consistency (Npgsql would store DateTime.MinValue as -infinity).
        // In order to stay consistent and on the safe side, we always limit to year 1000. 
        if (value.Year < 1000)
            throw new ArgumentOutOfRangeException(nameof(value), value, "Dates are supported from year 1000 to 9999 only.");

        return new DateTime(value.Ticks - (value.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Unspecified);
    }

}