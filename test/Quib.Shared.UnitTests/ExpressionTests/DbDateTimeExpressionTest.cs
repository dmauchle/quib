using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbDateTimeExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static readonly DateTime _low = new(2026, 3, 14, 9, 26, 53);
    private static readonly DateTime _high = new(2026, 3, 14, 23, 59, 58);

    [Fact]
    public async Task Parameter()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low))).FetchOne();
        Assert.Equal(_low, rs.Result);
    }

    [Fact]
    public async Task Parameter_WithFractionalSeconds_Truncated()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low.AddMilliseconds(999)))).FetchOne();
        Assert.Equal(_low, rs.Result);
    }

    [Fact]
    public async Task Parameter_Utc_ConvertedToLocal()
    {
        var value = DateTime.SpecifyKind(_low, DateTimeKind.Utc);
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(value))).FetchOne();
        Assert.Equal(value.ToLocalTime(), rs.Result);
        Assert.Equal(DateTimeKind.Local, rs.Result.Kind);
    }

    [Fact]
    public async Task AsNullable()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low).AsNullable())).FetchOne();
        Assert.Equal(_low, rs.Result);
    }

    [Fact]
    public async Task Equal_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) == SQL.Parameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) == SQL.NullableParameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) == SQL.NullableParameter((DateTime?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) == _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) != SQL.Parameter(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) != SQL.NullableParameter(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) != SQL.NullableParameter((DateTime?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) != _high)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) < SQL.Parameter(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) < SQL.NullableParameter(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) < SQL.NullableParameter((DateTime?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) < _high)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) <= SQL.Parameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) <= SQL.NullableParameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) <= SQL.NullableParameter((DateTime?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) <= _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_high) > SQL.Parameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_high) > SQL.NullableParameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_high) > SQL.NullableParameter((DateTime?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_high) > _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) >= SQL.Parameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) >= SQL.NullableParameter(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) >= SQL.NullableParameter((DateTime?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_low) >= _low)).FetchOne();
        Assert.True(rs.Result);
    }

}
