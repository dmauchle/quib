using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.QueryTests;

[Collection(TestCollections.DATABASE)]
public class SelectCodeStyles(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users").CreateWithPrimaryKey("name", ColumnType.VarChar(64));
        }

        await DB.InsertIntoTable("users")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Alice");
            })
            .Execute();
    }

    [Fact]
    public async Task ThroughTableName()
    {
        var rs = await DB
            .SelectFromTable("users")
            .Project(x => x.Column<DbText>("name"))
            .FetchOne();

        Assert.Equal("Alice", rs.Result);
    }


    private class UsersTable(DbTable table) : IDbTable, IDbTableFactory<UsersTable>
    {

        public static string TableName => "users";
        public static UsersTable Factory(DbTable table) => new UsersTable(table);


        public static UsersTable CreateQuery(DbTable table)
        {
            return new UsersTable(table);
        }

        public DbColumn<DbText> Name => table.Column<DbText>("name");

    }

    private class UsersQuery(UsersTable user) : ISelectQueryFactory<UsersQuery>
    {
        public static UsersQuery Factory(SelectQueryBuilder query) => new UsersQuery(query.From<UsersTable>());

        public UsersTable User => user;
    }


    [Fact]
    public async Task ThroughIDbTableFactory()
    {
        var rs = await DB
            .SelectFrom<UsersTable>()
            .Project(x => x.Name)
            .FetchOne();

        Assert.Equal("Alice", rs.Result);
    }

    [Fact]
    public async Task ThroughIDbTable()
    {
        var rs = await DB
            .SelectFrom(table => new UsersTable(table))
            .Project(x => x.Name)
            .FetchOne();

        Assert.Equal("Alice", rs.Result);
    }

    [Fact]
    public async Task ThroughQueryBuilder_WithTableName()
    {
        var rs = await DB
              .Select(x => x.FromTable("users"))
              .Project(x => x.Column<DbText>("name"))
              .FetchOne();

        Assert.Equal("Alice", rs.Result);
    }

    [Fact]
    public async Task ThroughQueryBuilder_WithIDbTableFactory()
    {
        var rs = await DB
              .Select(x => x.From<UsersTable>())
              .Project(x => x.Name)
              .FetchOne();

        Assert.Equal("Alice", rs.Result);
    }

    [Fact]
    public async Task ThroughQueryBuilder_WithIDbTable()
    {
        var rs = await DB
              .Select(query => query.From(table => new UsersTable(table)))
              .Project(x => x.Name)
              .FetchOne();

        Assert.Equal("Alice", rs.Result);
    }

    [Fact]
    public async Task ThroughQueryBuilder_WithISelectQueryFactory()
    {
        var rs = await DB
              .Select<UsersQuery>()
              .Project(x => x.User.Name)
              .FetchOne();

        Assert.Equal("Alice", rs.Result);
    }

    [Fact]
    public async Task ThroughProjectionBuilder()
    {
        var rs = await DB
            .SelectValues(x => x.Get(SQL.Constant(3)))
            .FetchOne();
        Assert.Equal(3, rs.Result);
    }

    [Fact]
    public async Task ThroughSubselect_WithTable()
    {
        var rs = await DB
            .SelectFromTable("users")
            .Select(x => new UsersTable(x))
            .Project(x => x.Name)
            .FetchOne();

        Assert.Equal("Alice", rs.Result);
    }

    [Fact]
    public async Task ThroughSubselect_WithTableAndQuery()
    {
        var rs = await DB
            .SelectFromTable("users")
            .Select((query, table) => new UsersTable(table))
            .Project(x => x.Name)
            .FetchOne();

        Assert.Equal("Alice", rs.Result);
    }

}