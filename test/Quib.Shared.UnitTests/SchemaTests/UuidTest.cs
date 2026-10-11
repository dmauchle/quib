using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class UuidTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static readonly Guid _first = Guid.Parse("0192f3a1-5c2e-7a00-8000-000000000001");
    private static readonly Guid _second = Guid.Parse("0192f3a1-5c2e-7a00-8000-000000000002");
    private static readonly Guid _third = Guid.Parse("0192f3a1-5c2e-7a00-8000-000000000003");

    private void CreateValuesTable()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("values_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.Uuid().NotNull())
                .CreateColumn("value_nullable", ColumnType.Uuid().Nullable());
        }
    }

    private async Task InsertValues(params (int id, Guid value, Guid? valueNullable)[] rows)
    {
        foreach (var (id, value, valueNullable) in rows)
        {
            await DB.InsertIntoTable("values_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbUuid>("value"), value);
                    query.Set(table.NullableColumn<DbUuid>("value_nullable"), valueNullable);
                })
                .Execute();
        }
    }

    // Column type tests

    [Fact]
    public async Task Uuid_AsPrimaryKey_Works()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users")
                .CreateWithPrimaryKey("id", ColumnType.Uuid())
                .CreateColumn("name", ColumnType.VarChar(32).NotNull());
        }

        foreach (var (id, name) in new[] { (_first, "Alice"), (_second, "Bob") })
        {
            await DB.InsertIntoTable("users")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbUuid>("id"), id);
                    query.Set(table.Column<DbText>("name"), name);
                })
                .Execute();
        }

        var rs = await DB.SelectFromTable("users")
            .Where(x => x.Column<DbUuid>("id") == _second)
            .Project(x => x.Column<DbText>("name"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal("Bob", rs.Result);
    }

    [Fact]
    public async Task Uuid_OrderByVersion7_SortsByTimestamp()
    {
        CreateValuesTable();
        var start = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var expected = Enumerable.Range(0, 5).Select(i => Guid.CreateVersion7(start.AddSeconds(i))).ToArray();
        // Insert in reverse order, so the result doesn't depend on insertion order
        for (var i = expected.Length - 1; i >= 0; i--)
            await InsertValues((i, expected[i], null));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbUuid>("value")])
            .Project(x => x.Column<DbUuid>("value"))
            .FetchArray();

        Assert.Equal(expected, rs);
    }

#if MYSQL

    [Fact]
    public async Task Uuid_StoredAsLowercaseText()
    {
        CreateValuesTable();
        await InsertValues((1, Guid.Parse("0192F3A1-5C2E-7A00-8000-0000000000AB"), null));

        // MySql.Data reads CHAR(36) as Guid, so compare the stored text in SQL (ascii_bin is case-sensitive)
        var rs = await DB.SelectFromTable("values_test")
            .Where(x => x.Column<DbText>("value") == "0192f3a1-5c2e-7a00-8000-0000000000ab")
            .Project(x => x.Column<DbInt>("id"))
            .FetchOne();

        Assert.True(rs.HasResult);
    }

#endif

    // Fetch and update tests
    // These tests verify the getters (ResultReader), mapping extensions (MappingExtensions)
    // and setters (UpdateQueryExtensions) for UUID columns.

    [Fact]
    public async Task FetchOne_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbUuid>("value"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal(_first, rs.Result);
    }

    [Fact]
    public async Task FetchArray_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null), (2, _second, null), (3, _third, null));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbUuid>("value"))
            .FetchArray();

        Assert.Equal([_first, _second, _third], rs);
    }

    [Fact]
    public async Task FetchStream_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null), (2, _second, null), (3, _third, null));

        var values = new List<Guid>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbUuid>("value"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([_first, _second, _third], values);
    }


    [Fact]
    public async Task FetchOneNullable_WithValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, _second));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbUuid>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal(_second, rs.Result);
    }

    [Fact]
    public async Task FetchOneNullable_WithNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbUuid>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task FetchArray_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, _first), (2, _first, null), (3, _first, _third));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbUuid>("value_nullable"))
            .FetchArray();

        Assert.Equal([_first, null, _third], rs);
    }

    [Fact]
    public async Task FetchStream_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, _first), (2, _first, null), (3, _first, _third));

        var values = new List<Guid?>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbUuid>("value_nullable"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([_first, null, _third], values);
    }

    [Fact]
    public async Task Update_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null));

        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.Column<DbUuid>("value"), _second))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbUuid>("value"))
            .FetchOne();

        Assert.Equal(_second, rs.Result);
    }

    [Fact]
    public async Task UpdateNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, _second));

        Guid? value = null;
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.NullableColumn<DbUuid>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbUuid>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

}
