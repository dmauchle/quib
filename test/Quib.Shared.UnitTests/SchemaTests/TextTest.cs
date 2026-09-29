using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class TextTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task ShortText_Insert_WithUtf8_Succeeds()
    {
        using (var install = new DbInstaller(DbConnection))
        {
            install.Table("short_text_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("content", ColumnType.ShortText().NotNull());
        }

        // Arrange: Test string with emoji for UTF-8 validation
        var testContent = "Hello 😀 World";

        // Act: Insert text with emoji
        var inserted = await DB.InsertIntoTable("short_text_test")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbText>("content"), testContent);
            })
            .Execute();

        // Assert: Verify insertion succeeded
        Assert.Equal(1, inserted);

        // Verify the data was stored and retrieved correctly
        var rs = await DB.SelectFromTable("short_text_test")
            .Project(x => x.Column<DbText>("content"))
            .FetchOne();

        Assert.Equal(testContent, rs.Result);
    }

    [Fact]
    public async Task MediumText_Insert_WithUtf8_Succeeds()
    {
        using (var install = new DbInstaller(DbConnection))
        {
            install.Table("medium_text_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("content", ColumnType.MediumText().NotNull());
        }

        // Arrange: Test string with emoji for UTF-8 validation
        var testContent = "Hello 😀 World";

        // Act: Insert text with emoji
        var inserted = await DB.InsertIntoTable("medium_text_test")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbText>("content"), testContent);
            })
            .Execute();

        // Assert: Verify insertion succeeded
        Assert.Equal(1, inserted);

        // Verify the data was stored and retrieved correctly
        var rs = await DB.SelectFromTable("medium_text_test")
            .Project(x => x.Column<DbText>("content"))
            .FetchOne();

        Assert.Equal(testContent, rs.Result);
    }

    [Fact]
    public async Task LongText_Insert_WithUtf8_Succeeds()
    {
        using (var install = new DbInstaller(DbConnection))
        {
            install.Table("long_text_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("content", ColumnType.LongText().NotNull());
        }

        // Arrange: Test string with emoji for UTF-8 validation
        var testContent = "Hello 😀 World";

        // Act: Insert text with emoji
        var inserted = await DB.InsertIntoTable("long_text_test")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbText>("content"), testContent);
            })
            .Execute();

        // Assert: Verify insertion succeeded
        Assert.Equal(1, inserted);

        // Verify the data was stored and retrieved correctly
        var rs = await DB.SelectFromTable("long_text_test")
            .Project(x => x.Column<DbText>("content"))
            .FetchOne();

        Assert.Equal(testContent, rs.Result);
    }

}
