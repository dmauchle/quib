using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class DbIntExpressionTest(DatabaseFixture env) : DbTestBase(env)
{

    [Fact]
    public async Task Constant()
    {
        var rs = await DB.Select(x => x.Get(SQL.Constant(3))).FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task Parameter()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(3))).FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task Plus()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(2) + 3)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusNullable()
    {
        var left = SQL.Parameter(2);
        var right = SQL.NullableParameter(3);
        var rs = await DB.Select(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusNull()
    {
        var left = SQL.Parameter(2);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.Select(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

}