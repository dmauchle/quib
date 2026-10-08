using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbDateTimeNullableExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static readonly DateTime _low = new(2026, 3, 14, 9, 26, 53);
    private static readonly DateTime _high = new(2026, 3, 14, 23, 59, 58);

    [Fact]
    public async Task CastFromNull()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Null.As<DbDateTimeNullable>())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task AsNullable()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low).AsNullable())).FetchOne();
        Assert.Equal(_low, rs.Result);
    }

    [Fact]
    public async Task ParameterNullable()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low))).FetchOne();
        Assert.Equal(_low, rs.Result);
    }

    [Fact]
    public async Task ParameterNull()
    {
        DateTime? value = null;
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(value))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithDbDateTimeRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) == SQL.Parameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) == SQL.NullableParameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) == SQL.NullableParameter((DateTime?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) == _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithDbDateTimeRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) != SQL.Parameter(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) != SQL.NullableParameter(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) != SQL.NullableParameter((DateTime?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) != _high)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithDbDateTimeRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) < SQL.Parameter(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) < SQL.NullableParameter(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) < SQL.NullableParameter((DateTime?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) < _high)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithDbDateTimeRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) <= SQL.Parameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) <= SQL.NullableParameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) <= SQL.NullableParameter((DateTime?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) <= _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithDbDateTimeRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_high) > SQL.Parameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_high) > SQL.NullableParameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_high) > SQL.NullableParameter((DateTime?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_high) > _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithDbDateTimeRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) >= SQL.Parameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) >= SQL.NullableParameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) >= SQL.NullableParameter((DateTime?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) >= _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((DateTime?)null) == _low)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((DateTime?)null) != _low)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((DateTime?)null) < _high)).FetchOne();
        Assert.Null(rs.Result);
    }

}
