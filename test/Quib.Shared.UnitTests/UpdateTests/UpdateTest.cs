using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.UpdateTests;

[Collection(TestCollections.DATABASE)]
public class UpdateTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users")
                .CreateWithPrimaryKey("username", ColumnType.VarChar(64))
                .CreateColumn("firstname", ColumnType.VarChar(64).NotNull())
                .CreateColumn("lastname", ColumnType.VarChar(64).NotNull())
                .CreateColumn("login_count", ColumnType.Int().NotNull());

            await DB.InsertInto<UsersTable>()
               .AddRow((query, table) =>
               {
                   query.Set(table.Username, "asmith");
                   query.Set(table.Firstname, "Alice");
                   query.Set(table.Lastname, "Smith");
                   query.Set(table.LoginCount, 0);
               })
               .Execute();
        }
    }


    [Fact]
    public async Task Update_WithStandardParameter_Works()
    {
        var updated = await DB.Update<UsersTable>()
            .AllRows()
            .With((row, table) =>
            {
                row.Set(table.Firstname, "Bob");
            })
            .Execute();

        Assert.Equal(1, updated);

        var users = await DB.SelectFrom<UsersTable>().Map<UserMapping, User>().FetchArray();

        Assert.Single(users);
        var user = users[0];
        Assert.Equal("Bob", user.Firstname);
    }

    [Fact]
    public async Task Update_WithExplicitParameter_Works()
    {
        var updated = await DB.Update<UsersTable>()
            .AllRows()
            .With((row, table) =>
            {
                row.Set(table.Firstname, SQL.Parameter("Bob"));
            })
            .Execute();

        Assert.Equal(1, updated);

        var users = await DB.SelectFrom<UsersTable>().Map<UserMapping, User>().FetchArray();

        Assert.Single(users);
        var user = users[0];
        Assert.Equal("Bob", user.Firstname);
    }

    [Fact]
    public async Task Update_WithOtherColumn_Works()
    {
        var updated = await DB.Update<UsersTable>()
            .AllRows()
            .With((row, table) =>
            {
                row.Set(table.Firstname, table.Lastname);
            })
            .Execute();

        Assert.Equal(1, updated);

        var users = await DB.SelectFrom<UsersTable>().Map<UserMapping, User>().FetchArray();

        Assert.Single(users);
        var user = users[0];
        Assert.Equal("Smith", user.Firstname);
        Assert.Equal("Smith", user.Lastname);
    }

    [Fact]
    public async Task Update_WithComplexExpression_Works()
    {
        var updated = await DB.Update<UsersTable>()
            .AllRows()
            .With((row, table) =>
            {
                row.Set(table.LoginCount, table.LoginCount + 1);
            })
            .Execute();

        Assert.Equal(1, updated);

        var users = await DB.SelectFrom<UsersTable>().Map<UserMapping, User>().FetchArray();
        Assert.Single(users);
        Assert.Equal(1, users[0].LoginCount);

        // Second update
        updated = await DB.Update<UsersTable>()
            .AllRows()
            .With((row, table) =>
            {
                row.Set(table.LoginCount, table.LoginCount + 2);
            })
            .Execute();

        Assert.Equal(1, updated);

        users = await DB.SelectFrom<UsersTable>().Map<UserMapping, User>().FetchArray();
        Assert.Single(users);
        Assert.Equal(3, users[0].LoginCount);
    }


    private class UsersTable(DbTable table) : IDbTable, IDbTable<UsersTable>
    {

        public static string TableName => "users";
        public static UsersTable CreateFrom(DbTable table) => new UsersTable(table);

        public static UsersTable CreateQuery(DbTable table)
        {
            return new UsersTable(table);
        }

        public DbColumn<DbText> Username => table.Column<DbText>("username");
        public DbColumn<DbText> Firstname => table.Column<DbText>("firstname");
        public DbColumn<DbText> Lastname => table.Column<DbText>("lastname");
        public DbColumn<DbInt> LoginCount => table.Column<DbInt>("login_count");
    }

    private class User
    {
        public required string Username { get; init; }
        public required string Firstname { get; init; }
        public required string Lastname { get; init; }
        public required int LoginCount { get; init; }
    }

    private class UserMapping(IProjectionBuilder query, UsersTable user) : IReaderMapping<UsersTable, User>
    {

        public static IReaderMapping<User> CreateFrom(IProjectionBuilder query, UsersTable user) => new UserMapping(query, user);

        private readonly ProjectedValue<DbText> _username = query.Get(user.Username);
        private readonly ProjectedValue<DbText> _firstname = query.Get(user.Firstname);
        private readonly ProjectedValue<DbText> _lastname = query.Get(user.Lastname);
        private readonly ProjectedValue<DbInt> _loginCount = query.Get(user.LoginCount);

        public IProjectedValue[] ObjectIdentity => [];

        public User Map(ResultReader rs)
        {
            return new User
            {
                Username = rs.Get(this._username),
                Firstname = rs.Get(this._firstname),
                Lastname = rs.Get(this._lastname),
                LoginCount = rs.Get(this._loginCount)
            };
        }

    }


}