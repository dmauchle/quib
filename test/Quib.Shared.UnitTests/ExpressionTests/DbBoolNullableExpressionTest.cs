using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class DbBoolNullableExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task CastFromNull()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Null.As<DbBoolNullable>())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNullable()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task ParameterNull()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithDbBool_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) == SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) == SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) == SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) == true)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithDbBool_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) != SQL.Parameter(false))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) != SQL.NullableParameter(false))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) != SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) != false)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithDbBool_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(false) < SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(false) < SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(false) < SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(false) < true)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithDbBool_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) > SQL.Parameter(false))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) > SQL.NullableParameter(false))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) > SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) > false)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithDbBool_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) <= SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) <= SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(false) <= SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) <= true)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithDbBool_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) >= SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) >= SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) >= SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) >= true)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task And_WithDbBool_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) & SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task And_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) & SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task And_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) & SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task And_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(true) & true)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Or_WithDbBool_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(false) | SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Or_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(false) | SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Or_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(false) | SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Or_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(false) | true)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Not_True()
    {
        var rs = await DB.SelectValues(x => x.Get(!SQL.NullableParameter(true))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Not_False()
    {
        var rs = await DB.SelectValues(x => x.Get(!SQL.NullableParameter(false))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Not_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(!SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

}
