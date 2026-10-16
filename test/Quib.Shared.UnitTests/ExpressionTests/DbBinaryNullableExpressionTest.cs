using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbBinaryNullableExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static readonly byte[] _value = [0, 1, 2, 127, 128, 255];
    private static readonly byte[] _other = [0, 1, 2, 127, 128, 254];

    [Fact]
    public async Task NullValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Null<DbBinary>())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNullable()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_value))).FetchOne();
        Assert.Equal(_value, rs.Result);
    }

    [Fact]
    public async Task ParameterNull()
    {
        byte[]? value = null;
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(value))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

#if POSTGRES
    [Fact(Skip = "MySQL only: Postgres has no issue with untyped NULL parameters compared to binary data.")]
    public async Task ParameterNull_TypedAsBinary()
    {
    }
#else
    [Fact]
    public async Task ParameterNull_TypedAsBinary()
    {
        // Depending on the driver state (e.g. test order in Visual Studio), MySQL may receive a NULL parameter as text.
        // Comparing it with binary data fails then ("Cannot convert string ... from binary to utf8mb4"). The state can't
        // be reproduced reliably, so the test verifies that NULL is explicitly typed as binary in the SQL.
        var sql = new List<string>();
        DB.Logger = sql.Add;

        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(_value) == SQL.NullableParameter((byte[]?)null))).FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
        Assert.Contains(sql, x => x.Contains("CAST(NULL AS BINARY)"));
    }
#endif

    [Fact]
    public async Task Equal_WithDbBinaryRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_value) == SQL.Parameter(_value))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_value) == SQL.NullableParameter(_value))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_value) == SQL.NullableParameter((byte[]?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_value) == _value)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithDbBinaryRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_value) != SQL.Parameter(_other))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_value) != SQL.NullableParameter(_other))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_value) != SQL.NullableParameter((byte[]?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(_value) != _other)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Null<DbBinary>() == _value)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Null<DbBinary>() != _value)).FetchOne();
        Assert.Null(rs.Result);
    }

}
