using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbDoubleExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task Constant()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(3d))).FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task Parameter()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3d))).FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task PlusParameterRight()
    {
        var left = SQL.Parameter(2d);
        var right = SQL.Parameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusNullableValue()
    {
        var left = SQL.Parameter(2d);
        var right = SQL.NullableParameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusNullableNull()
    {
        var left = SQL.Parameter(2d);
        var right = SQL.NullableParameter((double?)null);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task PlusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(2d) + 3d)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task MinusParameterRight()
    {
        var left = SQL.Parameter(5d);
        var right = SQL.Parameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusNullableValue()
    {
        var left = SQL.Parameter(5d);
        var right = SQL.NullableParameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusNullableNull()
    {
        var left = SQL.Parameter(5d);
        var right = SQL.NullableParameter((double?)null);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) - 3d)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MultiplyParameterRight()
    {
        var left = SQL.Parameter(2d);
        var right = SQL.Parameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyNullableValue()
    {
        var left = SQL.Parameter(2d);
        var right = SQL.NullableParameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyNullableNull()
    {
        var left = SQL.Parameter(2d);
        var right = SQL.NullableParameter((double?)null);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(2d) * 3d)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task DivideParameterRight()
    {
        var left = SQL.Parameter(6d);
        var right = SQL.Parameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideNullableValue()
    {
        var left = SQL.Parameter(6d);
        var right = SQL.NullableParameter(3d);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideNullableNull()
    {
        var left = SQL.Parameter(6d);
        var right = SQL.NullableParameter((double?)null);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(6d) / 3d)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task Equal_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) == SQL.Parameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) == SQL.NullableParameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) == SQL.NullableParameter((double?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) == 5d)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) != SQL.Parameter(3d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) != SQL.NullableParameter(3d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) != SQL.NullableParameter((double?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) != 3d)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3d) < SQL.Parameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3d) < SQL.NullableParameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3d) < SQL.NullableParameter((double?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3d) < 5d)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) > SQL.Parameter(3d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) > SQL.NullableParameter(3d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) > SQL.NullableParameter((double?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) > 3d)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) <= SQL.Parameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) <= SQL.NullableParameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) <= SQL.NullableParameter((double?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) <= 5d)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) >= SQL.Parameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) >= SQL.NullableParameter(5d))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) >= SQL.NullableParameter((double?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5d) >= 5d)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task DivideByZero()
    {
        // Database behavior on division by zero varies:
        // - PostgreSQL raises an error (division by zero)
        // - MySQL returns NULL, but materialization will fail due to datatype.

#if MYSQL
        await Assert.ThrowsAnyAsync<System.Data.SqlTypes.SqlNullValueException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(5d) / 0d)).FetchOne();
        });
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(5d) / 0d)).FetchOne();
        });
#endif
    }

    // Floating point specific

    [Fact]
    public async Task Constant_WithFraction()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(-2.25))).FetchOne();
        Assert.Equal(-2.25, rs.Result);
    }

    [Fact]
    public async Task Constant_WithLargeExponent()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(1.5e300))).FetchOne();
        Assert.Equal(1.5e300, rs.Result);
    }

    [Fact]
    public async Task Constant_WithSmallExponent()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(1.5e-300))).FetchOne();
        Assert.Equal(1.5e-300, rs.Result);
    }

    [Fact]
    public async Task Constant_MaxValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(double.MaxValue))).FetchOne();
        Assert.Equal(double.MaxValue, rs.Result);
    }

    [Fact]
    public async Task Constant_CalculatedAsDouble()
    {
        // Calculated as double, not as decimal (would be exactly 0.3)
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(0.1) + SQL.Constant(0.2))).FetchOne();
        Assert.Equal(0.1 + 0.2, rs.Result);
    }

    [Fact]
    public async Task Parameter_CalculatedAsDouble()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(0.1) + 0.2)).FetchOne();
        Assert.Equal(0.1 + 0.2, rs.Result);
    }

    [Fact]
    public async Task Parameter_MaxValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(double.MaxValue))).FetchOne();
        Assert.Equal(double.MaxValue, rs.Result);
    }

    [Fact]
    public async Task Parameter_MinValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(double.MinValue))).FetchOne();
        Assert.Equal(double.MinValue, rs.Result);
    }

    [Fact]
    public void Parameter_NaN_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SQL.Parameter(double.NaN));
    }

    [Fact]
    public void Parameter_PositiveInfinity_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SQL.Parameter(double.PositiveInfinity));
    }

    [Fact]
    public void Parameter_NegativeInfinity_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SQL.Parameter(double.NegativeInfinity));
    }

    [Fact]
    public void Constant_NaN_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SQL.Constant(double.NaN));
    }

    [Fact]
    public void Constant_Infinity_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SQL.Constant(double.PositiveInfinity));
    }

    [Fact]
    public async Task Divide_WithFraction()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(7d) / 2d)).FetchOne();
        Assert.Equal(3.5, rs.Result);
    }

    [Fact]
    public async Task Multiply_Overflow_Throws()
    {
        // Both databases raise an error (out of range) instead of returning infinity
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(double.MaxValue) * 2d)).FetchOne();
        });
    }

}
