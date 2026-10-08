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


    // Fetch and update tests
    // These tests verify the getters (ResultReader), mapping extensions (MappingExtensions)
    // and setters (UpdateQueryExtensions) for DbText and DbTextNullable on TEXT columns.
    // Only ShortText is tested, the other text column types use the same DB types.

    private void CreateValuesTable()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("values_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.ShortText().NotNull())
                .CreateColumn("value_nullable", ColumnType.ShortText().Nullable());
        }
    }

    private async Task InsertValues(params (int id, string value, string? valueNullable)[] rows)
    {
        foreach (var (id, value, valueNullable) in rows)
        {
            await DB.InsertIntoTable("values_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbText>("value"), value);
                    query.Set(table.Column<DbTextNullable>("value_nullable"), valueNullable);
                })
                .Execute();
        }
    }

    [Fact]
    public async Task FetchOne_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, "Alice 😀", null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbText>("value"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal("Alice 😀", rs.Result);
    }

    [Fact]
    public async Task FetchArray_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, "Alice", null), (2, "", null), (3, "Bob 😀", null));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbText>("value"))
            .FetchArray();

        Assert.Equal(["Alice", "", "Bob 😀"], rs);
    }

    [Fact]
    public async Task FetchStream_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, "Alice", null), (2, "", null), (3, "Bob 😀", null));

        var values = new List<string>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbText>("value"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal(["Alice", "", "Bob 😀"], values);
    }


    [Fact]
    public async Task FetchOneNullable_WithValue_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, "", "Alice 😀"));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbTextNullable>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Equal("Alice 😀", rs.Result);
    }

    [Fact]
    public async Task FetchOneNullable_WithNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, "", null));

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbTextNullable>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task FetchArray_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, "", "Alice"), (2, "", null), (3, "", ""));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbTextNullable>("value_nullable"))
            .FetchArray();

        // Empty string and null must be distinguished
        Assert.Equal(new List<string?> { "Alice", null, "" }, rs);
    }

    [Fact]
    public async Task FetchStream_WithValueAndNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, "", "Alice"), (2, "", null), (3, "", ""));

        var values = new List<string?>();
        var rs = DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbInt>("id")])
            .Project(x => x.Column<DbTextNullable>("value_nullable"))
            .FetchStream();
        await foreach (var value in rs)
            values.Add(value);

        Assert.Equal(["Alice", null, ""], values);
    }

    [Fact]
    public async Task Update_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, "Alice", null));

        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.Column<DbText>("value"), "Bob 😀"))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbText>("value"))
            .FetchOne();

        Assert.Equal("Bob 😀", rs.Result);
    }

    [Fact]
    public async Task UpdateNull_Succeeds()
    {
        CreateValuesTable();
        await InsertValues((1, "", "Alice"));

        string? value = null;
        var updated = await DB.UpdateTable("values_test")
            .AllRows()
            .With((row, x) => row.Set(x.Column<DbTextNullable>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.Column<DbTextNullable>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

}
