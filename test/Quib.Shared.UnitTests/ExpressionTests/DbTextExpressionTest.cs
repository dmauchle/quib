using Xunit.Abstractions;
using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class DbTextExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task Parameter()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello"))).FetchOne();
        Assert.Equal("Hello", rs.Result);
    }

    [Fact]
    public async Task AsNullable()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello").AsNullable())).FetchOne();
        Assert.Equal("Hello", rs.Result);
    }

    [Fact]
    public async Task Concat()
    {
        var left = SQL.Parameter("Hello");
        var right = " World";
        var rs = await DB.Select(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Equal("Hello World", rs.Result);
    }

    [Fact]
    public async Task ConcatWithNullable()
    {
        var left = SQL.Parameter("Hello");
        var right = SQL.NullableParameter(" World");
        var rs = await DB.Select(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Equal("Hello World", rs.Result);
    }

    [Fact]
    public async Task ConcatWithNull()
    {
        var left = SQL.Parameter("Hello");
        var right = SQL.NullableParameter((string?)null);
        var rs = await DB.Select(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") == SQL.Parameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") == SQL.NullableParameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_Null()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") == SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") == "Hello")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") != SQL.Parameter("World"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") != SQL.NullableParameter("World"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") != SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") != "World")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") < SQL.Parameter("World"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") < SQL.NullableParameter("World"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") < SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") < "World")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") <= SQL.Parameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") <= SQL.NullableParameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") <= SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") <= "Hello")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("World") > SQL.Parameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("World") > SQL.NullableParameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("World") > SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("World") > "Hello")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") >= SQL.Parameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") >= SQL.NullableParameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") >= SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello") >= "Hello")).FetchOne();
        Assert.True(rs.Result);
    }

}