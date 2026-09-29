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
}
