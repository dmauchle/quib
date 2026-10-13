using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class DoubleTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    // Values that aren't exact in binary floating point, to make sure no precision is lost
    private static readonly double _value = 0.1 + 0.2;
    private static readonly double _negative = -1.234567890123456e-200;

    private void CreateValuesTable()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("values_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.Double().NotNull())
                .CreateColumn("value_nullable", ColumnType.Double().Nullable());
        }
    }

    private async Task InsertValues(params (int id, double value, double? valueNullable)[] rows)
    {
        foreach (var (id, value, valueNullable) in rows)
        {
            await DB.InsertIntoTable("values_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbDouble>("value"), value);
                    query.Set(table.NullableColumn<DbDouble>("value_nullable"), valueNullable);
                })
                .Execute();
        }
    }

    // Column type tests

    [Fact]
    public async Task Double_WithMinValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, double.MinValue, double.MinValue));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDouble>("value"))
            .FetchOne();
        var rsNullable = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDouble>("value_nullable"))
            .FetchOne();

        Assert.Equal(double.MinValue, rs.Result);
        Assert.Equal(double.MinValue, rsNullable.Result);
    }

    [Fact]
    public async Task Double_WithMaxValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, double.MaxValue, double.MaxValue));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDouble>("value"))
            .FetchOne();
        var rsNullable = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDouble>("value_nullable"))
            .FetchOne();

        Assert.Equal(double.MaxValue, rs.Result);
        Assert.Equal(double.MaxValue, rsNullable.Result);
    }

    [Fact]
    public async Task Double_WithZeroValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, 0));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDouble>("value"))
            .FetchOne();

        Assert.Equal(0d, rs.Result);
    }

    [Fact]
    public async Task Double_Where_ComparesValues()
    {
        CreateValuesTable();
        await InsertValues((1, _value, null), (2, _value + 1, null), (3, _negative, null));

        var rs = await DB.SelectFromTable("values_test")
            .Where(x => x.Column<DbDouble>("value") > _value)
            .Project(x => x.Column<DbInt>("id"))
            .FetchArray();

        Assert.Equal([2], rs);
    }

#if POSTGRES

    // Only Postgres can store NaN and infinity (MySQL can't, Quib rejects them when writing)

    private async Task InsertRaw(string sql)
    {
        using (var cmd = DbConnection.CreateCommand())
        {
            cmd.CommandText = sql;
            await cmd.ExecuteNonQueryAsync();
        }
    }

    [Fact]
    public async Task Double_ReadNaNAndInfinity_Works()
    {
        CreateValuesTable();
        // Values written by others (bypassing Quib) must still be readable
        await InsertRaw("""
            INSERT INTO values_test (id, value, value_nullable) VALUES
                (1, 'NaN', 'NaN'),
                (2, 'Infinity', 'Infinity'),
                (3, '-Infinity', '-Infinity')
            """);

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbDouble>("value"))
            .FetchArray();
        var rsNullable = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbDouble>("value_nullable"))
            .FetchArray();

        Assert.Equal([double.NaN, double.PositiveInfinity, double.NegativeInfinity], rs);
        Assert.Equal([double.NaN, double.PositiveInfinity, double.NegativeInfinity], rsNullable);
    }

#endif


    // Fetch and update tests
    // These tests verify the getters (ResultReader), mapping extensions (MappingExtensions)
    // and setters (UpdateQueryExtensions) for DOUBLE columns.

    [Fact]
    public async Task FetchOne_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _value, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDouble>("value"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal(_value, rs.Result);
    }

    [Fact]
    public async Task FetchArray_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _value, null), (2, _negative, null), (3, 0, null));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbDouble>("value"))
            .FetchArray();

        Assert.Equal([_value, _negative, 0], rs);
    }

    [Fact]
    public async Task FetchStream_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _value, null), (2, _negative, null), (3, 0, null));

        var values = new List<double>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbDouble>("value"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([_value, _negative, 0], values);
    }


    [Fact]
    public async Task FetchOneNullable_WithValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, _value));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDouble>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal(_value, rs.Result);
    }

    [Fact]
    public async Task FetchOneNullable_WithNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDouble>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task FetchArray_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, _value), (2, 0, null), (3, 0, _negative));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbDouble>("value_nullable"))
            .FetchArray();

        Assert.Equal([_value, null, _negative], rs);
    }

    [Fact]
    public async Task FetchStream_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, _value), (2, 0, null), (3, 0, _negative));

        var values = new List<double?>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbDouble>("value_nullable"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([_value, null, _negative], values);
    }

    [Fact]
    public async Task GetNullable_OnNotNullProjection_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _value, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDouble>("value"))
            .FetchOne((rs, x) => rs.GetNullable(x));

        Assert.Equal(_value, rs.Result);
    }

    [Fact]
    public async Task Update_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _value, null));

        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.Column<DbDouble>("value"), _negative))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDouble>("value"))
            .FetchOne();

        Assert.Equal(_negative, rs.Result);
    }

    [Fact]
    public async Task UpdateNullable_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, null));

        double? value = _value;
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.NullableColumn<DbDouble>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDouble>("value_nullable"))
            .FetchOne();

        Assert.Equal(_value, rs.Result);
    }

    [Fact]
    public async Task UpdateNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, _value));

        double? value = null;
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.NullableColumn<DbDouble>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDouble>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

}
