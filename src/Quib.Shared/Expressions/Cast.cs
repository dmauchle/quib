#if MYSQL56
namespace Quib.MySql56.Expressions;
#elif MYSQL57
namespace Quib.MySql57.Expressions;
#elif MYSQL80
namespace Quib.MySql80.Expressions;
#elif MYSQL84
namespace Quib.MySql84.Expressions;
#elif POSTGRES18
namespace Quib.Postgres18.Expressions;
#endif


internal sealed class Cast : IQueryExpression
{

    private readonly IQueryExpression _value;
    private readonly string _sqlType;

    public Cast(IQueryExpression value, string sqlType)
    {
        _value = value;
        _sqlType = sqlType;
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("CAST(");
        query.Write(_value);
        query.Write(" AS ");
        query.Write(_sqlType);
        query.Write(")");
    }

    // Widening conversions for the As...() methods of the numeric types (the value is kept)

    public static IQueryExpression ToLong(IQueryExpression value)
    {
#if MYSQL
        return new Cast(value, "SIGNED");
#elif POSTGRES
        return new Cast(value, "BIGINT");
#endif
    }

    public static IQueryExpression ToDouble(IQueryExpression value)
    {
#if MYSQL
        // CAST(... AS DOUBLE) is only supported from MySQL 8.0.17
        return new BinaryOperator(value, "*", new Constant("1E0"));
#elif POSTGRES
        return new Cast(value, "DOUBLE PRECISION");
#endif
    }

    /// <param name="digits">Max. number of digits of the value (only needed for MySQL, as DECIMAL defaults to DECIMAL(10,0)).</param>
    public static IQueryExpression ToDecimal(IQueryExpression value, int digits)
    {
#if MYSQL
        return new Cast(value, $"DECIMAL({digits},0)");
#elif POSTGRES
        return new Cast(value, "NUMERIC");
#endif
    }

}
