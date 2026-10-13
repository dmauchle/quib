using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbLongExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task Constant()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(3L))).FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task Parameter()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3L))).FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task PlusParameterRight()
    {
        var left = SQL.Parameter(2L);
        var right = SQL.Parameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusNullableValue()
    {
        var left = SQL.Parameter(2L);
        var right = SQL.NullableParameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusNullableNull()
    {
        var left = SQL.Parameter(2L);
        var right = SQL.NullableParameter((long?)null);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task PlusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(2L) + 3L)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task MinusParameterRight()
    {
        var left = SQL.Parameter(5L);
        var right = SQL.Parameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusNullableValue()
    {
        var left = SQL.Parameter(5L);
        var right = SQL.NullableParameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusNullableNull()
    {
        var left = SQL.Parameter(5L);
        var right = SQL.NullableParameter((long?)null);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) - 3L)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MultiplyParameterRight()
    {
        var left = SQL.Parameter(2L);
        var right = SQL.Parameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyNullableValue()
    {
        var left = SQL.Parameter(2L);
        var right = SQL.NullableParameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyNullableNull()
    {
        var left = SQL.Parameter(2L);
        var right = SQL.NullableParameter((long?)null);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(2L) * 3L)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task DivideParameterRight()
    {
        var left = SQL.Parameter(6L);
        var right = SQL.Parameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideNullableValue()
    {
        var left = SQL.Parameter(6L);
        var right = SQL.NullableParameter(3L);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideNullableNull()
    {
        var left = SQL.Parameter(6L);
        var right = SQL.NullableParameter((long?)null);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(6L) / 3L)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task ModuloParameterRight()
    {
        var left = SQL.Parameter(5L);
        var right = SQL.Parameter(2L);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloNullableValue()
    {
        var left = SQL.Parameter(5L);
        var right = SQL.NullableParameter(2L);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloNullableNull()
    {
        var left = SQL.Parameter(5L);
        var right = SQL.NullableParameter((long?)null);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ModuloWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) % 2L)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task Equal_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) == SQL.Parameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) == SQL.NullableParameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) == SQL.NullableParameter((long?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) == 5L)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) != SQL.Parameter(3L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) != SQL.NullableParameter(3L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) != SQL.NullableParameter((long?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) != 3L)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3L) < SQL.Parameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3L) < SQL.NullableParameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3L) < SQL.NullableParameter((long?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3L) < 5L)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) > SQL.Parameter(3L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) > SQL.NullableParameter(3L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) > SQL.NullableParameter((long?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) > 3L)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) <= SQL.Parameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) <= SQL.NullableParameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) <= SQL.NullableParameter((long?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) <= 5L)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) >= SQL.Parameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) >= SQL.NullableParameter(5L))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) >= SQL.NullableParameter((long?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5L) >= 5L)).FetchOne();
        Assert.True(rs.Result);
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
            return DB.SelectValues(x => x.Get(SQL.Parameter(5L) / 0L)).FetchOne();
        });
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(5L) / 0L)).FetchOne();
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
            return DB.SelectValues(x => x.Get(SQL.Parameter(5L) % 0L)).FetchOne();
        });
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(5L) % 0L)).FetchOne();
        });
#endif
    }

    // 64-bit range

    [Fact]
    public async Task Constant_MaxValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(long.MaxValue))).FetchOne();
        Assert.Equal(long.MaxValue, rs.Result);
    }

    [Fact]
    public async Task Constant_MinValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(long.MinValue))).FetchOne();
        Assert.Equal(long.MinValue, rs.Result);
    }

    [Fact]
    public async Task Parameter_MaxValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(long.MaxValue))).FetchOne();
        Assert.Equal(long.MaxValue, rs.Result);
    }

    [Fact]
    public async Task Parameter_MinValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(long.MinValue))).FetchOne();
        Assert.Equal(long.MinValue, rs.Result);
    }

    [Fact]
    public async Task Plus_BeyondIntRange()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3_000_000_000L) + 3_000_000_000L)).FetchOne();
        Assert.Equal(6_000_000_000L, rs.Result);
    }

    [Fact]
    public async Task Multiply_BeyondIntRange()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(100_000L) * 100_000L)).FetchOne();
        Assert.Equal(10_000_000_000L, rs.Result);
    }

    [Fact]
    public async Task Plus_Overflow_Throws()
    {
        // Both databases raise an error (out of range) instead of wrapping around
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(long.MaxValue) + 1L)).FetchOne();
        });
    }

}