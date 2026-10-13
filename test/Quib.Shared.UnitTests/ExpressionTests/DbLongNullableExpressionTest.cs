using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbLongNullableExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task NullValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Null<DbLong>())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNull()
    {
        long? value = null;
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(value))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNullable()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(12L))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Equal(12, rs.Result);
    }

    [Fact]
    public async Task PlusWithDbLong()
    {
        var left = SQL.NullableParameter(2L);
        var right = SQL.Parameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusWithNullableValue()
    {
        var left = SQL.NullableParameter(2L);
        var right = SQL.NullableParameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusWithNullableNull()
    {
        var left = SQL.NullableParameter(2L);
        var right = SQL.NullableParameter((long?)null);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task PlusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(2L) + 3L)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusOnNull()
    {
        var left = SQL.NullableParameter((long?)null);
        var right = 3;
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusWithDbLong()
    {
        var left = SQL.NullableParameter(5L);
        var right = SQL.Parameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusWithNullableValue()
    {
        var left = SQL.NullableParameter(5L);
        var right = SQL.NullableParameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusWithNullableNull()
    {
        var left = SQL.NullableParameter(5L);
        var right = SQL.NullableParameter((long?)null);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) - 3L)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusOnNull()
    {
        var left = SQL.NullableParameter((long?)null);
        var right = 3;
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyWithDbLong()
    {
        var left = SQL.NullableParameter(2L);
        var right = SQL.Parameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyWithNullableValue()
    {
        var left = SQL.NullableParameter(2L);
        var right = SQL.NullableParameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyWithNullableNull()
    {
        var left = SQL.NullableParameter(2L);
        var right = SQL.NullableParameter((long?)null);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(2L) * 3L)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyOnNull()
    {
        var left = SQL.NullableParameter((long?)null);
        var right = 3;
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideWithDbLong()
    {
        var left = SQL.NullableParameter(6L);
        var right = SQL.Parameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideWithNullableValue()
    {
        var left = SQL.NullableParameter(6L);
        var right = SQL.NullableParameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideWithNullableNull()
    {
        var left = SQL.NullableParameter(6L);
        var right = SQL.NullableParameter((long?)null);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(6L) / 3L)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task Divide_WithRemainder_Truncates()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(7_000_000_001L) / 2L)).FetchOne();
        Assert.Equal(3_500_000_000L, rs.Result);
    }

    [Fact]
    public async Task Divide_Negative_TruncatesTowardZero()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(-7_000_000_001L) / 2L)).FetchOne();
        Assert.Equal(-3_500_000_000L, rs.Result);
    }

    [Fact]
    public async Task DivideOnNull()
    {
        var left = SQL.NullableParameter((long?)null);
        var right = 3;
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ModuloWithDbLong()
    {
        var left = SQL.NullableParameter(5L);
        var right = SQL.Parameter(2L);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloWithNullableValue()
    {
        var left = SQL.NullableParameter(5L);
        var right = SQL.NullableParameter(2L);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloWithNullableNull()
    {
        var left = SQL.NullableParameter(5L);
        var right = SQL.NullableParameter((long?)null);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ModuloWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) % 2L)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloOnNull()
    {
        var left = SQL.NullableParameter((long?)null);
        var right = 2;
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
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) / 0L)).FetchOne();
        Assert.Null(rs.Result);
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) / 0L)).FetchOne();
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
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) % 0L)).FetchOne();
        Assert.Null(rs.Result);
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) % 0L)).FetchOne();
        });
#endif
    }

    [Fact]
    public async Task Equal_WithDbLongRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) == SQL.Parameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) == SQL.NullableParameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) == SQL.NullableParameter((long?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) == 5L)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithDbLongRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) != SQL.Parameter(3L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) != SQL.NullableParameter(3L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) != SQL.NullableParameter((long?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) != 3L)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithDbLongRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3L) < SQL.Parameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3L) < SQL.NullableParameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3L) < SQL.NullableParameter((long?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3L) < 5L)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithDbLongRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) > SQL.Parameter(3L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) > SQL.NullableParameter(3L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) > SQL.NullableParameter((long?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) > 3L)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithDbLongRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) <= SQL.Parameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) <= SQL.NullableParameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) <= SQL.NullableParameter((long?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) <= 5L)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithDbLongRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) >= SQL.Parameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) >= SQL.NullableParameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) >= SQL.NullableParameter((long?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5L) >= 5L)).FetchOne();
        Assert.True(rs.Result);
    }

}
