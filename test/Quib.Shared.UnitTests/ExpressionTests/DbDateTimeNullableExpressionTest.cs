using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbDateTimeNullableExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static readonly DateTime _low = new(2026, 3, 14, 9, 26, 53);
    private static readonly DateTime _high = new(2026, 3, 14, 23, 59, 58);

    [Fact]
    public async Task NullValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Null<DbDateTime<LocalTime>>())).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ParameterNullable()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low))).FetchOne();
        Assert.Equal(_low, rs.Result);
    }

    [Fact]
    public async Task ParameterNullable_LocalTime_KindLocal()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low))).FetchOne();
        Assert.Equal(DateTimeKind.Local, rs.Result!.Value.Kind);
    }

    [Fact]
    public async Task ParameterNullable_Utc_KindUtc()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<Utc>(_low))).FetchOne();
        Assert.Equal(_low, rs.Result);
        Assert.Equal(DateTimeKind.Utc, rs.Result!.Value.Kind);
    }

    [Fact]
    public async Task ParameterNullable_Utc_LocalValue_ConvertedToUtc()
    {
        var value = DateTime.SpecifyKind(_low, DateTimeKind.Local);
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<Utc>(value))).FetchOne();
        Assert.Equal(value.ToUniversalTime(), rs.Result);
    }

    [Fact]
    public async Task ParameterNull()
    {
        DateTime? value = null;
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(value))).FetchOne();
        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithDbDateTimeRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) == SQL.Parameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) == SQL.NullableParameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) == SQL.NullableParameter<LocalTime>(null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) == _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithDbDateTimeRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) != SQL.Parameter<LocalTime>(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) != SQL.NullableParameter<LocalTime>(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) != SQL.NullableParameter<LocalTime>(null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) != _high)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithDbDateTimeRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) < SQL.Parameter<LocalTime>(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) < SQL.NullableParameter<LocalTime>(_high))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) < SQL.NullableParameter<LocalTime>(null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) < _high)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithDbDateTimeRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) <= SQL.Parameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) <= SQL.NullableParameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) <= SQL.NullableParameter<LocalTime>(null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) <= _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithDbDateTimeRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_high) > SQL.Parameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_high) > SQL.NullableParameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_high) > SQL.NullableParameter<LocalTime>(null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_high) > _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithDbDateTimeRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) >= SQL.Parameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) >= SQL.NullableParameter<LocalTime>(_low))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableNull_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) >= SQL.NullableParameter<LocalTime>(null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(_low) >= _low)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(null) == _low)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(null) != _low)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_NullWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.NullableParameter<LocalTime>(null) < _high)).FetchOne();
        Assert.Null(rs.Result);
    }

}
