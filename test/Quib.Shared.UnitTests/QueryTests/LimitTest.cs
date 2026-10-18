using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.QueryTests;

[Collection(TestCollections.DATABASE)]
public class LimitTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private class UsersTable(DbTable table) : IDbTable, IDbTable<UsersTable>
    {

        public static string TableName => "users";
        public static UsersTable CreateFrom(DbTable table) => new UsersTable(table);

        public NotNullColumn<DbText> Name => field ??= table.Column<DbText>("name");
        public NullableColumn<DbText> City => field ??= table.NullableColumn<DbText>("city");
    }

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var installer = new DbInstaller(DB.Connection))
        {
            installer.Table("users")
                .CreateWithPrimaryKey("name", ColumnType.VarChar(64))
                .CreateColumn("city", ColumnType.VarChar(64).Nullable());
        }

        await DB.InsertInto<UsersTable>()
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Alice");
                row.Set(user.City, "New York");
            })
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Bob");
                row.Set(user.City, "Amsterdam");
            })
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Charlie");
                row.Set(user.City, "New York");
            })
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Dave");
                row.Set(user.City, "New York");
            })
            .Execute();
    }

    private Task<string[]> FetchNames(Func<SelectQuery<UsersTable>, SelectQuery<UsersTable>> query)
    {
        var select = DB.SelectFrom<UsersTable>().OrderBy(x => [x.Name]);
        return query(select).Project((rs, x) => rs.Get(x.Name)).FetchArray();
    }

    [Fact]
    public async Task Limit_Works()
    {
        var rs = await FetchNames(q => q.Limit(2));
        Assert.Equal(["Alice", "Bob"], rs);
    }

    [Fact]
    public async Task Limit_WithOffset_Works()
    {
        var rs = await FetchNames(q => q.Limit(2, offset: 1));
        Assert.Equal(["Bob", "Charlie"], rs);
    }

    [Fact]
    public async Task Limit_WithOffsetZero_Works()
    {
        var rs = await FetchNames(q => q.Limit(2, offset: 0));
        Assert.Equal(["Alice", "Bob"], rs);
    }

    [Fact]
    public async Task Limit_Zero_ReturnsNoRows()
    {
        var rs = await FetchNames(q => q.Limit(0));
        Assert.Empty(rs);
    }

    [Fact]
    public async Task Limit_ExceedingRowCount_ReturnsAllRows()
    {
        var rs = await FetchNames(q => q.Limit(10));
        Assert.Equal(["Alice", "Bob", "Charlie", "Dave"], rs);
    }

    [Fact]
    public async Task Limit_OffsetExceedingRemainingRows_ReturnsRemainingRows()
    {
        var rs = await FetchNames(q => q.Limit(2, offset: 3));
        Assert.Equal(["Dave"], rs);
    }

    [Fact]
    public async Task Limit_OffsetExceedingRowCount_ReturnsNoRows()
    {
        var rs = await FetchNames(q => q.Limit(2, offset: 4));
        Assert.Empty(rs);
    }

    [Fact]
    public async Task Limit_OffsetExceedingIntRange_ReturnsNoRows()
    {
        var rs = await FetchNames(q => q.Limit(2, offset: int.MaxValue + 1L));
        Assert.Empty(rs);
    }

    [Fact]
    public async Task Limit_MaxValue_ReturnsAllRows()
    {
        var rs = await FetchNames(q => q.Limit(int.MaxValue));
        Assert.Equal(["Alice", "Bob", "Charlie", "Dave"], rs);
    }

    [Fact]
    public async Task Limit_WithFilter_LimitsFilteredRows()
    {
        var rs = await FetchNames(q => q.Where(x => x.City == "New York").Limit(1, offset: 1));
        Assert.Equal(["Charlie"], rs);
    }

    [Fact]
    public async Task Limit_CalledTwice_LastCallWins()
    {
        var rs = await FetchNames(q => q.Limit(1, offset: 3).Limit(2, offset: 1));
        Assert.Equal(["Bob", "Charlie"], rs);
    }

    [Fact]
    public async Task Limit_CalledTwiceWithoutOffset_ResetsOffset()
    {
        var rs = await FetchNames(q => q.Limit(1, offset: 3).Limit(2));
        Assert.Equal(["Alice", "Bob"], rs);
    }

    [Fact]
    public void Limit_NegativeLimit_ThrowsException()
    {
        var query = DB.SelectFrom<UsersTable>();
        Assert.Throws<ArgumentOutOfRangeException>("limit", () => query.Limit(-1));
    }

    [Fact]
    public void Limit_NegativeOffset_ThrowsException()
    {
        var query = DB.SelectFrom<UsersTable>();
        Assert.Throws<ArgumentOutOfRangeException>("offset", () => query.Limit(1, offset: -1));
    }

}
