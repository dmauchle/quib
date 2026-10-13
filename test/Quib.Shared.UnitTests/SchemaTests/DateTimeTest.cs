using Quib.UnitTests.Infrastructure;
using System.Data.Common;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class DateTimeTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private static readonly DateTime _first = new(2026, 3, 14, 9, 26, 53);
    private static readonly DateTime _second = new(2026, 3, 14, 23, 59, 58);
    private static readonly DateTime _third = new(2026, 3, 15, 0, 0, 0);

    private void CreateValuesTable()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("values_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.DateTime().NotNull())
                .CreateColumn("value_nullable", ColumnType.DateTime().Nullable());
        }
    }

    private Task InsertValues(params (int id, DateTime value, DateTime? valueNullable)[] rows)
    {
        return InsertValues<LocalTime>(rows);
    }

    private async Task InsertValues<TTimeZone>(params (int id, DateTime value, DateTime? valueNullable)[] rows)
        where TTimeZone : IDateTimeStorage
    {
        foreach (var (id, value, valueNullable) in rows)
        {
            await DB.InsertIntoTable("values_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbDateTime<TTimeZone>>("value"), value);
                    query.Set(table.NullableColumn<DbDateTime<TTimeZone>>("value_nullable"), valueNullable);
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
    public async Task DateTime_OrderBy_SortsChronologically()
    {
        CreateValuesTable();
        await InsertValues((1, _third, null), (2, _first, null), (3, _second, null));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbDateTime<LocalTime>>("value")])
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchArray();

        Assert.Equal([_first, _second, _third], rs);
    }

    [Fact]
    public async Task DateTime_WithFractionalSeconds_Truncated()
    {
        CreateValuesTable();
        await InsertValues((1, _first.AddMilliseconds(999), null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchOne();

        Assert.Equal(_first, rs.Result);
    }

    [Fact]
    public async Task DateTime_Read_KindLocal()
    {
        CreateValuesTable();
        await InsertValues((1, _first, _second));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchOne();
        var rsNullable = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDateTime<LocalTime>>("value_nullable"))
            .FetchOne();

        Assert.Equal(DateTimeKind.Local, rs.Result.Kind);
        Assert.Equal(DateTimeKind.Local, rsNullable.Result!.Value.Kind);
    }

    [Fact]
    public async Task DateTime_Now_SameLocalTime()
    {
        CreateValuesTable();
        var now = DateTime.Now;
        await InsertValues((1, now, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchOne();

        Assert.Equal(now.AddTicks(-(now.Ticks % TimeSpan.TicksPerSecond)), rs.Result);
    }

    [Fact]
    public async Task DateTime_Utc_ConvertedToLocal()
    {
        CreateValuesTable();
        var utc = DateTime.SpecifyKind(_first, DateTimeKind.Utc);
        await InsertValues((1, utc, utc));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchOne();
        var rsNullable = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDateTime<LocalTime>>("value_nullable"))
            .FetchOne();

        // Same instant, independent of the time zone the tests run in
        Assert.Equal(utc.ToLocalTime(), rs.Result);
        Assert.Equal(utc, rs.Result.ToUniversalTime());
        Assert.Equal(utc, rsNullable.Result!.Value.ToUniversalTime());
    }

    [Fact]
    public async Task DateTime_UtcNow_SameInstant()
    {
        CreateValuesTable();
        var now = DateTime.UtcNow;
        await InsertValues((1, now, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchOne();

        Assert.Equal(now.AddTicks(-(now.Ticks % TimeSpan.TicksPerSecond)), rs.Result.ToUniversalTime());
    }

    [Fact]
    public async Task DateTime_Unspecified_StoredAsIs()
    {
        CreateValuesTable();
        await InsertValues((1, DateTime.SpecifyKind(_first, DateTimeKind.Unspecified), null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchOne();

        Assert.Equal(_first, rs.Result);
    }

    [Fact]
    public async Task DateTime_MaxValue_Works()
    {
        CreateValuesTable();
        await InsertValues((1, DateTime.MaxValue, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchOne();

        Assert.Equal(new DateTime(9999, 12, 31, 23, 59, 59), rs.Result);
    }

    [Fact]
    public async Task DateTime_MinValue_Throws()
    {
        CreateValuesTable();

        // Only years 1000 - 9999 are supported (the DBs would accept smaller values)
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => InsertValues((1, DateTime.MinValue, null)));
    }

    [Fact]
    public async Task DateTime_BeforeYear1000_Throws()
    {
        CreateValuesTable();

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => InsertValues((1, new DateTime(999, 12, 31, 23, 59, 59), null)));
    }

    [Fact]
    public async Task DateTimeNullable_BeforeYear1000_Throws()
    {
        CreateValuesTable();

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => InsertValues((1, _first, new DateTime(999, 12, 31, 23, 59, 59))));
    }

    [Fact]
    public async Task DateTime_Year1000_Works()
    {
        CreateValuesTable();
        await InsertValues((1, new DateTime(1000, 1, 1, 0, 0, 0), null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchOne();

        Assert.Equal(new DateTime(1000, 1, 1, 0, 0, 0), rs.Result);
    }

    [Fact]
    public async Task DateTime_ReadBeforeYear1000_Works()
    {
        CreateValuesTable();
        // Values written by others (bypassing Quib) must still be readable
        await InsertRaw("INSERT INTO values_test (id, value, value_nullable) VALUES (1, '0950-01-01 12:34:56', '0950-01-01 12:34:56')");

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchOne();
        var rsNullable = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDateTime<LocalTime>>("value_nullable"))
            .FetchOne();

        Assert.Equal(new DateTime(950, 1, 1, 12, 34, 56), rs.Result);
        Assert.Equal(new DateTime(950, 1, 1, 12, 34, 56), rsNullable.Result);
    }


    // Date-time storage (time zone) tests
    // The tests above use LocalTime (the same column can be used with either time zone).

    [Fact]
    public async Task Utc_Read_KindUtc()
    {
        CreateValuesTable();
        await InsertValues<Utc>((1, _first, _second));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<Utc>>("value"))
            .FetchOne();
        var rsNullable = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDateTime<Utc>>("value_nullable"))
            .FetchOne();

        Assert.Equal(_first, rs.Result);
        Assert.Equal(DateTimeKind.Utc, rs.Result.Kind);
        Assert.Equal(_second, rsNullable.Result);
        Assert.Equal(DateTimeKind.Utc, rsNullable.Result!.Value.Kind);
    }

    [Fact]
    public async Task Utc_UtcNow_SameInstant()
    {
        CreateValuesTable();
        var now = DateTime.UtcNow;
        await InsertValues<Utc>((1, now, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<Utc>>("value"))
            .FetchOne();

        Assert.Equal(now.AddTicks(-(now.Ticks % TimeSpan.TicksPerSecond)), rs.Result);
    }

    [Fact]
    public async Task Utc_Local_ConvertedToUtc()
    {
        CreateValuesTable();
        var local = DateTime.SpecifyKind(_first, DateTimeKind.Local);
        await InsertValues<Utc>((1, local, local));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<Utc>>("value"))
            .FetchOne();
        var rsNullable = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDateTime<Utc>>("value_nullable"))
            .FetchOne();

        // Same instant, independent of the time zone the tests run in
        Assert.Equal(local.ToUniversalTime(), rs.Result);
        Assert.Equal(local, rs.Result.ToLocalTime());
        Assert.Equal(local, rsNullable.Result!.Value.ToLocalTime());
    }

    [Fact]
    public async Task Utc_Unspecified_StoredAsIs()
    {
        CreateValuesTable();
        await InsertValues<Utc>((1, DateTime.SpecifyKind(_first, DateTimeKind.Unspecified), null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<Utc>>("value"))
            .FetchOne();

        Assert.Equal(_first, rs.Result);
    }

    [Fact]
    public async Task Utc_StoredAsUtc()
    {
        CreateValuesTable();
        var now = DateTime.Now;
        await InsertValues<Utc>((1, now, null));

        // Reading the stored value with the other time zone reveals the stored wall-clock time
        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchOne();

        var utc = now.ToUniversalTime();
        Assert.Equal(utc.AddTicks(-(utc.Ticks % TimeSpan.TicksPerSecond)), DateTime.SpecifyKind(rs.Result, DateTimeKind.Utc));
    }

    [Fact]
    public async Task Utc_Where_LocalValue_ConvertedToUtc()
    {
        CreateValuesTable();
        var utc = DateTime.SpecifyKind(_first, DateTimeKind.Utc);
        await InsertValues<Utc>((1, utc, null), (2, DateTime.SpecifyKind(_second, DateTimeKind.Utc), null));

        var rs = await DB.SelectFromTable("values_test")
            .Where(x => x.Column<DbDateTime<Utc>>("value") == utc.ToLocalTime())
            .Project(x => x.Column<DbInt>("id"))
            .FetchArray();

        Assert.Equal([1], rs);
    }


    // Fetch and update tests
    // These tests verify the getters (ResultReader), mapping extensions (MappingExtensions)
    // and setters (UpdateQueryExtensions) for DATETIME/timestamp columns.

    [Fact]
    public async Task FetchOne_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
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
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchArray();

        Assert.Equal([_first, _second, _third], rs);
    }

    [Fact]
    public async Task FetchStream_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, null), (2, _second, null), (3, _third, null));

        var values = new List<DateTime>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
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
            .Project(x => x.NullableColumn<DbDateTime<LocalTime>>("value_nullable"))
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
            .Project(x => x.NullableColumn<DbDateTime<LocalTime>>("value_nullable"))
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
            .Project(x => x.NullableColumn<DbDateTime<LocalTime>>("value_nullable"))
            .FetchArray();

        Assert.Equal([_first, null, _third], rs);
    }

    [Fact]
    public async Task FetchStream_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, _first), (2, _first, null), (3, _first, _third));

        var values = new List<DateTime?>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbDateTime<LocalTime>>("value_nullable"))
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
            .With((row, x) => row.Set(x.Column<DbDateTime<LocalTime>>("value"), _second))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<LocalTime>>("value"))
            .FetchOne();

        Assert.Equal(_second, rs.Result);
    }

    [Fact]
    public async Task UpdateNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, _first, _second));

        DateTime? value = null;
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.NullableColumn<DbDateTime<LocalTime>>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDateTime<LocalTime>>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Utc_FetchArray_Succeeds()
    {
        CreateValuesTable();
        await InsertValues<Utc>((1, _first, null), (2, _second, null), (3, _third, null));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbDateTime<Utc>>("value"))
            .FetchArray();

        Assert.Equal([_first, _second, _third], rs);
        Assert.All(rs, x => Assert.Equal(DateTimeKind.Utc, x.Kind));
    }

    [Fact]
    public async Task Utc_FetchStream_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues<Utc>((1, _first, _first), (2, _first, null), (3, _first, _third));

        var values = new List<DateTime?>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbDateTime<Utc>>("value_nullable"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([_first, null, _third], values);
        Assert.Equal(DateTimeKind.Utc, values[0]!.Value.Kind);
    }

    [Fact]
    public async Task Utc_FetchOneNullable_WithNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues<Utc>((1, _first, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDateTime<Utc>>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Utc_Update_Succeeds()
    {
        CreateValuesTable();
        await InsertValues<Utc>((1, _first, _first));

        var local = DateTime.SpecifyKind(_second, DateTimeKind.Local);
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) =>
            {
                row.Set(x.Column<DbDateTime<Utc>>("value"), local);
                row.Set(x.NullableColumn<DbDateTime<Utc>>("value_nullable"), (DateTime?)null);
            })
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDateTime<Utc>>("value"))
            .FetchOne();
        var rsNullable = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDateTime<Utc>>("value_nullable"))
            .FetchOne();

        Assert.Equal(local.ToUniversalTime(), rs.Result);
        Assert.Null(rsNullable.Result);
    }

}
