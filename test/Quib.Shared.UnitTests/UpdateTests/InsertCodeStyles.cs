using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.QueryTests;

[Collection(TestCollections.DATABASE)]
public class InsertCodeStyles(DatabaseFixture env) : DbTestBase(env)
{

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users").CreateWithPrimaryKey("name", DbType.VarChar(64, nullable: false));
        }
    }

    private async ValueTask AssertResult()
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
    public async Task GenericSytnax()
    {
        await DB.InsertInto("users")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Alice");
            })
            .Execute();

        await AssertResult();
    }

    [Fact]
    public async Task FactoryInterface()
    {
        // Syntax using factory interface
        await DB.InsertInto<UsersTable>().AddRow<UserMapping, User>(new User { Name = "Alice" }).Execute();

        await AssertResult();
    }

    [Fact]
    public async Task FactoryMethods()
    {

        await DB.InsertInto(UsersTable.Factory).AddRow(UserMapping.Write, new User { Name = "Alice" }).Execute();

        await AssertResult();
    }

    [Fact]
    public async Task Lambdas()
    {
        var charlie = new User { Name = "Alice" };
        await DB.InsertInto(x => new UsersTable(x))
            .AddRow((query, table) =>
            {
                query.Set(table.Name, charlie.Name);
            })
            .Execute();
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
                Name = rs.Get(this._name)
            };
        }

        public static void Write(IUpdateQueryBuilder query, UsersTable table, User user)
        {
            query.Set(table.Name, user.Name);
            
        }

    }
}