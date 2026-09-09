using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class DbIntNullableExpressionTest(DatabaseFixture env) : DbTestBase(env)
{

    [Fact]
    public async Task CastFromNull()
    {
        var rs = await DB.Select(x => x.Get(SQL.NULL.As<DbIntNullable>())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNull()
    {
        int? value = null;
        var rs = await DB.Select(x => x.Get(SQL.NullableParameter(value))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNullable()
    {
        var rs = await DB.Select(x => x.Get(SQL.NullableParameter(12))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Equal(12, rs.Result);
    }

    [Fact]
    public async Task Plus()
    {
        var rs = await DB.Select(x => x.Get(SQL.NullableParameter(2) + 3)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusNullable()
    {
        var left = SQL.NullableParameter(2);
        var right = SQL.NullableParameter(3);
        var rs = await DB.Select(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusNull()
    {
        var left = SQL.NullableParameter(2);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.Select(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task PlusOnNull()
    {
        var left = SQL.NullableParameter((int?)null);
        var right = 3;
        var rs = await DB.Select(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

}
