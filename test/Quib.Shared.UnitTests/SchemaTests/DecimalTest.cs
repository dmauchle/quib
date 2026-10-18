using System.Data.Common;
using System.Globalization;
using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class DecimalTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private void CreateNumbersTable(int precision, int scale)
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("numbers")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.Decimal(precision, scale).NotNull());
        }
    }

    private async Task<int> InsertNumber(decimal value)
    {
        return await DB.InsertIntoTable("numbers")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbDecimal>("value"), value);
            })
            .Execute();
    }

    private async Task<decimal> SelectNumber()
    {
        var rs = await DB.SelectFromTable("numbers")
            .Project(x => x.Column<DbDecimal>("value"))
            .FetchOne();
        return rs.Result;
    }


    // Column type tests

    [Theory]
    [InlineData(0, 0)]
    [InlineData(29, 0)]
    [InlineData(10, -1)]
    [InlineData(10, 11)]
    public void Decimal_WithInvalidPrecisionOrScale_Throws(int precision, int scale)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ColumnType.Decimal(precision, scale));
    }

    [Fact]
    public async Task Decimal_WithMaxPrecision_Succeeds()
    {
        CreateNumbersTable(28, 0);
        var value = 9999999999999999999999999999m;

        Assert.Equal(1, await InsertNumber(value));
        Assert.Equal(value, await SelectNumber());
    }

    [Fact]
    public async Task Decimal_WithMaxScale_Succeeds()
    {
        CreateNumbersTable(28, 28);
        var value = 0.1234567890123456789012345678m;

        Assert.Equal(1, await InsertNumber(value));
        Assert.Equal(value, await SelectNumber());
    }

    [Fact]
    public async Task Decimal_WithNegativeValue_Succeeds()
    {
        CreateNumbersTable(19, 4);
        var value = -123456789012345.6789m;

        Assert.Equal(1, await InsertNumber(value));
        Assert.Equal(value, await SelectNumber());
    }

    [Fact]
    public async Task Decimal_WithFewerDecimals_ReturnsColumnScale()
    {
        CreateNumbersTable(10, 2);

        await InsertNumber(1.5m);
        var result = await SelectNumber();

        // Equal by value, but the decimal carries the scale of the column
        Assert.Equal(1.5m, result);
        Assert.Equal("1.50", result.ToString(CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData("1.235", "1.24")]
    [InlineData("1.234", "1.23")]
    [InlineData("-1.235", "-1.24")]
    public async Task Decimal_WithTooManyDecimals_Rounds(string value, string expected)
    {
        CreateNumbersTable(10, 2);

        Assert.Equal(1, await InsertNumber(decimal.Parse(value, CultureInfo.InvariantCulture)));
        Assert.Equal(decimal.Parse(expected, CultureInfo.InvariantCulture), await SelectNumber());
    }

    [Theory]
    [InlineData("1000")]
    [InlineData("-1000")]
    public async Task Decimal_ExceedingPrecision_Throws(string value)
    {
        // DECIMAL(5,2) allows max. 999.99. MySQL raises an error in strict mode only (enabled in the test containers).
        CreateNumbersTable(5, 2);

        await Assert.ThrowsAnyAsync<DbException>(() => InsertNumber(decimal.Parse(value, CultureInfo.InvariantCulture)));
    }


    // Fetch and update tests
    // These tests verify the getters (ResultReader), mapping extensions (MappingExtensions)
    // and setters (UpdateQueryExtensions) for DECIMAL columns.

    private void CreateValuesTable()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("values_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.Decimal(19, 4).NotNull())
                .CreateColumn("value_nullable", ColumnType.Decimal(19, 4).Nullable());
        }
    }

    private async Task InsertValues(params (int id, decimal value, decimal? valueNullable)[] rows)
    {
        foreach (var (id, value, valueNullable) in rows)
        {
            await DB.InsertIntoTable("values_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbDecimal>("value"), value);
                    query.Set(table.NullableColumn<DbDecimal>("value_nullable"), valueNullable);
                })
                .Execute();
        }
    }

    [Fact]
    public async Task FetchOne_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 42.5m, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDecimal>("value"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal(42.5m, rs.Result);
    }

    [Fact]
    public async Task FetchArray_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 42.5m, null), (2, -7.25m, null), (3, 0m, null));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbDecimal>("value"))
            .FetchArray();

        Assert.Equal([42.5m, -7.25m, 0m], rs);
    }

    [Fact]
    public async Task FetchStream_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 42.5m, null), (2, -7.25m, null), (3, 0m, null));

        var values = new List<decimal>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbDecimal>("value"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([42.5m, -7.25m, 0m], values);
    }


    [Fact]
    public async Task FetchOneNullable_WithValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0m, 42.5m));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDecimal>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal(42.5m, rs.Result);
    }

    [Fact]
    public async Task FetchOneNullable_WithNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0m, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDecimal>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task FetchArray_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0m, 42.5m), (2, 0m, null), (3, 0m, -7.25m));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbDecimal>("value_nullable"))
            .FetchArray();

        Assert.Equal([42.5m, null, -7.25m], rs);
    }

    [Fact]
    public async Task FetchStream_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0m, 42.5m), (2, 0m, null), (3, 0m, -7.25m));

        var values = new List<decimal?>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbDecimal>("value_nullable"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([42.5m, null, -7.25m], values);
    }

    [Fact]
    public async Task Update_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 42.5m, null));

        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.Column<DbDecimal>("value"), -7.25m))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbDecimal>("value"))
            .FetchOne();

        Assert.Equal(-7.25m, rs.Result);
    }

    [Fact]
    public async Task UpdateNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0m, 42.5m));

        decimal? value = null;
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.NullableColumn<DbDecimal>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbDecimal>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

}
