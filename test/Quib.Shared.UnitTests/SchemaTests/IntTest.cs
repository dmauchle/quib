using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Note: Unlike VARCHAR which can receive string values exceeding a length limit,
/// DbInt is a C# strongly-typed struct that prevents non-integer values at compile time.
/// Therefore, this test suite focuses on verifying successful insertion and retrieval
/// of boundary values (int.MinValue, int.MaxValue) rather than exception cases for
/// invalid types—the compiler already prevents those scenarios.
/// </remarks>
[Collection(TestCollections.DATABASE)]
public class IntTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{
    [Fact]
    public async Task Int_WithMinValue_Succeeds()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("numbers")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.Int().NotNull());
        }

        // Arrange: Use the minimum possible integer value
        var minValue = int.MinValue;

        // Act: Insert the row with minimum integer value
        var inserted = await DB.InsertIntoTable("numbers")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbInt>("value"), minValue);
            })
            .Execute();

        // Assert: Verify insertion succeeded
        Assert.Equal(1, inserted);

        // Verify the data was stored correctly
        var rs = await DB.SelectFromTable("numbers")
            .Project(x => x.Column<DbInt>("value"))
            .FetchOne();

        Assert.Equal(minValue, rs.Result);
    }

    [Fact]
    public async Task Int_WithMaxValue_Succeeds()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("numbers")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.Int().NotNull());
        }

        // Arrange: Use the maximum possible integer value
        var maxValue = int.MaxValue;

        // Act: Insert the row with maximum integer value
        var inserted = await DB.InsertIntoTable("numbers")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbInt>("value"), maxValue);
            })
            .Execute();

        // Assert: Verify insertion succeeded
        Assert.Equal(1, inserted);

        // Verify the data was stored correctly
        var rs = await DB.SelectFromTable("numbers")
            .Project(x => x.Column<DbInt>("value"))
            .FetchOne();

        Assert.Equal(maxValue, rs.Result);
    }

    [Fact]
    public async Task Int_WithZeroValue_Succeeds()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("numbers")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.Int().NotNull());
        }

        // Arrange: Use the default/zero value
        var zeroValue = 0;

        // Act: Insert the row with zero value
        var inserted = await DB.InsertIntoTable("numbers")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbInt>("value"), zeroValue);
            })
            .Execute();

        // Assert: Verify insertion succeeded
        Assert.Equal(1, inserted);

        // Verify the data was stored correctly
        var rs = await DB.SelectFromTable("numbers")
            .Project(x => x.Column<DbInt>("value"))
            .FetchOne();

        Assert.Equal(zeroValue, rs.Result);
    }


    // Fetch and update tests
    // These tests verify the getters (ResultReader), mapping extensions (MappingExtensions)
    // and setters (UpdateQueryExtensions) for INT columns.

    private void CreateValuesTable()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("values_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.Int().NotNull())
                .CreateColumn("value_nullable", ColumnType.Int().Nullable());
        }
    }

    private async Task InsertValues(params (int id, int value, int? valueNullable)[] rows)
    {
        foreach (var (id, value, valueNullable) in rows)
        {
            await DB.InsertIntoTable("values_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbInt>("value"), value);
                    query.Set(table.NullableColumn<DbInt>("value_nullable"), valueNullable);
                })
                .Execute();
        }
    }

    [Fact]
    public async Task FetchOne_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 42, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbInt>("value"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal(42, rs.Result);
    }

    [Fact]
    public async Task FetchArray_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 42, null), (2, -7, null), (3, 0, null));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbInt>("value"))
            .FetchArray();

        Assert.Equal([42, -7, 0], rs);
    }

    [Fact]
    public async Task FetchStream_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 42, null), (2, -7, null), (3, 0, null));

        var values = new List<int>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbInt>("value"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([42, -7, 0], values);
    }


    [Fact]
    public async Task FetchOneNullable_WithValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, 42));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbInt>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal(42, rs.Result);
    }

    [Fact]
    public async Task FetchOneNullable_WithNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbInt>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task FetchArray_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, 42), (2, 0, null), (3, 0, -7));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbInt>("value_nullable"))
            .FetchArray();

        Assert.Equal([42, null, -7], rs);
    }

    [Fact]
    public async Task FetchStream_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, 42), (2, 0, null), (3, 0, -7));

        var values = new List<int?>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.NullableColumn<DbInt>("value_nullable"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal([42, null, -7], values);
    }

    [Fact]
    public async Task Update_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 42, null));

        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.Column<DbInt>("value"), -7))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbInt>("value"))
            .FetchOne();

        Assert.Equal(-7, rs.Result);
    }

    [Fact]
    public async Task UpdateNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, 0, 42));

        int? value = null;
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.NullableColumn<DbInt>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbInt>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

}
