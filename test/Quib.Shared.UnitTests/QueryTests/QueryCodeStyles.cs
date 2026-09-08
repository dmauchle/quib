using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.QueryTests;

[Collection(TestCollections.DATABASE)]
public class QueryCodeStyles(DatabaseFixture env) : DbTestBase(env)
{

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users").CreateWithPrimaryKey("name", DbType.VarChar(64, nullable: false));
        }

        await DB.InsertInto("users")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Alice");
            })
            .Execute();
    }

    [Fact]
    public async Task GenericSyntax()
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

    private class User
    {
        public required string Name { get; init; }

        public static IReaderMapping<UsersTable, User> FromDbTable(ProjectionBuilder query, UsersTable user) => new UserMapping(query, user);
    }

    private class UserMapping(ProjectionBuilder query, UsersTable user) : IReaderMappingFactory<UsersTable, User>, IWriterMapping<UsersTable, User>
    {

        public static IReaderMapping<UsersTable, User> Factory(ProjectionBuilder query, UsersTable user) => new UserMapping(query, user);

        private readonly ProjectedValue<DbText> _name = query.Get(user.Name);

        public User Map(ResultRow rs)
        {
            return new User
            {
                Name = rs.Get(_name)
            };
        }

        public static void Write(IUpdateQueryBuilder query, UsersTable table, User user)
        {
            query.Set(table.Name, user.Name);
        }

    }

    [Fact]
    public async Task FactoryInterface()
    {
        var rs = await DB
            .SelectFrom<UsersTable>()
            .Map<UserMapping, User>()
            .FetchArrayAsync();

        Assert.Single(rs);
        foreach (var row in rs)
            Assert.Equal("Alice", row.Name);
    }

    [Fact]
    public async Task FactoryMethod()
    {
        var rs = await DB
            .SelectFrom(UsersTable.Factory)
            .Map(User.FromDbTable)
            .FetchArrayAsync();

        Assert.Single(rs);
        foreach (var row in rs)
            Assert.Equal("Alice", row.Name);
    }

    [Fact]
    public async Task Lambdas()
    {
        var rs = await DB
              .SelectFrom(x => new UsersTable(x))
              .Project((rs, x) => new UserMapping(rs, x))
              .FetchArray((rs, x) => x.Map(rs));

        Assert.Single(rs);
        foreach (var row in rs)
            Assert.Equal("Alice", row.Name);
    }
}