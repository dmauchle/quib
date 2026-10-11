using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.UpdateTests;

[Collection(TestCollections.DATABASE)]
public class UpdateFilterTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private class UsersTable(DbTable table) : IDbTable, IDbTable<UsersTable>
    {

        public static string TableName => "users";
        public static UsersTable CreateFrom(DbTable table) => new UsersTable(table);

        public NotNullColumn<DbText> Name => field ??= table.Column<DbText>("name");
        public NotNullColumn<DbText> Lastname => field ??= table.Column<DbText>("lastname");
        public NullableColumn<DbText> City => field ??= table.NullableColumn<DbText>("city");
    }

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var installer = new DbInstaller(DB.Connection))
        {
            installer.Table("users")
                .CreateWithPrimaryKey("name", ColumnType.VarChar(64))
                .CreateColumn("lastname", ColumnType.VarChar(64).NotNull())
                .CreateColumn("city", ColumnType.VarChar(64).Nullable());
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
        var updated = await DB.Update<UsersTable>()
            .Where(x => x.Name == "Alice")
            .With((row, x) => row.Set(x.City, "Zurich"))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFrom<UsersTable>().Where(x => x.City == "Zurich").Project((rs, x) => rs.Get(x.Name)).FetchArray();
        Assert.Single(rs);
        Assert.Equal("Alice", rs[0]);
    }

    [Fact]
    public async Task DualFilter()
    {
        var updated = await DB.Update<UsersTable>()
            .Where(x => x.City == "New York")
            .Where(x => x.Lastname == "Jones")
            .With((row, x) => row.Set(x.Lastname, "Sinclair"))
            .Execute();

        var rs = await DB.SelectFrom<UsersTable>()
            .Where(x => x.Lastname == "Sinclair")
            .Project((rs, x) => rs.Get(x.Name)).FetchArray();
        Assert.Single(rs);
        Assert.Equal("Charlie", rs[0]);
    }

    [Fact]
    public async Task DynamicFilter()
    {
        var updated = await DB.UpdateTable("users")
            .Where(x => x.Column<DbText>("name") == "Alice")
            .With((row, x) => row.Set(x.Column<DbText>("city"), "Zurich"))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("users")
            .Where(x => x.Column<DbText>("city") == "Zurich")
            .Project((rs, x) => rs.Get(x.Column<DbText>("name")))
            .FetchArray();
        Assert.Single(rs);
        Assert.Equal("Alice", rs[0]);
    }

    [Fact]
    public async Task AllRows()
    {
        var updated = await DB.Update<UsersTable>()
            .AllRows()
            .With((row, x) => row.Set(x.City, "Zurich"))
            .Execute();

        Assert.Equal(3, updated);

        var rs = await DB.SelectFrom<UsersTable>().Project((rs, x) => rs.Get(x.City)).FetchArray();
        Assert.Equal(3, rs.Length);
        Assert.All(rs, x => Assert.Equal("Zurich", x));
    }

}