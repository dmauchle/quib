using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class FetchCodeStyles(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
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
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Bob");
            })
            .Execute();
    }

    private class UsersTable(DbTable table) : IDbTable, IDbTableFactory<UsersTable>
    {
        public static string TableName => "users";
        public static UsersTable Factory(DbTable table) => new UsersTable(table);
     
        public DbColumn<DbText> Name => table.Column<DbText>("name");
    }

    private record User(string Name);

    private class UserMapping(ProjectionBuilder rs, UsersTable user) : IReaderMappingFactory<UsersTable, User>
    {
        public static IReaderMapping<User> Factory(ProjectionBuilder rs, UsersTable user) => new UserMapping(rs, user);
        
        public ProjectedValue<DbText> Name { get; } = rs.Get(user.Name);

        public IProjectedValue[] ObjectIdentity => [];

        public User Map(ResultReader rs)
        {
            return new User(rs.Get(Name));
        }
    }


    [Fact]
    public async Task Projected_FetchStream()
    {
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
    public async Task Projected_FetchArray()
    {
        var rs = await DB.SelectFromTable("users")
            .Project(x => x.Column<DbText>("name"))
            .FetchArray();

        Assert.Equal(2, rs.Length);
        Assert.Contains("Alice", rs);
        Assert.Contains("Bob", rs);
    }

    [Fact]
    public async Task Projected_FetchOne()
    {
        var rs = await DB.SelectFromTable("users")
            .OrderBy(x => [x.Column<DbText>("name")])
            .Project(x => x.Column<DbText>("name"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal("Alice", rs.Result);
    }

    [Fact]
    public async Task Mapped_FetchStream()
    {
        var count = 0;
        var rs = DB.SelectFrom<UsersTable>()
            .Map<UserMapping, User>()
            .FetchStream();
        await foreach (var user in rs)
        {
            Assert.True(user.Name == "Alice" || user.Name == "Bob");
            count++;
        }

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task Mapped_FetchArray()
    {
        var rs = await DB
            .SelectFrom<UsersTable>()
            .Map<UserMapping, User>()
            .FetchArray();

        Assert.Equal(2, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice");
        Assert.Contains(rs, x => x.Name == "Bob");
    }

    [Fact]
    public async Task Mapped_FetchOne()
    {
        var rs = await DB.SelectFrom<UsersTable>()
            .OrderBy(x => [x.Name])
            .Map<UserMapping, User>()
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal("Alice", rs.Result.Name);
    }

}