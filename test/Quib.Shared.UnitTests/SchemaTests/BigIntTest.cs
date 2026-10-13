using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class BigIntTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    // Values outside the 32-bit range, to make sure they're not truncated anywhere
    private static readonly long _large = 5_000_000_000L;
    private static readonly long _largeNegative = -7_000_000_000L;

    private void CreateValuesTable()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("values_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.BigInt().NotNull())
                .CreateColumn("value_nullable", ColumnType.BigInt().Nullable());
        }
    }

    private async Task InsertValues(params (int id, long value, long? valueNullable)[] rows)
    {
        foreach (var (id, value, valueNullable) in rows)
        {
            await DB.InsertIntoTable("values_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbLong>("value"), value);
                    query.Set(table.NullableColumn<DbLong>("value_nullable"), valueNullable);
                })
                .Execute();
        }
    }

    // Column type tests

    [Fact]
    public async Task BigInt_WithMinValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, long.MinValue, long.MinValue));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbLong>("value"))
            .FetchOne();
        var rsNullable = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbLong>("value_nullable"))
            .FetchOne();

        Assert.Equal(long.MinValue, rs.Result);
        Assert.Equal(long.MinValue, rsNullable.Result);
    }

    [Fact]
    public async Task BigInt_WithMaxValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, long.MaxValue, long.MaxValue));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbLong>("value"))
            .FetchOne();
        var rsNullable = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbLong>("value_nullable"))
            .FetchOne();

        Assert.Equal(long.MaxValue, rs.Result);
        Assert.Equal(long.MaxValue, rsNullable.Result);
    }

    [Fact]
    public async Task BigInt_WithZeroValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, 0));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbLong>("value"))
            .FetchOne();

        Assert.Equal(0L, rs.Result);
    }

    [Fact]
    public async Task BigInt_Where_ComparesLargeValues()
    {
        CreateValuesTable();
        await InsertValues((1, _large, null), (2, _large + 1, null), (3, _largeNegative, null));

        var rs = await DB.SelectFromTable("values_test")
            .Where(x => x.Column<DbLong>("value") > _large)
            .Project(x => x.Column<DbInt>("id"))
            .FetchArray();

        Assert.Equal([2], rs);
    }


    // Fetch and update tests
    // These tests verify the getters (ResultReader), mapping extensions (MappingExtensions)
    // and setters (UpdateQueryExtensions) for BIGINT columns.

    [Fact]
    public async Task FetchOne_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _large, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbLong>("value"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal(_large, rs.Result);
    }

    [Fact]
    public async Task FetchArray_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _large, null), (2, _largeNegative, null), (3, 0, null));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbLong>("value"))
            .FetchArray();

        Assert.Equal([_large, _largeNegative, 0], rs);
    }

    [Fact]
    public async Task FetchStream_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _large, null), (2, _largeNegative, null), (3, 0, null));

        var values = new List<long>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbLong>("value"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([_large, _largeNegative, 0], values);
    }


    [Fact]
    public async Task FetchOneNullable_WithValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, _large));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbLong>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal(_large, rs.Result);
    }

    [Fact]
    public async Task FetchOneNullable_WithNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbLong>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task FetchArray_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, _large), (2, 0, null), (3, 0, _largeNegative));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbLong>("value_nullable"))
            .FetchArray();

        Assert.Equal([_large, null, _largeNegative], rs);
    }

    [Fact]
    public async Task FetchStream_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, _large), (2, 0, null), (3, 0, _largeNegative));

        var values = new List<long?>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbLong>("value_nullable"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([_large, null, _largeNegative], values);
    }

    [Fact]
    public async Task GetNullable_OnNotNullProjection_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _large, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbLong>("value"))
            .FetchOne((rs, x) => rs.GetNullable(x));

        Assert.Equal(_large, rs.Result);
    }

    [Fact]
    public async Task Update_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _large, null));

        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.Column<DbLong>("value"), _largeNegative))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbLong>("value"))
            .FetchOne();

        Assert.Equal(_largeNegative, rs.Result);
    }

    [Fact]
    public async Task UpdateNullable_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, null));

        long? value = _large;
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.NullableColumn<DbLong>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbLong>("value_nullable"))
            .FetchOne();

        Assert.Equal(_large, rs.Result);
    }

    [Fact]
    public async Task UpdateNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, _large));

        long? value = null;
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.NullableColumn<DbLong>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbLong>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

}
