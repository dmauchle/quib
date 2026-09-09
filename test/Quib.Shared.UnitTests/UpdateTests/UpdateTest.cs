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
                .AddColumn("firstname", ColumnType.VarChar(64).NotNull())
                .AddColumn("lastname", ColumnType.VarChar(64).NotNull());

            await DB.InsertInto<UsersTable>()
               .AddRow((query, table) =>
               {
                   query.Set(table.Username, "asmith");
                   query.Set(table.Firstname, "Alice");
                   query.Set(table.Lastname, "Smith");
               })
               .Execute();
        }
    }


    [Fact]
    public async Task Update_WithStandardParameter_Works()
    {
        var updated = await DB.Update<UsersTable>()
            .With((row, table) =>
            {
                row.Set(table.Firstname, "Bob");
            })
            .Execute();

        Assert.Equal(1, updated);

        var users = await DB.SelectFrom<UsersTable>().Map(User.FromDbTable).FetchArray();

        Assert.Single(users);
        var user = users[0];
        Assert.Equal("Bob", user.Firstname);
    }

    [Fact]
    public async Task Update_WithExplicitParameter_Works()
    {
        var updated = await DB.Update<UsersTable>()
            .With((row, table) =>
            {
                row.Set(table.Firstname, "Bob");
            })
            .Execute();

        Assert.Equal(1, updated);

        var users = await DB.SelectFrom<UsersTable>().Map(User.FromDbTable).FetchArray();

        Assert.Single(users);
        var user = users[0];
        Assert.Equal("Bob", user.Firstname);
    }

    [Fact]
    public async Task Update_WithOtherColumn_Works()
    {
        var updated = await DB.Update<UsersTable>()
            .With((row, table) =>
            {
                row.Set(table.Firstname, table.Lastname);
            })
            .Execute();

        Assert.Equal(1, updated);

        var users = await DB.SelectFrom<UsersTable>().Map(User.FromDbTable).FetchArray();

        Assert.Single(users);
        var user = users[0];
        Assert.Equal("Smith", user.Firstname);
        Assert.Equal("Smith", user.Lastname);
    }


    private class UsersTable(DbTable table) : IDbTable, IDbTableFactory<UsersTable>
    {

        public static string TableName => "users";
        public static UsersTable Factory(DbTable table) => new UsersTable(table);

        public static UsersTable CreateQuery(DbTable table)
        {
            return new UsersTable(table);
        }

        public DbColumn<DbText> Username => table.Column<DbText>("username");
        public DbColumn<DbText> Firstname => table.Column<DbText>("firstname");
        public DbColumn<DbText> Lastname => table.Column<DbText>("lastname");
    }

    private class User
    {
        public required string Username { get; init; }
        public required string Firstname { get; init; }
        public required string Lastname { get; init; }

        public static IReaderMapping<UsersTable, User> FromDbTable(ProjectionBuilder query, UsersTable user) => new UserMapping(query, user);
    }

    private class UserMapping(ProjectionBuilder query, UsersTable user) : IReaderMappingFactory<UsersTable, User>
    {

        public static IReaderMapping<UsersTable, User> Factory(ProjectionBuilder query, UsersTable user) => new UserMapping(query, user);

        private readonly ProjectedValue<DbText> _username = query.Get(user.Username);
        private readonly ProjectedValue<DbText> _firstname = query.Get(user.Firstname);
        private readonly ProjectedValue<DbText> _lastname = query.Get(user.Lastname);

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