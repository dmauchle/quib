using Quib.UnitTests.Infrastructure;
using System.Globalization;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbDecimalExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task Constant()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(3.25m))).FetchOne();
        Assert.Equal(3.25m, rs.Result);
    }

    [Fact]
    public async Task Constant_WithNegativeValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(-3.25m))).FetchOne();
        Assert.Equal(-3.25m, rs.Result);
    }

    [Fact]
    public async Task Constant_WithTrailingZeros_KeepsScale()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(1.50m))).FetchOne();
        Assert.Equal("1.50", rs.Result.ToString(CultureInfo.InvariantCulture));
    }

    [Fact]
    public async Task Parameter()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3.25m))).FetchOne();
        Assert.Equal(3.25m, rs.Result);
    }

    [Fact]
    public async Task Parameter_WithMaxScale()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(0.1234567890123456789012345678m))).FetchOne();
        Assert.Equal(0.1234567890123456789012345678m, rs.Result);
    }

    [Fact]
    public async Task Parameter_WithMaxValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(decimal.MaxValue))).FetchOne();
        Assert.Equal(decimal.MaxValue, rs.Result);
    }

    [Fact]
    public async Task AsNullable()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3.25m).AsNullable())).FetchOne();
        Assert.Equal(3.25m, rs.Result);
    }

    [Fact]
    public async Task PlusParameterRight()
    {
        var left = SQL.Parameter(2.5m);
        var right = SQL.Parameter(3.25m);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5.75m, rs.Result);
    }

    [Fact]
    public async Task PlusNullableValue()
    {
        var left = SQL.Parameter(2.5m);
        var right = SQL.NullableParameter(3.25m);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5.75m, rs.Result);
    }

    [Fact]
    public async Task PlusNullableNull()
    {
        var left = SQL.Parameter(2.5m);
        var right = SQL.NullableParameter((decimal?)null);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task PlusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(2.5m) + 3.25m)).FetchOne();
        Assert.Equal(5.75m, rs.Result);
    }

    [Fact]
    public async Task MinusParameterRight()
    {
        var left = SQL.Parameter(5.5m);
        var right = SQL.Parameter(3.25m);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2.25m, rs.Result);
    }

    [Fact]
    public async Task MinusNullableValue()
    {
        var left = SQL.Parameter(5.5m);
        var right = SQL.NullableParameter(3.25m);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2.25m, rs.Result);
    }

    [Fact]
    public async Task MinusNullableNull()
    {
        var left = SQL.Parameter(5.5m);
        var right = SQL.NullableParameter((decimal?)null);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) - 3.25m)).FetchOne();
        Assert.Equal(2.25m, rs.Result);
    }

    [Fact]
    public async Task MultiplyParameterRight()
    {
        var left = SQL.Parameter(1.5m);
        var right = SQL.Parameter(2.5m);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(3.75m, rs.Result);
    }

    [Fact]
    public async Task MultiplyNullableValue()
    {
        var left = SQL.Parameter(1.5m);
        var right = SQL.NullableParameter(2.5m);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(3.75m, rs.Result);
    }

    [Fact]
    public async Task MultiplyNullableNull()
    {
        var left = SQL.Parameter(1.5m);
        var right = SQL.NullableParameter((decimal?)null);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(1.5m) * 2.5m)).FetchOne();
        Assert.Equal(3.75m, rs.Result);
    }

    [Fact]
    public async Task DivideParameterRight()
    {
        var left = SQL.Parameter(7.5m);
        var right = SQL.Parameter(2.5m);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(3m, rs.Result);
    }

    [Fact]
    public async Task DivideNullableValue()
    {
        var left = SQL.Parameter(7.5m);
        var right = SQL.NullableParameter(2.5m);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(3m, rs.Result);
    }

    [Fact]
    public async Task DivideNullableNull()
    {
        var left = SQL.Parameter(7.5m);
        var right = SQL.NullableParameter((decimal?)null);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(7.5m) / 2.5m)).FetchOne();
        Assert.Equal(3m, rs.Result);
    }

    [Fact]
    public async Task ModuloParameterRight()
    {
        var left = SQL.Parameter(5.5m);
        var right = SQL.Parameter(2m);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1.5m, rs.Result);
    }

    [Fact]
    public async Task ModuloNullableValue()
    {
        var left = SQL.Parameter(5.5m);
        var right = SQL.NullableParameter(2m);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1.5m, rs.Result);
    }

    [Fact]
    public async Task ModuloNullableNull()
    {
        var left = SQL.Parameter(5.5m);
        var right = SQL.NullableParameter((decimal?)null);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ModuloWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) % 2m)).FetchOne();
        Assert.Equal(1.5m, rs.Result);
    }

    [Fact]
    public async Task Equal_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) == SQL.Parameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) == SQL.NullableParameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) == SQL.NullableParameter((decimal?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) == 5.5m)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithDifferentScale_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) == 5.500m)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) != SQL.Parameter(3.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) != SQL.NullableParameter(3.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) != SQL.NullableParameter((decimal?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) != 3.5m)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3.5m) < SQL.Parameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3.5m) < SQL.NullableParameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3.5m) < SQL.NullableParameter((decimal?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3.5m) < 5.5m)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) > SQL.Parameter(3.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) > SQL.NullableParameter(3.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) > SQL.NullableParameter((decimal?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) > 3.5m)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) <= SQL.Parameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) <= SQL.NullableParameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) <= SQL.NullableParameter((decimal?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) <= 5.5m)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) >= SQL.Parameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) >= SQL.NullableParameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) >= SQL.NullableParameter((decimal?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) >= 5.5m)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Divide_WithFraction_Works()
    {
        // The scale of the result differs (MySQL: 4 more decimals than the dividend, Postgres: at least 16 significant digits)
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(1m) / 3m)).FetchOne();
        Assert.Equal(0.3333m, Math.Round(rs.Result, 4));
    }

    [Fact]
    public async Task Divide_WithScaleExceedingDecimal_Rounds()
    {
        // The result has more than 28 decimals in the database. It's rounded when read into a .NET decimal.
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(0.1234567890123456789012345679m) / 3m)).FetchOne();
        Assert.Equal(0.0411522630041152263004115226m, rs.Result);
    }

    [Fact]
    public async Task Multiply_ExceedingDecimalRange_Throws()
    {
        // The database can hold the result, but it doesn't fit into a .NET decimal
        await Assert.ThrowsAnyAsync<OverflowException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(decimal.MaxValue) * 10m)).FetchOne();
        });
    }

    [Fact]
    public async Task DivideByZero()
    {
        // Database behavior on division by zero varies:
        // - PostgreSQL raises an error (division by zero)
        // - MySQL returns NULL, but materialization will fail due to datatype.

#if MYSQL
        await Assert.ThrowsAnyAsync<System.Data.SqlTypes.SqlNullValueException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) / 0m)).FetchOne();
        });
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) / 0m)).FetchOne();
        });
#endif
    }

    [Fact]
    public async Task ModuloByZero()
    {
        // Database behavior on modulo by zero varies:
        // - PostgreSQL raises an error (division by zero)
        // - MySQL returns NULL, but materialization will fail due to datatype.

#if MYSQL
        await Assert.ThrowsAnyAsync<System.Data.SqlTypes.SqlNullValueException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) % 0m)).FetchOne();
        });
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(5.5m) % 0m)).FetchOne();
        });
#endif
    }

}
