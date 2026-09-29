using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbIntNullableExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task CastFromNull()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Null.As<DbIntNullable>())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNull()
    {
        int? value = null;
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(value))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNullable()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(12))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Equal(12, rs.Result);
    }

    [Fact]
    public async Task PlusWithDbInt()
    {
        var left = SQL.NullableParameter(2);
        var right = SQL.Parameter(3);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusWithNullableValue()
    {
        var left = SQL.NullableParameter(2);
        var right = SQL.NullableParameter(3);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusWithNullableNull()
    {
        var left = SQL.NullableParameter(2);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task PlusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(2) + 3)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusOnNull()
    {
        var left = SQL.NullableParameter((int?)null);
        var right = 3;
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusWithDbInt()
    {
        var left = SQL.NullableParameter(5);
        var right = SQL.Parameter(3);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusWithNullableValue()
    {
        var left = SQL.NullableParameter(5);
        var right = SQL.NullableParameter(3);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusWithNullableNull()
    {
        var left = SQL.NullableParameter(5);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) - 3)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusOnNull()
    {
        var left = SQL.NullableParameter((int?)null);
        var right = 3;
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyWithDbInt()
    {
        var left = SQL.NullableParameter(2);
        var right = SQL.Parameter(3);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyWithNullableValue()
    {
        var left = SQL.NullableParameter(2);
        var right = SQL.NullableParameter(3);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyWithNullableNull()
    {
        var left = SQL.NullableParameter(2);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(2) * 3)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyOnNull()
    {
        var left = SQL.NullableParameter((int?)null);
        var right = 3;
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideWithDbInt()
    {
        var left = SQL.NullableParameter(6);
        var right = SQL.Parameter(3);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideWithNullableValue()
    {
        var left = SQL.NullableParameter(6);
        var right = SQL.NullableParameter(3);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideWithNullableNull()
    {
        var left = SQL.NullableParameter(6);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(6) / 3)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideOnNull()
    {
        var left = SQL.NullableParameter((int?)null);
        var right = 3;
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ModuloWithDbInt()
    {
        var left = SQL.NullableParameter(5);
        var right = SQL.Parameter(2);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloWithNullableValue()
    {
        var left = SQL.NullableParameter(5);
        var right = SQL.NullableParameter(2);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloWithNullableNull()
    {
        var left = SQL.NullableParameter(5);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ModuloWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) % 2)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloOnNull()
    {
        var left = SQL.NullableParameter((int?)null);
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
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) / 0)).FetchOne();
        Assert.Null(rs.Result);
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.NullableParameter(5) / 0)).FetchOne();
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
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) % 0)).FetchOne();
        Assert.Null(rs.Result);
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.NullableParameter(5) % 0)).FetchOne();
        });
#endif
    }

    [Fact]
    public async Task Equal_WithDbIntRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) == SQL.Parameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) == SQL.NullableParameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) == SQL.NullableParameter((int?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) == 5)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithDbIntRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) != SQL.Parameter(3))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) != SQL.NullableParameter(3))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) != SQL.NullableParameter((int?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) != 3)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithDbIntRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3) < SQL.Parameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3) < SQL.NullableParameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3) < SQL.NullableParameter((int?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3) < 5)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithDbIntRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) > SQL.Parameter(3))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) > SQL.NullableParameter(3))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) > SQL.NullableParameter((int?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) > 3)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithDbIntRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) <= SQL.Parameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) <= SQL.NullableParameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) <= SQL.NullableParameter((int?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) <= 5)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithDbIntRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) >= SQL.Parameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) >= SQL.NullableParameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) >= SQL.NullableParameter((int?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5) >= 5)).FetchOne();
        Assert.True(rs.Result);
    }

}
