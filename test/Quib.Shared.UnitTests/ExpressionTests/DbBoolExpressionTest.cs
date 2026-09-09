using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class DbBoolExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task AsNullable()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true).AsNullable())).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task ParameterTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task ParameterFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task ConstantTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Constant(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task ConstantFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Constant(false))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Equal_WithParameterRight_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) == SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithParameterRight_False()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) == SQL.Parameter(false))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableValue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) == SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableNull()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) == SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_TrueWithTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) == true)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_TrueWithFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) == false)).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Equal_FalseWithFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) == false)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_TrueWithFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) != SQL.Parameter(false))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_TrueWithTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) != SQL.Parameter(true))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableValue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) != SQL.NullableParameter(false))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableNull()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) != SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_TrueWithValueFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) != false)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_TrueWithValueTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) != true)).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task LessThan_FalseWithTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) < SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_TrueWithFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) < SQL.Parameter(false))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableValue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) < SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableNull()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) < SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) < true)).FetchOne();
        Assert.True(rs.Result);
    }

    // > operator
    [Fact]
    public async Task GreaterThan_TrueWithFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) > SQL.Parameter(false))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_FalseWithTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) > SQL.Parameter(true))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableValue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) > SQL.NullableParameter(false))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableNull()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) > SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) > false)).FetchOne();
        Assert.True(rs.Result);
    }

    // <= operator
    [Fact]
    public async Task LessOrEqual_TrueWithTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) <= SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_FalseWithTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) <= SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableValue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) <= SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableNull()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) <= SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) <= true)).FetchOne();
        Assert.True(rs.Result);
    }

    // >= operator
    [Fact]
    public async Task GreaterOrEqual_TrueWithTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) >= SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_TrueWithFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) >= SQL.Parameter(false))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableValue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) >= SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableNull()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) >= SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) >= true)).FetchOne();
        Assert.True(rs.Result);
    }

    // & operator
    [Fact]
    public async Task And_TrueWithTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) & SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task And_TrueWithFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) & SQL.Parameter(false))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task And_FalseWithFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) & SQL.Parameter(false))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task And_WithNullableValue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) & SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task And_WithNullableNull_FromTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) & SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task And_WithNullableNull_FromFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) & SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task And_WithValue_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) & true)).FetchOne();
        Assert.True(rs.Result);
    }

    // | operator
    [Fact]
    public async Task Or_TrueWithFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) | SQL.Parameter(false))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Or_FalseWithFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) | SQL.Parameter(false))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Or_TrueWithTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) | SQL.Parameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Or_WithNullableValue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) | SQL.NullableParameter(true))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Or_WithNullableNull_FromFalse()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) | SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Or_WithNullableNull_FromTrue()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(true) | SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Or_WithValue_True()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(false) | true)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Not_True()
    {
        var rs = await DB.Select(x => x.Get(!SQL.Parameter(true))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Not_False()
    {
        var rs = await DB.Select(x => x.Get(!SQL.Parameter(false))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Not_WithNullable_True()
    {
        var rs = await DB.Select(x => x.Get(!SQL.NullableParameter(true))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Not_WithNullable_Null()
    {
        var rs = await DB.Select(x => x.Get(!SQL.NullableParameter((bool?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

}
