using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbUuidNullableExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static readonly Guid _low = Guid.Parse("0192f3a1-5c2e-7a00-8000-000000000001");
    private static readonly Guid _high = Guid.Parse("0192f3a1-5c2e-7a00-8000-0000000000ff");

    [Fact]
    public async Task CastFromNull()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Null.As<DbUuidNullable>())).FetchOne();
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
        Guid? value = null;
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(value))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithDbUuidRight_True()
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
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) == SQL.NullableParameter((Guid?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) == _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithDbUuidRight_True()
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
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) != SQL.NullableParameter((Guid?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) != _high)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithDbUuidRight_True()
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
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) < SQL.NullableParameter((Guid?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) < _high)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithDbUuidRight_True()
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
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) <= SQL.NullableParameter((Guid?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) <= _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithDbUuidRight_True()
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
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_high) > SQL.NullableParameter((Guid?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_high) > _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithDbUuidRight_True()
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
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_low) >= SQL.NullableParameter((Guid?)null))).FetchOne();
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
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((Guid?)null) == _low)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((Guid?)null) != _low)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((Guid?)null) < _high)).FetchOne();
        Assert.Null(rs.Result);
    }

}
