using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.QueryTests;

[Collection(TestCollections.DATABASE)]
public class SelectFilterTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private class UsersTable(DbTable table) : IDbTable, IDbTableFactory<UsersTable>
    {

        public static string TableName => "users";
        public static UsersTable Factory(DbTable table) => new UsersTable(table);

        public DbColumn<DbText> Name => field ??= table.Column<DbText>("name");
        public DbColumn<DbText> Lastname => field ??= table.Column<DbText>("lastname");
        public DbColumn<DbTextNullable> City => field ??= table.Column<DbTextNullable>("city");
    }

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var installer = new DbInstaller(DB.Connection))
        {
            installer.Table("users")
                .CreateWithPrimaryKey("name", ColumnType.VarChar(64))
                .AddColumn("lastname", ColumnType.VarChar(64).NotNull())
                .AddColumn("city", ColumnType.VarChar(64).Nullable());
        }

        await DB.InsertInto<UsersTable>()
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Alice");
                row.Set(user.Lastname, "Smith");
                row.Set(user.City, "New York");
            })
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Bob");
                row.Set(user.Lastname, "Jones");
            })
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Charlie");
                row.Set(user.Lastname, "Jones");
                row.Set(user.City, "New York");
            })
            .Execute();
    }

    [Fact]
    public async Task FilterOnNotNull()
    {
        var rs = await DB.SelectFrom<UsersTable>().Filter(x => x.Name == "Alice").Project((rs, x) => rs.Get(x.Name)).FetchArray();
        Assert.Single(rs);
        Assert.Equal("Alice", rs[0]);
    }

    [Fact]
    public async Task FilterOnNullable()
    {
        var rs = await DB.SelectFrom<UsersTable>().Filter(x => x.City == "New York").Project((rs, x) => rs.Get(x.Name)).FetchArray();
        Assert.Equal(2, rs.Length);
        Assert.Contains(rs, x => x == "Alice");
        Assert.Contains(rs, x => x == "Charlie");
    }

    [Fact]
    public async Task FilterNull()
    {
        var rs = await DB.SelectFrom<UsersTable>().Filter(x => x.City.IsNull()).Project((rs, x) => rs.Get(x.Name)).FetchArray();
        Assert.Single(rs);
        Assert.Equal("Bob", rs[0]);
    }

    [Fact]
    public async Task DualFilter()
    {
        var rs = await DB.SelectFrom<UsersTable>()
            .Filter(x => x.City == "New York")
            .Filter(x => x.Lastname == "Jones")
            .Project((rs, x) => rs.Get(x.Name)).FetchArray();
        Assert.Single(rs);
        Assert.Equal("Charlie", rs[0]);
    }

    [Fact]
    public async Task AndFilter()
    {
        var rs = await DB.SelectFrom<UsersTable>()
            .Filter(x => x.City == "New York" & x.Lastname == "Jones")
            .Project((rs, x) => rs.Get(x.Name)).FetchArray();
        Assert.Single(rs);
        Assert.Equal("Charlie", rs[0]);
    }

    [Fact]
    public async Task OrFilter()
    {
        var rs = await DB.SelectFrom<UsersTable>()
            .Filter(x => x.Name == "Alice" | x.Name == "Bob")
            .Project((rs, x) => rs.Get(x.Name)).FetchArray();

        Assert.Equal(2, rs.Length);
        Assert.Contains(rs, x => x == "Alice");
        Assert.Contains(rs, x => x == "Bob");
    }

    [Fact]
    public async Task MultiAndFilter()
    {
        var rs = await DB.SelectFrom<UsersTable>()
            .Filter(x => x.City == "New York" & x.Lastname == "Jones" & x.Name == "Charlie")
            .Project((rs, x) => rs.Get(x.Name)).FetchArray();
        Assert.Single(rs);
        Assert.Equal("Charlie", rs[0]);
    }

    [Fact]
    public async Task DynamicFilter()
    {
        var rs = await DB.SelectFromTable("users")
            .Filter(x => x.Column<DbText>("name") == "Alice")
            .Project((rs, x) => rs.Get(x.Column<DbText>("name")))
            .FetchArray();
        Assert.Single(rs);
        Assert.Equal("Alice", rs[0]);
    }

}