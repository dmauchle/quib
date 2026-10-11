using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbDecimalNullableExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task NullValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Null<DbDecimal>())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNull()
    {
        decimal? value = null;
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(value))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNullable()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(12.75m))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Equal(12.75m, rs.Result);
    }

    [Fact]
    public async Task PlusWithDbDecimal()
    {
        var left = SQL.NullableParameter(2.5m);
        var right = SQL.Parameter(3.25m);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5.75m, rs.Result);
    }

    [Fact]
    public async Task PlusWithNullableValue()
    {
        var left = SQL.NullableParameter(2.5m);
        var right = SQL.NullableParameter(3.25m);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5.75m, rs.Result);
    }

    [Fact]
    public async Task PlusWithNullableNull()
    {
        var left = SQL.NullableParameter(2.5m);
        var right = SQL.NullableParameter((decimal?)null);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task PlusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(2.5m) + 3.25m)).FetchOne();
        Assert.Equal(5.75m, rs.Result);
    }

    [Fact]
    public async Task PlusOnNull()
    {
        var left = SQL.NullableParameter((decimal?)null);
        var right = 3.25m;
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusWithDbDecimal()
    {
        var left = SQL.NullableParameter(5.5m);
        var right = SQL.Parameter(3.25m);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2.25m, rs.Result);
    }

    [Fact]
    public async Task MinusWithNullableValue()
    {
        var left = SQL.NullableParameter(5.5m);
        var right = SQL.NullableParameter(3.25m);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2.25m, rs.Result);
    }

    [Fact]
    public async Task MinusWithNullableNull()
    {
        var left = SQL.NullableParameter(5.5m);
        var right = SQL.NullableParameter((decimal?)null);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) - 3.25m)).FetchOne();
        Assert.Equal(2.25m, rs.Result);
    }

    [Fact]
    public async Task MinusOnNull()
    {
        var left = SQL.NullableParameter((decimal?)null);
        var right = 3.25m;
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyWithDbDecimal()
    {
        var left = SQL.NullableParameter(1.5m);
        var right = SQL.Parameter(2.5m);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(3.75m, rs.Result);
    }

    [Fact]
    public async Task MultiplyWithNullableValue()
    {
        var left = SQL.NullableParameter(1.5m);
        var right = SQL.NullableParameter(2.5m);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(3.75m, rs.Result);
    }

    [Fact]
    public async Task MultiplyWithNullableNull()
    {
        var left = SQL.NullableParameter(1.5m);
        var right = SQL.NullableParameter((decimal?)null);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(1.5m) * 2.5m)).FetchOne();
        Assert.Equal(3.75m, rs.Result);
    }

    [Fact]
    public async Task MultiplyOnNull()
    {
        var left = SQL.NullableParameter((decimal?)null);
        var right = 2.5m;
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideWithDbDecimal()
    {
        var left = SQL.NullableParameter(7.5m);
        var right = SQL.Parameter(2.5m);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(3m, rs.Result);
    }

    [Fact]
    public async Task DivideWithNullableValue()
    {
        var left = SQL.NullableParameter(7.5m);
        var right = SQL.NullableParameter(2.5m);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(3m, rs.Result);
    }

    [Fact]
    public async Task DivideWithNullableNull()
    {
        var left = SQL.NullableParameter(7.5m);
        var right = SQL.NullableParameter((decimal?)null);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(7.5m) / 2.5m)).FetchOne();
        Assert.Equal(3m, rs.Result);
    }

    [Fact]
    public async Task DivideOnNull()
    {
        var left = SQL.NullableParameter((decimal?)null);
        var right = 2.5m;
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ModuloWithDbDecimal()
    {
        var left = SQL.NullableParameter(5.5m);
        var right = SQL.Parameter(2m);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1.5m, rs.Result);
    }

    [Fact]
    public async Task ModuloWithNullableValue()
    {
        var left = SQL.NullableParameter(5.5m);
        var right = SQL.NullableParameter(2m);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1.5m, rs.Result);
    }

    [Fact]
    public async Task ModuloWithNullableNull()
    {
        var left = SQL.NullableParameter(5.5m);
        var right = SQL.NullableParameter((decimal?)null);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ModuloWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) % 2m)).FetchOne();
        Assert.Equal(1.5m, rs.Result);
    }

    [Fact]
    public async Task ModuloOnNull()
    {
        var left = SQL.NullableParameter((decimal?)null);
        var right = 2m;
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideByZero()
    {
        // Database behavior on division by zero varies:
        // - PostgreSQL raises an error (division by zero)
        // - MySQL returns NULL

#if MYSQL
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) / 0m)).FetchOne();
        Assert.Null(rs.Result);
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) / 0m)).FetchOne();
        });
#endif
    }

    [Fact]
    public async Task ModuloByZero()
    {
        // Database behavior on modulo by zero varies:
        // - PostgreSQL raises an error (division by zero)
        // - MySQL returns NULL

#if MYSQL
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) % 0m)).FetchOne();
        Assert.Null(rs.Result);
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) % 0m)).FetchOne();
        });
#endif
    }

    [Fact]
    public async Task Equal_WithDbDecimalRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) == SQL.Parameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) == SQL.NullableParameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) == SQL.NullableParameter((decimal?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) == 5.5m)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithDbDecimalRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) != SQL.Parameter(3.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) != SQL.NullableParameter(3.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) != SQL.NullableParameter((decimal?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) != 3.5m)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithDbDecimalRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3.5m) < SQL.Parameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3.5m) < SQL.NullableParameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3.5m) < SQL.NullableParameter((decimal?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3.5m) < 5.5m)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithDbDecimalRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) > SQL.Parameter(3.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) > SQL.NullableParameter(3.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) > SQL.NullableParameter((decimal?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) > 3.5m)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithDbDecimalRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) <= SQL.Parameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) <= SQL.NullableParameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) <= SQL.NullableParameter((decimal?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) <= 5.5m)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithDbDecimalRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) >= SQL.Parameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) >= SQL.NullableParameter(5.5m))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) >= SQL.NullableParameter((decimal?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5.5m) >= 5.5m)).FetchOne();
        Assert.True(rs.Result);
    }

}
