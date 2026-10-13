using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbDoubleNullableExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task NullValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Null<DbDouble>())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNull()
    {
        double? value = null;
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(value))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNullable()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(12d))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Equal(12, rs.Result);
    }

    [Fact]
    public async Task PlusWithDbDouble()
    {
        var left = SQL.NullableParameter(2d);
        var right = SQL.Parameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusWithNullableValue()
    {
        var left = SQL.NullableParameter(2d);
        var right = SQL.NullableParameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusWithNullableNull()
    {
        var left = SQL.NullableParameter(2d);
        var right = SQL.NullableParameter((double?)null);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task PlusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(2d) + 3d)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusOnNull()
    {
        var left = SQL.NullableParameter((double?)null);
        var right = 3;
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusWithDbDouble()
    {
        var left = SQL.NullableParameter(5d);
        var right = SQL.Parameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusWithNullableValue()
    {
        var left = SQL.NullableParameter(5d);
        var right = SQL.NullableParameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusWithNullableNull()
    {
        var left = SQL.NullableParameter(5d);
        var right = SQL.NullableParameter((double?)null);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) - 3d)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusOnNull()
    {
        var left = SQL.NullableParameter((double?)null);
        var right = 3;
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyWithDbDouble()
    {
        var left = SQL.NullableParameter(2d);
        var right = SQL.Parameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyWithNullableValue()
    {
        var left = SQL.NullableParameter(2d);
        var right = SQL.NullableParameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyWithNullableNull()
    {
        var left = SQL.NullableParameter(2d);
        var right = SQL.NullableParameter((double?)null);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(2d) * 3d)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyOnNull()
    {
        var left = SQL.NullableParameter((double?)null);
        var right = 3;
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideWithDbDouble()
    {
        var left = SQL.NullableParameter(6d);
        var right = SQL.Parameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideWithNullableValue()
    {
        var left = SQL.NullableParameter(6d);
        var right = SQL.NullableParameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideWithNullableNull()
    {
        var left = SQL.NullableParameter(6d);
        var right = SQL.NullableParameter((double?)null);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(6d) / 3d)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideOnNull()
    {
        var left = SQL.NullableParameter((double?)null);
        var right = 3;
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideByZero()
    {
        // Database behavior on division by zero varies:
        // - PostgreSQL raises an error (division by zero)
        // - MySQL returns NULL

#if MYSQL
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) / 0d)).FetchOne();
        Assert.Null(rs.Result);
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) / 0d)).FetchOne();
        });
#endif
    }

    [Fact]
    public async Task Equal_WithDbDoubleRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) == SQL.Parameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) == SQL.NullableParameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) == SQL.NullableParameter((double?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) == 5d)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithDbDoubleRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) != SQL.Parameter(3d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) != SQL.NullableParameter(3d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) != SQL.NullableParameter((double?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) != 3d)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithDbDoubleRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3d) < SQL.Parameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3d) < SQL.NullableParameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3d) < SQL.NullableParameter((double?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(3d) < 5d)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithDbDoubleRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) > SQL.Parameter(3d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) > SQL.NullableParameter(3d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) > SQL.NullableParameter((double?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) > 3d)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithDbDoubleRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) <= SQL.Parameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) <= SQL.NullableParameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) <= SQL.NullableParameter((double?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) <= 5d)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithDbDoubleRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) >= SQL.Parameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) >= SQL.NullableParameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) >= SQL.NullableParameter((double?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(5d) >= 5d)).FetchOne();
        Assert.True(rs.Result);
    }

    // Floating point specific

    [Fact]
    public async Task ParameterNullable_WithFraction()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(0.1) + 0.2)).FetchOne();
        Assert.Equal(0.1 + 0.2, rs.Result);
    }

    [Fact]
    public void ParameterNullable_NaN_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SQL.NullableParameter(double.NaN));
    }

    [Fact]
    public void ParameterNullable_Infinity_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SQL.NullableParameter(double.PositiveInfinity));
    }

}
