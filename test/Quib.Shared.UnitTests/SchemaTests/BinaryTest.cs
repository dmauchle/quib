using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class BinaryTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static readonly byte[] _first = [0, 1, 2, 255];
    private static readonly byte[] _second = [0, 1, 2, 254];
    private static readonly byte[] _third = [];

    private static byte[] AllByteValues()
    {
        return Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
    }

    private static byte[] LargeValue(int size)
    {
        var value = new byte[size];
        new Random(42).NextBytes(value);
        return value;
    }

    private async Task<byte[]> InsertAndRead(ColumnType columnType, byte[] value)
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("binary_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("content", columnType.NotNull());
        }

        await DB.InsertIntoTable("binary_test")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbBinary>("content"), value);
            })
            .Execute();

        var rs = await DB.SelectFromTable("binary_test")
            .Project(x => x.Column<DbBinary>("content"))
            .FetchOne();
        return rs.Result!;
    }

    // Column type tests

    [Fact]
    public async Task ShortBinary_WithAllByteValues_Succeeds()
    {
        var value = AllByteValues();
        Assert.Equal(value, await InsertAndRead(ColumnType.ShortBinary(), value));
    }

    [Fact]
    public async Task MediumBinary_WithAllByteValues_Succeeds()
    {
        var value = AllByteValues();
        Assert.Equal(value, await InsertAndRead(ColumnType.MediumBinary(), value));
    }

    [Fact]
    public async Task LongBinary_WithAllByteValues_Succeeds()
    {
        var value = AllByteValues();
        Assert.Equal(value, await InsertAndRead(ColumnType.LongBinary(), value));
    }

    [Fact]
    public async Task ShortBinary_WithEmptyValue_Succeeds()
    {
        Assert.Empty(await InsertAndRead(ColumnType.ShortBinary(), []));
    }

    [Fact]
    public async Task MediumBinary_WithLargeValue_Succeeds()
    {
        // Larger than ShortBinary (64 KB on MySQL)
        var value = LargeValue(100_000);
        Assert.Equal(value, await InsertAndRead(ColumnType.MediumBinary(), value));
    }

    [Fact]
    public async Task LongBinary_WithLargeValue_Succeeds()
    {
        var value = LargeValue(1_000_000);
        Assert.Equal(value, await InsertAndRead(ColumnType.LongBinary(), value));
    }

    [Fact]
    public async Task ShortBinary_ExceedingSize()
    {
        var value = LargeValue(70_000);
#if MYSQL
        // BLOB is limited to 65,535 bytes. MySQL raises an error in strict mode only (enabled in the test containers).
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() => InsertAndRead(ColumnType.ShortBinary(), value));
#elif POSTGRES
        // ShortBinary is an alias for LongBinary on Postgres, so the size isn't limited (except by the 1 GB value limit)
        Assert.Equal(value, await InsertAndRead(ColumnType.ShortBinary(), value));
#endif
    }


    // Fetch and update tests
    // These tests verify the getters (ResultReader), mapping extensions (MappingExtensions)
    // and setters (UpdateQueryExtensions) for BLOB/BYTEA columns.
    // Only ShortBinary is tested, the other binary column types use the same DB types.

    private void CreateValuesTable()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("values_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.ShortBinary().NotNull())
                .CreateColumn("value_nullable", ColumnType.ShortBinary().Nullable());
        }
    }

    private async Task InsertValues(params (int id, byte[] value, byte[]? valueNullable)[] rows)
    {
        foreach (var (id, value, valueNullable) in rows)
        {
            await DB.InsertIntoTable("values_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbBinary>("value"), value);
                    query.Set(table.NullableColumn<DbBinary>("value_nullable"), valueNullable);
                })
                .Execute();
        }
    }

    [Fact]
    public async Task Where_Equal_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null), (2, _second, null), (3, _third, null));

        var rs = await DB.SelectFromTable("values_test")
            .Where(x => x.Column<DbBinary>("value") == _second)
            .Project(x => x.Column<DbInt>("id"))
            .FetchArray();

        Assert.Equal([2], rs);
    }

    [Fact]
    public async Task FetchOne_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbBinary>("value"))
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
            .Project(x => x.Column<DbBinary>("value"))
            .FetchArray();

        Assert.Equal([_first, _second, _third], rs);
    }

    [Fact]
    public async Task FetchStream_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null), (2, _second, null), (3, _third, null));

        var values = new List<byte[]>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbBinary>("value"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([_first, _second, _third], values);
    }


    [Fact]
    public async Task FetchOneNullable_WithValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _third, _first));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbBinary>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal(_first, rs.Result);
    }

    [Fact]
    public async Task FetchOneNullable_WithNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _third, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbBinary>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task FetchArray_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _third, _first), (2, _third, null), (3, _third, _second));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbBinary>("value_nullable"))
            .FetchArray();

        Assert.Equal([_first, null, _second], rs);
    }

    [Fact]
    public async Task FetchStream_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _third, _first), (2, _third, null), (3, _third, _second));

        var values = new List<byte[]?>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbBinary>("value_nullable"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([_first, null, _second], values);
    }

    [Fact]
    public async Task GetNullable_OnNotNullProjection_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbBinary>("value"))
            .FetchOne((rs, x) => rs.GetNullable(x));

        Assert.Equal(_first, rs.Result);
    }

    [Fact]
    public async Task Update_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null));

        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.Column<DbBinary>("value"), _second))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbBinary>("value"))
            .FetchOne();

        Assert.Equal(_second, rs.Result);
    }

    [Fact]
    public async Task UpdateNullable_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null));

        var value = _second;
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.NullableColumn<DbBinary>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbBinary>("value_nullable"))
            .FetchOne();

        Assert.Equal(_second, rs.Result);
    }

    [Fact]
    public async Task UpdateNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, _second));

        byte[]? value = null;
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.NullableColumn<DbBinary>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbBinary>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

}
