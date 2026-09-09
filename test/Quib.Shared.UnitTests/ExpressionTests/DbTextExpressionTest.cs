using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class DbTextExpressionTest(DatabaseFixture env) : DbTestBase(env)
{

    [Fact]
    public async Task Parameter()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter("Hello"))).FetchOne();
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

}