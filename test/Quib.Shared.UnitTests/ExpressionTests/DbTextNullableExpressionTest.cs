using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class DbTextNullableExpressionTest(DatabaseFixture env) : DbTestBase(env)
{

    [Fact]
    public async Task CastFromNull()
    {
        var rs = await DB.Select(x => x.Get(SQL.NULL.As<DbTextNullable>())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNullable()
    {
        var rs = await DB.Select(x => x.Get(SQL.NullableParameter("Hello"))).FetchOne();
        Assert.Equal("Hello", rs.Result);
    }

    [Fact]
    public async Task ParameterNull()
    {
        string? value = null;
        var rs = await DB.Select(x => x.Get(SQL.NullableParameter(value))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Concat()
    {
        var left = SQL.NullableParameter("Hello");
        var right = " World";
        var rs = await DB.Select(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Equal("Hello World", rs.Result);
    }

    [Fact]
    public async Task ConcatOnNull()
    {
        var left = SQL.NULL.As<DbTextNullable>();
        var right = " World";
        var rs = await DB.Select(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ConcatWithNullable()
    {
        var left = SQL.NullableParameter("Hello");
        var right = SQL.NullableParameter(" World");
        var rs = await DB.Select(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Equal("Hello World", rs.Result);
    }

    [Fact]
    public async Task ConcatWithNull()
    {
        var left = SQL.NullableParameter("Hello");
        var right = SQL.NullableParameter((string?)null);
        var rs = await DB.Select(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Null(rs.Result);
    }

}