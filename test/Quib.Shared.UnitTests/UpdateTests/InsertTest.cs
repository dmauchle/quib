using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.UpdateTests;

[Collection(TestCollections.DATABASE)]
public class InsertTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users")
                .CreateWithPrimaryKey("username", ColumnType.VarChar(64))
                .CreateColumn("firstname", ColumnType.VarChar(64).NotNull())
                .CreateColumn("lastname", ColumnType.VarChar(64).NotNull());
        }
    }


    [Fact]
    public async Task Insert_StandardParameter_Works()
    {
        var insert = await DB.InsertInto<UsersTable>()
            .AddRow((query, table) =>
            {
                query.Set(table.Username, "asmith");
                query.Set(table.Firstname, "Alice");
                query.Set(table.Lastname, "Smith");
            })
            .Execute();

        Assert.Equal(1, insert);

        var users = await DB.SelectFrom<UsersTable>().Map<UserMapping, User>().FetchArray();

        Assert.NotNull(users);
        Assert.Single(users);
        var user = users[0];
        Assert.Equal("asmith", user.Username);
        Assert.Equal("Alice", user.Firstname);
        Assert.Equal("Smith", user.Lastname);
    }

    [Fact]
    public async Task Insert_ExplicitParameter_Works()
    {
        var insert = await DB.InsertInto<UsersTable>()
            .AddRow((query, table) =>
            {
                query.Set(table.Username, SQL.Parameter("asmith"));
                query.Set(table.Firstname, SQL.Parameter("Alice"));
                query.Set(table.Lastname, SQL.Parameter("Smith"));
            })
            .Execute();

        Assert.Equal(1, insert);

        var users = await DB.SelectFrom<UsersTable>().Map<UserMapping, User>().FetchArray();

        Assert.NotNull(users);
        Assert.Single(users);
        var user = users[0];
        Assert.Equal("asmith", user.Username);
        Assert.Equal("Alice", user.Firstname);
        Assert.Equal("Smith", user.Lastname);
    }


    private class UsersTable(DbTable table) : IDbTable, IDbTable<UsersTable>
    {

        public static string TableName => "users";
        public static UsersTable CreateFrom(DbTable table) => new UsersTable(table);

        public static UsersTable CreateQuery(DbTable table)
        {
            return new UsersTable(table);
        }

        public NotNullColumn<DbText> Username => table.Column<DbText>("username");
        public NotNullColumn<DbText> Firstname => table.Column<DbText>("firstname");
        public NotNullColumn<DbText> Lastname => table.Column<DbText>("lastname");
    }

    private class User
    {
        public required string Username { get; init; }
        public required string Firstname { get; init; }
        public required string Lastname { get; init; }
    }

    private class UserMapping(IProjectionBuilder query, UsersTable user) : IReaderMapping<UsersTable, User>
    {
        public static IReaderMapping<User> CreateFrom(IProjectionBuilder query, UsersTable user) => new UserMapping(query, user);

        private readonly NotNullProjection<DbText> _username = query.Get(user.Username);
        private readonly NotNullProjection<DbText> _firstname = query.Get(user.Firstname);
        private readonly NotNullProjection<DbText> _lastname = query.Get(user.Lastname);

        public IProjectionField[] ObjectIdentity => [];

        public User Map(ResultReader rs)
        {
            return new User
            {
                Username = rs.Get(this._username),
                Firstname = rs.Get(this._firstname),
                Lastname = rs.Get(this._lastname)
            };
        }

    }


}