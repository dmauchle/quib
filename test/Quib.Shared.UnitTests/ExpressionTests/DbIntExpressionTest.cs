using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class DbIntExpressionTest(DatabaseFixture env) : DbTestBase(env)
{

    [Fact]
    public async Task Constant()
    {
        var rs = await DB.Select(x => x.Get(SQL.Constant(3))).FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task Parameter()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(3))).FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task Plus()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(2) + 3)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusNullable()
    {
        var left = SQL.Parameter(2);
        var right = SQL.NullableParameter(3);
        var rs = await DB.Select(x => x.Get(left + right)).FetchOne();
        Assert.Equal(5, rs.Result);
    }

    [Fact]
    public async Task PlusNull()
    {
        var left = SQL.Parameter(2);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.Select(x => x.Get(left + right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Minus()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(5) - 3)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusNullable()
    {
        var left = SQL.Parameter(5);
        var right = SQL.NullableParameter(3);
        var rs = await DB.Select(x => x.Get(left - right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task MinusNull()
    {
        var left = SQL.Parameter(5);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.Select(x => x.Get(left - right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Multiply()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(2) * 3)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyNullable()
    {
        var left = SQL.Parameter(2);
        var right = SQL.NullableParameter(3);
        var rs = await DB.Select(x => x.Get(left * right)).FetchOne();
        Assert.Equal(6, rs.Result);
    }

    [Fact]
    public async Task MultiplyNull()
    {
        var left = SQL.Parameter(2);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.Select(x => x.Get(left * right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Divide()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(6) / 3)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideNullable()
    {
        var left = SQL.Parameter(6);
        var right = SQL.NullableParameter(3);
        var rs = await DB.Select(x => x.Get(left / right)).FetchOne();
        Assert.Equal(2, rs.Result);
    }

    [Fact]
    public async Task DivideNull()
    {
        var left = SQL.Parameter(6);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.Select(x => x.Get(left / right)).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Modulo()
    {
        var rs = await DB.Select(x => x.Get(SQL.Parameter(5) % 2)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloNullable()
    {
        var left = SQL.Parameter(5);
        var right = SQL.NullableParameter(2);
        var rs = await DB.Select(x => x.Get(left % right)).FetchOne();
        Assert.Equal(1, rs.Result);
    }

    [Fact]
    public async Task ModuloNull()
    {
        var left = SQL.Parameter(5);
        var right = SQL.NullableParameter((int?)null);
        var rs = await DB.Select(x => x.Get(left % right)).FetchOne();
        Assert.Null(rs.Result);
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
            return DB.Select(x => x.Get(SQL.Parameter(5) / 0)).FetchOne();
        });
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.Select(x => x.Get(SQL.Parameter(5) / 0)).FetchOne();
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
            return DB.Select(x => x.Get(SQL.Parameter(5) % 0)).FetchOne();
        });
#elif POSTGRES
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() =>
        {
            return DB.Select(x => x.Get(SQL.Parameter(5) % 0)).FetchOne();
        });
#endif
    }

}