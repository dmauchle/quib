using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

// VarBinary is only available on MySQL (Postgres has no length-limited binary type, LongBinary is used there).
// The tests are skipped on Postgres.
[Collection(TestCollections.DATABASE)]
public class VarBinaryTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

#if POSTGRES
    private const string SKIP_REASON = "VarBinary is not available on Postgres.";
#else
    private static readonly byte[] _first = [0, 1, 2, 255];
    private static readonly byte[] _second = [0, 1, 2, 254];
    private static readonly byte[] _third = [];

    private async Task<byte[]> InsertAndRead(ColumnType columnType, byte[] value)
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("varbinary_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("content", columnType.NotNull());
        }

        await DB.InsertIntoTable("varbinary_test")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbBinary>("content"), value);
            })
            .Execute();

        var rs = await DB.SelectFromTable("varbinary_test")
            .Project(x => x.Column<DbBinary>("content"))
            .FetchOne();
        return rs.Result!;
    }

#endif

    // Column type tests

#if POSTGRES
    [Fact(Skip = "VarBinary is not available on Postgres")]
    public async Task VarBinary_WithMaxLength_Succeeds()
    {
    }
#else
    [Fact]

    public async Task VarBinary_WithMaxLength_Succeeds()
    {
        byte[] value = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        Assert.Equal(value, await InsertAndRead(ColumnType.VarBinary(10), value));
    }
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task VarBinary_ExceedingLength_Throws()
    {
    }
#else
    [Fact]
    public async Task VarBinary_ExceedingLength_Throws()
    {
        // The length is the number of bytes. MySQL raises an error in strict mode only (enabled in the test containers).
        byte[] value = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(() => InsertAndRead(ColumnType.VarBinary(10), value));
    }
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task VarBinary_WithAllByteValues_Succeeds()
    {
    }
#else
    [Fact]
    public async Task VarBinary_WithAllByteValues_Succeeds()
    {
        var value = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
        Assert.Equal(value, await InsertAndRead(ColumnType.VarBinary(256), value));
    }
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task VarBinary_WithTrailingZeros_NotTrimmed()
    {
    }
#else
    [Fact]
    public async Task VarBinary_WithTrailingZeros_NotTrimmed()
    {
        // Unlike BINARY(n), VARBINARY(n) neither pads nor trims values
        byte[] value = [1, 2, 0, 0];
        Assert.Equal(value, await InsertAndRead(ColumnType.VarBinary(10), value));
    }
#endif


    // Fetch and update tests
    // These tests verify the getters (ResultReader), mapping extensions (MappingExtensions)
    // and setters (UpdateQueryExtensions) for VARBINARY columns.

#if !POSTGRES
    private void CreateValuesTable()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("values_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.VarBinary(16).NotNull())
                .CreateColumn("value_nullable", ColumnType.VarBinary(16).Nullable());
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

#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task Where_Equal_Succeeds()
    {
    }
#else
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
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task FetchOne_Succeeds()
    {
    }
#else
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
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task FetchArray_Succeeds()
    {
    }
#else
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
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task FetchStream_Succeeds()
    {
    }
#else
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
#endif


#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task FetchOneNullable_WithValue_Succeeds()
    {
    }
#else
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
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task FetchOneNullable_WithNull_Succeeds()
    {
    }
#else
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
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task FetchArray_WithValueAndNull_Succeeds()
    {
    }
#else
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
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task FetchStream_WithValueAndNull_Succeeds()
    {
    }
#else
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
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task GetNullable_OnNotNullProjection_Succeeds()
    {
    }
#else
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
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task Update_Succeeds()
    {
    }
#else
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
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task UpdateNullable_Succeeds()
    {
    }
#else
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
#endif

#if POSTGRES
    [Fact(Skip = SKIP_REASON)]
    public async Task UpdateNull_Succeeds()
    {
    }
#else
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
#endif

}
