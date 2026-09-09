using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class ExpressionTest(DatabaseFixture env) : DbTestBase(env)
{

    [Fact]
    public async Task NullText()
    {
        var rs = await DB.Select(x => x.Get(SQL.Null<DbTextNullable>())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task IntConstant()
    {
        var rs = await DB.Select(x => x.Get(SQL.Constant(3))).FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task StringParameter()
    {
        var rs = await DB.Select(x => x.Get(x.Parameters.Create("Hello"))).FetchOne();
        Assert.Equal("Hello", rs.Result);
    }

    [Fact]
    public async Task IntPlusOperator()
    {
        var rs = await DB.Select(x => x.Get(SQL.Constant(2) + SQL.Constant(3))).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task IntPlusNullOperator()
    {
        var rs = await DB.Select(x => x.Get(SQL.Constant(2) + SQL.Null<DbIntNullable>())).FetchOne();
        Assert.Null(rs.Result);

        rs = await DB.Select(x => x.Get(SQL.Null<DbIntNullable>() + SQL.Constant(2))).FetchOne();
        Assert.Null(rs.Result);
    }

}