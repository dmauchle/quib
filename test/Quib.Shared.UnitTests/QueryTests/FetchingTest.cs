using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class Fetching(DatabaseFixture env) : DbTestBase(env)
{

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users").CreateWithPrimaryKey("name", ColumnType.VarChar(64));
        }
    }

    [Fact]
    public async Task FetchStream_WithRows_Works()
    {
        var inserted = await DB.InsertIntoTable("users")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Alice");
            })
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Bob");
            })
            .Execute();

        Assert.Equal(2, inserted);

        var count = 0;
        var rs = DB.SelectFromTable("users")
            .Project(x => x.Column<DbText>("name"))
            .FetchStream();
        await foreach (var name in rs)
        {
            Assert.True(name == "Alice" || name == "Bob");
            count++;
        }

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task FetchStream_WithoutRows_Works()
    {
        var rs = DB.SelectFromTable("users")
            .Project(x => x.Column<DbText>("name"))
            .FetchStream();
        await foreach (var name in rs)
        {
            Assert.Fail("There should be no rows in the enumerator");
        }
    }

    [Fact]
    public async Task FetchArray_WithRows_Works()
    {
        var inserted = await DB.InsertIntoTable("users")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Alice");
            })
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Bob");
            })
            .Execute();

        Assert.Equal(2, inserted);

        var rs = await DB.SelectFromTable("users")
            .Project(x => x.Column<DbText>("name"))
            .FetchArray();

        Assert.Equal(2, rs.Length);
        Assert.Contains("Alice", rs);
        Assert.Contains("Bob", rs);
    }

    [Fact]
    public async Task FetchArray_WithoutRows_Works()
    {
        var rs = await DB.SelectFromTable("users")
            .Project(x => x.Column<DbText>("name"))
            .FetchArray();

        Assert.Empty(rs);
    }

    [Fact]
    public async Task FetchOne_WithRow_Works()
    {
        var inserted = await DB.InsertIntoTable("users")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Alice");
            })
            .Execute();

        Assert.Equal(1, inserted);

        var rs = await DB.SelectFromTable("users")
            .Project(x => x.Column<DbText>("name"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal("Alice", rs.Result);
    }

    [Fact]
    public async Task FetchOne_WithoutRow_Works()
    {
        var rs = await DB.SelectFromTable("users")
            .Project(x => x.Column<DbText>("name"))
            .FetchOne();

        Assert.False(rs.HasResult);
    }

}