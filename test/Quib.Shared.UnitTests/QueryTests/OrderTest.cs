using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.QueryTests;

[Collection(TestCollections.DATABASE)]
public class OrderTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
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
                row.Set(user.Name, "Charlie");
                row.Set(user.Lastname, "Jones");
                row.Set(user.City, "New York");
            })
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Bob");
                row.Set(user.Lastname, "Jones");
                row.Set(user.City, "Amsterdam");
            })
            .Execute();
    }

    private void AssertCollection(string[] collection, params string[] expectedValues)
    {
        string error()
        {
            return $"Expected: {string.Join(", ", expectedValues)}\nActual: {string.Join(", ", collection)}";
        }

        Assert.True(expectedValues.Length == collection.Length, error());
        for (var i = 0; i < collection.Length; i++)
        {
            Assert.True(expectedValues[i] == collection[i], error());
        }
    }

    [Fact]
    public async Task SingleSort()
    {
        var rs = await DB.SelectFrom<UsersTable>().OrderBy(x => [x.Name]).Project((rs, x) => rs.Get(x.Name)).FetchArray();
        AssertCollection(rs, "Alice", "Bob", "Charlie");
    }

    [Fact]
    public async Task AscSort()
    {
        var rs = await DB.SelectFrom<UsersTable>().OrderBy(x => [SQL.Sort(x.Name, ASC)]).Project((rs, x) => rs.Get(x.Name)).FetchArray();
        AssertCollection(rs, "Alice", "Bob", "Charlie");
    }

    [Fact]
    public async Task DescSort()
    {
        var rs = await DB.SelectFrom<UsersTable>().OrderBy(x => [SQL.Sort(x.Name, DESC)]).Project((rs, x) => rs.Get(x.Name)).FetchArray();
        AssertCollection(rs, "Charlie", "Bob", "Alice");
    }

    [Fact]
    public async Task DualSort1()
    {
        var rs = await DB.SelectFrom<UsersTable>().OrderBy(x => [SQL.Sort(x.Lastname, ASC), SQL.Sort(x.City, ASC)]).Project((rs, x) => rs.Get(x.Name)).FetchArray();
        AssertCollection(rs, "Bob", "Charlie", "Alice");
    }

    [Fact]
    public async Task DualSort2()
    {
        var rs = await DB.SelectFrom<UsersTable>().OrderBy(x => [SQL.Sort(x.Lastname, ASC), SQL.Sort(x.City, DESC)]).Project((rs, x) => rs.Get(x.Name)).FetchArray();
        AssertCollection(rs, "Charlie", "Bob", "Alice");
    }


    [Fact]
    public async Task DualSort3()
    {
        var rs = await DB.SelectFrom<UsersTable>().OrderBy(x => [SQL.Sort(x.Lastname, DESC), SQL.Sort(x.City, ASC)]).Project((rs, x) => rs.Get(x.Name)).FetchArray();
        AssertCollection(rs, "Alice", "Bob", "Charlie");
    }

    [Fact]
    public async Task DualSort4()
    {
        var rs = await DB.SelectFrom<UsersTable>().OrderBy(x => [SQL.Sort(x.Lastname, DESC), SQL.Sort(x.City, DESC)]).Project((rs, x) => rs.Get(x.Name)).FetchArray();
        AssertCollection(rs, "Alice", "Charlie", "Bob");
    }

}