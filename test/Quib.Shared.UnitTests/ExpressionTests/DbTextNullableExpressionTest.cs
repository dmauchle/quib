using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbTextNullableExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task NullValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Null<DbText>())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNullable()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello"))).FetchOne();
        Assert.Equal("Hello", rs.Result);
    }

    [Fact]
    public async Task ParameterNull()
    {
        string? value = null;
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(value))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Concat()
    {
        var left = SQL.NullableParameter("Hello");
        var right = " World";
        var rs = await DB.SelectValues(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Equal("Hello World", rs.Result);
    }

    [Fact]
    public async Task ConcatOnNull()
    {
        var left = SQL.Null<DbText>();
        var right = " World";
        var rs = await DB.SelectValues(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ConcatWithNullable()
    {
        var left = SQL.NullableParameter("Hello");
        var right = SQL.NullableParameter(" World");
        var rs = await DB.SelectValues(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Equal("Hello World", rs.Result);
    }

    [Fact]
    public async Task ConcatWithNull()
    {
        var left = SQL.NullableParameter("Hello");
        var right = SQL.NullableParameter((string?)null);
        var rs = await DB.SelectValues(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithDbTextRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") == SQL.Parameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") == SQL.NullableParameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") == SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") == "Hello")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithDbTextRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") != SQL.Parameter("World"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") != SQL.NullableParameter("World"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") != SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") != "World")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithDbTextRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") < SQL.Parameter("World"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") < SQL.NullableParameter("World"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") < SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") < "World")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithDbTextRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") <= SQL.Parameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") <= SQL.NullableParameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") <= SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") <= "Hello")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithDbTextRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("World") > SQL.Parameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("World") > SQL.NullableParameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("World") > SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("World") > "Hello")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithDbTextRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") >= SQL.Parameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") >= SQL.NullableParameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") >= SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter("Hello") >= "Hello")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((string?)null) == "Hello")).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((string?)null) != "Hello")).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((string?)null) < "World")).FetchOne();
        Assert.Null(rs.Result);
    }

}