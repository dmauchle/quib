using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.QueryTests;

[Collection(TestCollections.DATABASE)]
public class MapCodeStyles(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
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
    public async Task ThroughAnonymousTypes()
    {
        var rs = await DB
            .SelectFromTable("users")
            .Project((rs, x) => new
            {
                Name = rs.Get(x.Column<DbText>("name"))
            })
            .FetchArray((rs, x) => new
            {
                Name = rs.Get(x.Name)
            });

        Assert.Single(rs);
        foreach (var row in rs)
            Assert.Equal("Alice", row.Name);
    }


    private class UsersTable(DbTable table) : IDbTable, IDbTable<UsersTable>
    {

        public static string TableName => "users";
        public static UsersTable CreateFrom(DbTable table) => new UsersTable(table);
    

        public static UsersTable CreateQuery(DbTable table)
        {
            return new UsersTable(table);
        }

        public NotNullColumn<DbText> Name => table.Column<DbText>("name");
     
    }

    private class UsersQuery(UsersTable user) : ISelectContext<UsersQuery>
    {
        public static UsersQuery CreateFrom(SelectQueryBuilder query) => new UsersQuery(query.From<UsersTable>());

        public UsersTable User => user;

    }

    private class User
    {
        public required string Name { get; init; }
    }

    private class UserMapping(IProjectionBuilder query, UsersTable user) : IReaderMapping<UsersTable, User>
    {

        public static IReaderMapping<User> CreateFrom(IProjectionBuilder query, UsersTable user) => new UserMapping(query, user);

        private readonly NotNullProjection<DbText> _name = query.Get(user.Name);

        public IProjectionField[] ObjectIdentity => [];

        public User Map(ResultReader rs)
        {
            return new User
            {
                Name = rs.Get(_name)
            };
        }

    }

    [Fact]
    public async Task ThroughIReaderMappingFactory()
    {
        var rs = await DB
            .SelectFrom<UsersTable>()
            .Map<UserMapping, User>()
            .FetchArray();

        Assert.Single(rs);
        foreach (var row in rs)
            Assert.Equal("Alice", row.Name);
    }

    [Fact]
    public async Task ThroughIReaderMapping()
    {
        var rs = await DB
            .SelectFrom<UsersTable>()
            .Map((rs, x) => new UserMapping(rs, x))
            .FetchArray();

        Assert.Single(rs);
        foreach (var row in rs)
            Assert.Equal("Alice", row.Name);
    }

}