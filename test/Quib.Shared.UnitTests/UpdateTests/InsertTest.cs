using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.UpdateTests;

[Collection(TestCollections.DATABASE)]
public class InsertTest(DatabaseFixture env) : DbTestBase(env)
{

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users")
                .CreateWithPrimaryKey("username", ColumnType.VarChar(64))
                .AddColumn("firstname", ColumnType.VarChar(64).NotNull())
                .AddColumn("lastname", ColumnType.VarChar(64).NotNull());
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

        var users = await DB.SelectFrom<UsersTable>().Map(User.FromDbTable).FetchArray();

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
                query.Set(table.Username, query.Parameters.Create("asmith"));
                query.Set(table.Firstname, query.Parameters.Create("Alice"));
                query.Set(table.Lastname, query.Parameters.Create("Smith"));
            })
            .Execute();

        Assert.Equal(1, insert);

        var users = await DB.SelectFrom<UsersTable>().Map(User.FromDbTable).FetchArray();

        Assert.NotNull(users);
        Assert.Single(users);
        var user = users[0];
        Assert.Equal("asmith", user.Username);
        Assert.Equal("Alice", user.Firstname);
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