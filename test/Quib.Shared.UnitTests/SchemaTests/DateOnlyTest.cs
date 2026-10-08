using Quib.UnitTests.Infrastructure;
using System.Data.Common;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class DateOnlyTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static readonly DateOnly _first = new(2025, 12, 31);
    private static readonly DateOnly _second = new(2026, 1, 1);
    private static readonly DateOnly _third = new(2026, 2, 28);

    private void CreateValuesTable()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("values_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.DateOnly().NotNull())
                .CreateColumn("value_nullable", ColumnType.DateOnly().Nullable());
        }
    }

    private async Task InsertValues(params (int id, DateOnly value, DateOnly? valueNullable)[] rows)
    {
        foreach (var (id, value, valueNullable) in rows)
        {
            await DB.InsertIntoTable("values_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbDateOnly>("value"), value);
                    query.Set(table.Column<DbDateOnlyNullable>("value_nullable"), valueNullable);
                })
                .Execute();
        }
    }

    private async Task InsertRaw(string sql)
    {
        using (var cmd = DbConnection.CreateCommand())
        {
            cmd.CommandText = sql;
            await cmd.ExecuteNonQueryAsync();
        }
    }

    // Column type tests

    [Fact]
    public async Task DateOnly_OrderBy_SortsChronologically()
    {
        CreateValuesTable();
        await InsertValues((1, _third, null), (2, _first, null), (3, _second, null));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbDateOnly>("value")])
            .Project(x => x.Column<DbDateOnly>("value"))
            .FetchArray();

        Assert.Equal([_first, _second, _third], rs);
    }

    [Fact]
    public async Task DateOnly_MaxValue_Works()
    {
        CreateValuesTable();
        await InsertValues((1, DateOnly.MaxValue, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateOnly>("value"))
            .FetchOne();

        Assert.Equal(DateOnly.MaxValue, rs.Result);
    }

    [Fact]
    public async Task DateOnly_MinValue_Throws()
    {
        CreateValuesTable();

        // Only years 1000 - 9999 are supported (the DBs would accept smaller values)
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => InsertValues((1, DateOnly.MinValue, null)));
    }

    [Fact]
    public async Task DateOnly_BeforeYear1000_Throws()
    {
        CreateValuesTable();

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => InsertValues((1, new DateOnly(999, 12, 31), null)));
    }

    [Fact]
    public async Task DateOnlyNullable_BeforeYear1000_Throws()
    {
        CreateValuesTable();

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => InsertValues((1, _first, new DateOnly(999, 12, 31))));
    }

    [Fact]
    public async Task DateOnly_Year1000_Works()
    {
        CreateValuesTable();
        await InsertValues((1, new DateOnly(1000, 1, 1), null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateOnly>("value"))
            .FetchOne();

        Assert.Equal(new DateOnly(1000, 1, 1), rs.Result);
    }

    [Fact]
    public async Task DateOnly_ReadBeforeYear1000_Works()
    {
        CreateValuesTable();
        // Values written by others (bypassing Quib) must still be readable
        await InsertRaw("INSERT INTO values_test (id, value, value_nullable) VALUES (1, '0950-01-01', '0950-01-01')");

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateOnly>("value"))
            .FetchOne();
        var rsNullable = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateOnlyNullable>("value_nullable"))
            .FetchOne();

        Assert.Equal(new DateOnly(950, 1, 1), rs.Result);
        Assert.Equal(new DateOnly(950, 1, 1), rsNullable.Result);
    }


    // Fetch and update tests
    // These tests verify the getters (ResultReader), mapping extensions (MappingExtensions)
    // and setters (UpdateQueryExtensions) for DbDateOnly and DbDateOnlyNullable on DATE columns.

    [Fact]
    public async Task FetchOne_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateOnly>("value"))
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
            .Project(x => x.Column<DbDateOnly>("value"))
            .FetchArray();

        Assert.Equal([_first, _second, _third], rs);
    }

    [Fact]
    public async Task FetchStream_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null), (2, _second, null), (3, _third, null));

        var values = new List<DateOnly>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbDateOnly>("value"))
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
            .Project(x => x.Column<DbDateOnlyNullable>("value_nullable"))
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
            .Project(x => x.Column<DbDateOnlyNullable>("value_nullable"))
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
            .Project(x => x.Column<DbDateOnlyNullable>("value_nullable"))
            .FetchArray();

        Assert.Equal([_first, null, _third], rs);
    }

    [Fact]
    public async Task FetchStream_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, _first), (2, _first, null), (3, _first, _third));

        var values = new List<DateOnly?>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbDateOnlyNullable>("value_nullable"))
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
            .With((row, x) => row.Set(x.Column<DbDateOnly>("value"), _second))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateOnly>("value"))
            .FetchOne();

        Assert.Equal(_second, rs.Result);
    }

    [Fact]
    public async Task UpdateNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, _second));

        DateOnly? value = null;
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.Column<DbDateOnlyNullable>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateOnlyNullable>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

}
