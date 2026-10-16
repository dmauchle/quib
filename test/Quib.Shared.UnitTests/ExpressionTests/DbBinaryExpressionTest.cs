using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbBinaryExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static readonly byte[] _value = [0, 1, 2, 127, 128, 255];
    private static readonly byte[] _other = [0, 1, 2, 127, 128, 254];

    [Fact]
    public async Task Parameter()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_value))).FetchOne();
        Assert.Equal(_value, rs.Result);
    }

    [Fact]
    public async Task Parameter_Empty()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(Array.Empty<byte>()))).FetchOne();
        Assert.Empty(rs.Result);
    }

    [Fact]
    public async Task Parameter_AllByteValues()
    {
        // Every byte value must be kept as is (no encoding or escaping issues)
        var value = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(value))).FetchOne();
        Assert.Equal(value, rs.Result);
    }

    [Fact]
    public void Parameter_Null_Throws()
    {
        // In case nullable compiler errors are turned off in the consuming project
        Assert.Throws<ArgumentNullException>(() => SQL.Parameter((byte[])null!));
    }

    [Fact]
    public async Task Equal_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_value) == SQL.Parameter(_value))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_value) == SQL.NullableParameter(_value))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_value) == SQL.NullableParameter((byte[]?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_value) == _value)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_False()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_value) == _other)).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Equal_WithTrailingZero_False()
    {
        // Binary values are not padded, so a trailing zero byte makes a difference
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(new byte[] { 1, 2 }) == new byte[] { 1, 2, 0 })).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_value) != SQL.Parameter(_other))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_value) != SQL.NullableParameter(_other))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_value) != SQL.NullableParameter((byte[]?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_value) != _other)).FetchOne();
        Assert.True(rs.Result);
    }

}
