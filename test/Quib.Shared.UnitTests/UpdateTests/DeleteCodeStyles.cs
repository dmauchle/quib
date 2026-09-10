using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.UpdateTests;

[Collection(TestCollections.DATABASE)]
public class DeleteCodeStyles(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users").CreateWithPrimaryKey("name", ColumnType.VarChar(64));
            await DB.InsertInto<UsersTable>().AddRow<UserMapping, User>(new User { Name = "Alice" }).Execute();
        }
    }

    private async ValueTask AssertResult()
    {
        var rs = await DB
            .SelectFrom<UsersTable>()
            .Map<UserMapping, User>()
            .FetchArray();

        Assert.Empty(rs);
    }

    [Fact]
    public async Task GenericSytnax()
    {
        await DB.DeleteFromTable("users")
            .AllRows()
            .Execute();

        await AssertResult();
    }

    [Fact]
    public async Task FactoryInterface()
    {
        await DB.DeleteFrom<UsersTable>().AllRows().Execute();

        await AssertResult();
    }

    [Fact]
    public async Task FactoryMethods()
    {
        await DB.DeleteFrom(UsersTable.Factory).AllRows().Execute();

        await AssertResult();
    }

    [Fact]
    public async Task Lambdas()
    {
        var bob = new User { Name = "Bob" };
        await DB.DeleteFrom(x => new UsersTable(x))
            .AllRows()
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