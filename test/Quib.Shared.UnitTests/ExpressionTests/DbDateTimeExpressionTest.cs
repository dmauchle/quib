using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbDateTimeExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static readonly DateTime _low = new(2026, 3, 14, 9, 26, 53);
    private static readonly DateTime _high = new(2026, 3, 14, 23, 59, 58);

    [Fact]
    public async Task Parameter()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low))).FetchOne();
        Assert.Equal(_low, rs.Result);
    }

    [Fact]
    public async Task Parameter_WithFractionalSeconds_Truncated()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low.AddMilliseconds(999)))).FetchOne();
        Assert.Equal(_low, rs.Result);
    }

    [Fact]
    public async Task Parameter_LocalTime_KindLocal()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low))).FetchOne();
        Assert.Equal(_low, rs.Result);
        Assert.Equal(DateTimeKind.Local, rs.Result.Kind);
    }

    [Fact]
    public async Task Parameter_LocalTime_UtcValue_ConvertedToLocal()
    {
        var value = DateTime.SpecifyKind(_low, DateTimeKind.Utc);
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(value))).FetchOne();
        Assert.Equal(value.ToLocalTime(), rs.Result);
        Assert.Equal(DateTimeKind.Local, rs.Result.Kind);
    }

    [Fact]
    public async Task Parameter_Utc_KindUtc()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<Utc>(_low))).FetchOne();
        Assert.Equal(_low, rs.Result);
        Assert.Equal(DateTimeKind.Utc, rs.Result.Kind);
    }

    [Fact]
    public async Task Parameter_Utc_LocalValue_ConvertedToUtc()
    {
        var value = DateTime.SpecifyKind(_low, DateTimeKind.Local);
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<Utc>(value))).FetchOne();
        Assert.Equal(value.ToUniversalTime(), rs.Result);
        Assert.Equal(DateTimeKind.Utc, rs.Result.Kind);
    }

    [Fact]
    public async Task Parameter_Utc_WithFractionalSeconds_Truncated()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<Utc>(_low.AddMilliseconds(999)))).FetchOne();
        Assert.Equal(_low, rs.Result);
    }

    [Fact]
    public async Task Parameter_Utc_BeforeYear1000_Throws()
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await DB.SelectValues(x => x.Get(SQL.Parameter<Utc>(new DateTime(999, 12, 31, 23, 59, 59)))).FetchOne());
    }

    [Fact]
    public async Task Equal_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) == SQL.Parameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) == SQL.NullableParameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) == SQL.NullableParameter<LocalTime>(null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) == _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_Utc_WithLocalValue_ConvertedToUtc()
    {
        // The value is converted by the date-time storage (time zone) of the left expression, so the same instant is equal
        var utc = DateTime.SpecifyKind(_low, DateTimeKind.Utc);
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<Utc>(utc) == utc.ToLocalTime())).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_LocalTime_WithUtcValue_ConvertedToLocal()
    {
        var local = DateTime.SpecifyKind(_low, DateTimeKind.Local);
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(local) == local.ToUniversalTime())).FetchOne();
        Assert.True(rs.Result);
    }

    //[Fact]
    //public async Task Equal_WithDifferentTimeZone_CompilerError()
    //{
    //    var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) == SQL.Parameter<Utc>(_low))).FetchOne();  // Compiler Error
    //    Assert.True(rs.Result);
    //}

    [Fact]
    public async Task NotEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) != SQL.Parameter<LocalTime>(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) != SQL.NullableParameter<LocalTime>(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) != SQL.NullableParameter<LocalTime>(null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) != _high)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) < SQL.Parameter<LocalTime>(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) < SQL.NullableParameter<LocalTime>(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) < SQL.NullableParameter<LocalTime>(null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) < _high)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) <= SQL.Parameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) <= SQL.NullableParameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) <= SQL.NullableParameter<LocalTime>(null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) <= _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_high) > SQL.Parameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_high) > SQL.NullableParameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_high) > SQL.NullableParameter<LocalTime>(null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_high) > _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) >= SQL.Parameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) >= SQL.NullableParameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) >= SQL.NullableParameter<LocalTime>(null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter<LocalTime>(_low) >= _low)).FetchOne();
        Assert.True(rs.Result);
    }

}
