using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.UpdateTests;

[Collection(TestCollections.DATABASE)]
public class InsertCodeStyles(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users").CreateWithPrimaryKey("name", ColumnType.VarChar(64));
        }
    }

    private async ValueTask AssertResult()
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
    public async Task DynamicAPI()
    {
        await DB.InsertIntoTable("users")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Alice");
            })
            .Execute();

        await AssertResult();
    }

    [Fact]
    public async Task TypedAPIWithFactoryInterface()
    {
        await DB.InsertInto<UsersTable>().AddRow<UserMapping, User>(new User { Name = "Alice" }).Execute();

        await AssertResult();
    }

    [Fact]
    public async Task TypedAPIWithFactoryMethod()
    {

        await DB.InsertInto(UsersTable.Factory).AddRow(UserMapping.Write, new User { Name = "Alice" }).Execute();

        await AssertResult();
    }

    [Fact]
    public async Task TypedAPIWithLambdas()
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

        public static IReaderMapping<User> FromDbTable(ProjectionBuilder query, UsersTable user) => new UserMapping(query, user);
    }

    private class UserMapping(ProjectionBuilder query, UsersTable user) : IReaderMappingFactory<UsersTable, User>, IWriterMapping<UsersTable, User>
    {

        public static IReaderMapping<User> Factory(ProjectionBuilder query, UsersTable user) => new UserMapping(query, user);

        private readonly ProjectedValue<DbText> _name = query.Get(user.Name);

        public User Map(ResultReader rs)
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