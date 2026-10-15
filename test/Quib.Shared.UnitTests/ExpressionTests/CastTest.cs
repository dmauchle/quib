using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class CastTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    // DbInt

    [Fact]
    public async Task Int_AsLong()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(2).AsLong() + SQL.Parameter(3L))).FetchOne();
        Assert.Equal(5L, rs.Result);
    }

    [Fact]
    public async Task Int_AsLong_BeyondIntRange()
    {
        // Calculated as long, so it doesn't overflow
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(int.MaxValue).AsLong() + 1L)).FetchOne();
        Assert.Equal(int.MaxValue + 1L, rs.Result);
    }

    [Fact]
    public async Task Int_AsDouble()
    {
        // Floating point division, not integer division
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(7).AsDouble() / 2d)).FetchOne();
        Assert.Equal(3.5, rs.Result);
    }

    [Fact]
    public async Task Int_AsDouble_CalculatedAsDouble()
    {
        // Calculated as double, not as decimal (would be exactly 1.3)
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(1).AsDouble() + SQL.Parameter(0.1) + 0.2)).FetchOne();
        Assert.Equal(1d + 0.1 + 0.2, rs.Result);
    }

    [Fact]
    public async Task Int_AsDecimal()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(7).AsDecimal() / 2m)).FetchOne();
        Assert.Equal(3.5m, rs.Result);
    }

    [Fact]
    public async Task Int_AsDecimal_MinAndMaxValue()
    {
        var rsMin = await DB.SelectValues(x => x.Get(SQL.Parameter(int.MinValue).AsDecimal())).FetchOne();
        var rsMax = await DB.SelectValues(x => x.Get(SQL.Parameter(int.MaxValue).AsDecimal() * 10m)).FetchOne();
        Assert.Equal(int.MinValue, rsMin.Result);
        Assert.Equal(int.MaxValue * 10m, rsMax.Result);
    }

    // DbInt (nullable)

    [Fact]
    public async Task IntNullable_AsLong_WithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(int.MaxValue).AsLong() + 1L)).FetchOne();
        Assert.Equal(int.MaxValue + 1L, rs.Result);
    }

    [Fact]
    public async Task IntNullable_AsLong_WithNull()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((int?)null).AsLong())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task IntNullable_AsDouble_WithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(7).AsDouble() / 2d)).FetchOne();
        Assert.Equal(3.5, rs.Result);
    }

    [Fact]
    public async Task IntNullable_AsDouble_WithNull()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((int?)null).AsDouble())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task IntNullable_AsDecimal_WithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(7).AsDecimal() / 2m)).FetchOne();
        Assert.Equal(3.5m, rs.Result);
    }

    [Fact]
    public async Task IntNullable_AsDecimal_WithNull()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((int?)null).AsDecimal())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    // DbLong

    [Fact]
    public async Task Long_AsDouble()
    {
        // Floating point division, not integer division
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(7_000_000_001L).AsDouble() / 2d)).FetchOne();
        Assert.Equal(3_500_000_000.5, rs.Result);
    }

    [Fact]
    public async Task Long_AsDouble_LosesPrecisionAbove2Pow53()
    {
        // Like in C#, long values above 2^53 can't be represented exactly as double
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(9_007_199_254_740_993L).AsDouble())).FetchOne();
        Assert.Equal(9_007_199_254_740_992d, rs.Result);  // 2^53 + 1 is rounded to 2^53
    }

    [Fact]
    public async Task Long_AsDecimal()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(7_000_000_001L).AsDecimal() / 2m)).FetchOne();
        Assert.Equal(3_500_000_000.5m, rs.Result);
    }

    [Fact]
    public async Task Long_AsDecimal_MinAndMaxValue()
    {
        // All 19 digits are kept (MySQL's DECIMAL defaults to 10 digits)
        var rsMin = await DB.SelectValues(x => x.Get(SQL.Parameter(long.MinValue).AsDecimal())).FetchOne();
        var rsMax = await DB.SelectValues(x => x.Get(SQL.Parameter(long.MaxValue).AsDecimal() + 1m)).FetchOne();
        Assert.Equal(long.MinValue, rsMin.Result);
        Assert.Equal(long.MaxValue + 1m, rsMax.Result);
    }

    // DbLong (nullable)

    [Fact]
    public async Task LongNullable_AsDouble_WithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(7_000_000_001L).AsDouble() / 2d)).FetchOne();
        Assert.Equal(3_500_000_000.5, rs.Result);
    }

    [Fact]
    public async Task LongNullable_AsDouble_WithNull()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((long?)null).AsDouble())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LongNullable_AsDecimal_WithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(long.MaxValue).AsDecimal() + 1m)).FetchOne();
        Assert.Equal(long.MaxValue + 1m, rs.Result);
    }

    [Fact]
    public async Task LongNullable_AsDecimal_WithNull()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((long?)null).AsDecimal())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    // DbDecimal

    [Fact]
    public async Task Decimal_AsDouble()
    {
        // Calculated as double, not as decimal (would be exactly 0.3)
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(0.1m).AsDouble() + 0.2)).FetchOne();
        Assert.Equal(0.1 + 0.2, rs.Result);
    }

    [Fact]
    public async Task Decimal_AsDouble_MaxValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(decimal.MaxValue).AsDouble())).FetchOne();
        Assert.Equal((double)decimal.MaxValue, rs.Result);
    }

    [Fact]
    public async Task Decimal_AsDouble_LosesPrecision()
    {
        // Digits beyond the precision of double (about 15-17 significant digits) are lost.
        // Compared to the (correctly rounded) double literal, as .NET's decimal to double conversion may differ in the last digit.
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(0.12345678901234567890123m).AsDouble())).FetchOne();
        Assert.Equal(0.12345678901234567890123, rs.Result);
    }

    // DbDecimal (nullable)

    [Fact]
    public async Task DecimalNullable_AsDouble_WithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter(7.5m).AsDouble() / 2d)).FetchOne();
        Assert.Equal(3.75, rs.Result);
    }

    [Fact]
    public async Task DecimalNullable_AsDouble_WithNull()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter((decimal?)null).AsDouble())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

}
