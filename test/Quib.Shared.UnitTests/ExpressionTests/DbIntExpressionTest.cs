using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbIntExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task Constant()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Constant(3))).FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task Parameter()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3))).FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task PlusParameterRight()
    {
        var left = SQL.Parameter(2);
        var right = SQL.Parameter(3);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusNullableValue()
    {
        var left = SQL.Parameter(2);
        var right = SQL.NullableParameter(3);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusNullableNull()
    {
        var left = SQL.Parameter(2);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.SelectValues(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task PlusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(2) + 3)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task MinusParameterRight()
    {
        var left = SQL.Parameter(5);
        var right = SQL.Parameter(3);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusNullableValue()
    {
        var left = SQL.Parameter(5);
        var right = SQL.NullableParameter(3);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusNullableNull()
    {
        var left = SQL.Parameter(5);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.SelectValues(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) - 3)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MultiplyParameterRight()
    {
        var left = SQL.Parameter(2);
        var right = SQL.Parameter(3);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyNullableValue()
    {
        var left = SQL.Parameter(2);
        var right = SQL.NullableParameter(3);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyNullableNull()
    {
        var left = SQL.Parameter(2);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.SelectValues(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(2) * 3)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task DivideParameterRight()
    {
        var left = SQL.Parameter(6);
        var right = SQL.Parameter(3);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideNullableValue()
    {
        var left = SQL.Parameter(6);
        var right = SQL.NullableParameter(3);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideNullableNull()
    {
        var left = SQL.Parameter(6);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.SelectValues(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(6) / 3)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task Divide_WithRemainder_Truncates()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(7) / 2)).FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task Divide_Negative_TruncatesTowardZero()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(-7) / 2)).FetchOne();
        Assert.Equal(-3, rs.Result);
    }

    [Fact]
    public async Task Divide_ComparedInDatabase_Truncates()
    {
        // Truncation happens in the database, not when reading the result
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(7) / 2 == 3)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task ModuloParameterRight()
    {
        var left = SQL.Parameter(5);
        var right = SQL.Parameter(2);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloNullableValue()
    {
        var left = SQL.Parameter(5);
        var right = SQL.NullableParameter(2);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloNullableNull()
    {
        var left = SQL.Parameter(5);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.SelectValues(x => x.Get(left % right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ModuloWithValue()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) % 2)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task Equal_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) == SQL.Parameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) == SQL.NullableParameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) == SQL.NullableParameter((int?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) == 5)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) != SQL.Parameter(3))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) != SQL.NullableParameter(3))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) != SQL.NullableParameter((int?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) != 3)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3) < SQL.Parameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3) < SQL.NullableParameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3) < SQL.NullableParameter((int?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(3) < 5)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) > SQL.Parameter(3))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) > SQL.NullableParameter(3))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) > SQL.NullableParameter((int?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) > 3)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) <= SQL.Parameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) <= SQL.NullableParameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) <= SQL.NullableParameter((int?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) <= 5)).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) >= SQL.Parameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) >= SQL.NullableParameter(5))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) >= SQL.NullableParameter((int?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter(5) >= 5)).FetchOne();
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
            return DB.SelectValues(x => x.Get(SQL.Parameter(5) / 0)).FetchOne();
        });
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(5) / 0)).FetchOne();
        });
#endif
    }

    [Fact]
    public async Task ModuloByZero()
    {
        // Database behavior on modulo by zero varies:
        // - PostgreSQL raises an error (division by zero)
        // - MySQL returns NULL, but materialization will fail due to datatype.

#if MYSQL
        await Assert.ThrowsAnyAsync<System.Data.SqlTypes.SqlNullValueException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(5) % 0)).FetchOne();
        });
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.SelectValues(x => x.Get(SQL.Parameter(5) % 0)).FetchOne();
        });
#endif
    }

}