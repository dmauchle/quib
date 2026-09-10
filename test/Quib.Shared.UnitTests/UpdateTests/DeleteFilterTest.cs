using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.UpdateTests;

[Collection(TestCollections.DATABASE)]
public class DeleteFilterTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private class UsersTable(DbTable table) : IDbTable, IDbTableFactory<UsersTable>
    {

        public static string TableName => "users";
        public static UsersTable Factory(DbTable table) => new UsersTable(table);

        public DbColumn<DbText> Name => field ??= table.Column<DbText>("name");
        public DbColumn<DbText> Lastname => field ??= table.Column<DbText>("lastname");
        public DbColumn<DbTextNullable> City => field ??= table.Column<DbTextNullable>("city");
    }

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var installer = new DbInstaller(DB.Connection))
        {
            installer.Table("users")
                .CreateWithPrimaryKey("name", ColumnType.VarChar(64))
                .AddColumn("lastname", ColumnType.VarChar(64).NotNull())
                .AddColumn("city", ColumnType.VarChar(64).Nullable());
        }

        await DB.InsertInto<UsersTable>()
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Alice");
                row.Set(user.Lastname, "Smith");
                row.Set(user.City, "New York");
            })
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Bob");
                row.Set(user.Lastname, "Jones");
            })
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Charlie");
                row.Set(user.Lastname, "Jones");
                row.Set(user.City, "New York");
            })
            .Execute();
    }

    [Fact]
    public async Task SingleFilter()
    {
        var updated = await DB.DeleteFrom<UsersTable>()
            .Where(x => x.Name == "Alice")
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFrom<UsersTable>().Project((rs, x) => rs.Get(x.Name)).FetchArray();
        Assert.Equal(2, rs.Length);
        Assert.All(rs, x => Assert.NotEqual("Alice", x));
    }

    [Fact]
    public async Task DualFilter()
    {
        var updated = await DB.DeleteFrom<UsersTable>()
            .Where(x => x.City == "New York")
            .Where(x => x.Lastname == "Jones")
            .Execute();

        var rs = await DB.SelectFrom<UsersTable>().Project((rs, x) => rs.Get(x.Name)).FetchArray();
        Assert.Equal(2, rs.Length);
        Assert.All(rs, x => Assert.NotEqual("Charlie", x));
    }

    [Fact]
    public async Task DynamicFilter()
    {
        var updated = await DB.DeleteFromTable("users")
            .Where(x => x.Column<DbText>("name") == "Alice")
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFrom<UsersTable>().Project((rs, x) => rs.Get(x.Name)).FetchArray();
        Assert.Equal(2, rs.Length);
        Assert.All(rs, x => Assert.NotEqual("Alice", x));
    }

    [Fact]
    public async Task AllRows()
    {
        var updated = await DB.DeleteFrom<UsersTable>()
            .AllRows()
            .Execute();

        Assert.Equal(3, updated);

        var rs = await DB.SelectFrom<UsersTable>().Project((rs, x) => rs.Get(x.Name)).FetchArray();
        Assert.Empty(rs);
    }

}