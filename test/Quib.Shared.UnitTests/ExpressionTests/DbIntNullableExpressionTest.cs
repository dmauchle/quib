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

    [Fact]
    public async Task Minus()
    {
        var rs = await DB.Select(x => x.Get(SQL.NullableParameter(5) - 3)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusNullable()
    {
        var left = SQL.NullableParameter(5);
        var right = SQL.NullableParameter(3);
        var rs = await DB.Select(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusNull()
    {
        var left = SQL.NullableParameter(5);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.Select(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MinusOnNull()
    {
        var left = SQL.NullableParameter((int?)null);
        var right = 3;
        var rs = await DB.Select(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Multiply()
    {
        var rs = await DB.Select(x => x.Get(SQL.NullableParameter(2) * 3)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyNullable()
    {
        var left = SQL.NullableParameter(2);
        var right = SQL.NullableParameter(3);
        var rs = await DB.Select(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyNull()
    {
        var left = SQL.NullableParameter(2);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.Select(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task MultiplyOnNull()
    {
        var left = SQL.NullableParameter((int?)null);
        var right = 3;
        var rs = await DB.Select(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Divide()
    {
        var rs = await DB.Select(x => x.Get(SQL.NullableParameter(6) / 3)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideNullable()
    {
        var left = SQL.NullableParameter(6);
        var right = SQL.NullableParameter(3);
        var rs = await DB.Select(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideNull()
    {
        var left = SQL.NullableParameter(6);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.Select(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideOnNull()
    {
        var left = SQL.NullableParameter((int?)null);
        var right = 3;
        var rs = await DB.Select(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Modulo()
    {
        var rs = await DB.Select(x => x.Get(SQL.NullableParameter(5) % 2)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloNullable()
    {
        var left = SQL.NullableParameter(5);
        var right = SQL.NullableParameter(2);
        var rs = await DB.Select(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloNull()
    {
        var left = SQL.NullableParameter(5);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.Select(x => x.Get(left % right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task ModuloOnNull()
    {
        var left = SQL.NullableParameter((int?)null);
        var right = 2;
        var rs = await DB.Select(x => x.Get(left % right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task DivideByZero()
    {
        // Database behavior on division by zero varies:
        // - PostgreSQL raises an error (division by zero)
        // - MySQL returns NULL

#if MYSQL
        var rs = await DB.Select(x => x.Get(SQL.NullableParameter(5) / 0)).FetchOne();
        Assert.Null(rs.Result);
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.Select(x => x.Get(SQL.NullableParameter(5) / 0)).FetchOne();
        });
#endif
    }

    [Fact]
    public async Task ModuloByZero()
    {
        // Database behavior on modulo by zero varies:
        // - PostgreSQL raises an error (division by zero)
        // - MySQL returns NULL

#if MYSQL
        var rs = await DB.Select(x => x.Get(SQL.NullableParameter(5) % 0)).FetchOne();
        Assert.Null(rs.Result);
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.Select(x => x.Get(SQL.NullableParameter(5) % 0)).FetchOne();
        });
#endif
    }

}
