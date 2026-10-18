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
    // and setters (UpdateQueryExtensions) for TEXT columns.
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
                    query.Set(table.NullableColumn<DbText>("value_nullable"), valueNullable);
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
            .Project(x => x.NullableColumn<DbText>("value_nullable"))
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
            .Project(x => x.NullableColumn<DbText>("value_nullable"))
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
            .Project(x => x.NullableColumn<DbText>("value_nullable"))
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
            .Project(x => x.NullableColumn<DbText>("value_nullable"))
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
            .With((row, x) => row.Set(x.NullableColumn<DbText>("value_nullable"), value))
            .Execute();

        Assert.Equal(1, updated);

        var rs = await DB.SelectFromTable("values_test")
            .Project(x => x.NullableColumn<DbText>("value_nullable"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    // Comparison and sorting (collation)
    // MySQL 8+ and Postgres: case- and accent-sensitive, linguistic sort order.
    // MySQL 5.x: case-sensitive by default (sorted by code point), case-insensitive by option (TextCollation).

    [Fact]
    public async Task Where_Equal_DifferentCase_NoMatch()
    {
        CreateValuesTable();
        await InsertValues((1, "hello", null), (2, "HELLO", null));

        var rs = await DB.SelectFromTable("values_test")
            .Where(x => x.Column<DbText>("value") == "HELLO")
            .Project(x => x.Column<DbInt>("id"))
            .FetchArray();

        Assert.Equal([2], rs);
    }

    [Fact]
    public async Task Where_Equal_DifferentCaseNonAscii_NoMatch()
    {
        CreateValuesTable();
        await InsertValues((1, "über", null), (2, "ÜBER", null));

        var rs = await DB.SelectFromTable("values_test")
            .Where(x => x.Column<DbText>("value") == "ÜBER")
            .Project(x => x.Column<DbInt>("id"))
            .FetchArray();

        Assert.Equal([2], rs);
    }

    [Fact]
    public async Task Where_Equal_DifferentAccent_NoMatch()
    {
        CreateValuesTable();
        await InsertValues((1, "uber", null), (2, "über", null));

        var rs = await DB.SelectFromTable("values_test")
            .Where(x => x.Column<DbText>("value") == "über")
            .Project(x => x.Column<DbInt>("id"))
            .FetchArray();

        Assert.Equal([2], rs);
    }

    [Fact]
    public async Task Where_EqualNullable_DifferentCase_NoMatch()
    {
        CreateValuesTable();
        await InsertValues((1, "a", "hello"), (2, "b", "HELLO"));

        var rs = await DB.SelectFromTable("values_test")
            .Where(x => x.NullableColumn<DbText>("value_nullable") == "HELLO")
            .Project(x => x.Column<DbInt>("id"))
            .FetchArray();

        Assert.Equal([2], rs);
    }

    [Fact]
    public async Task OrderBy_SortOrder()
    {
        CreateValuesTable();
        await InsertValues((1, "b", null), (2, "B", null), (3, "a", null), (4, "A", null), (5, "ü", null), (6, "z", null));

        var rs = await DB.SelectFromTable("values_test")
            .OrderBy(x => [x.Column<DbText>("value")])
            .Project(x => x.Column<DbText>("value"))
            .FetchArray();

#if MYSQL56 || MYSQL57
        // Sorted by code point (utf8mb4_bin)
        Assert.Equal(["A", "B", "a", "b", "z", "ü"], rs);
#else
        // Linguistic sort order
        Assert.Equal(["a", "A", "b", "B", "ü", "z"], rs);
#endif
    }

#if MYSQL56 || MYSQL57
    private void CreateCaseInsensitiveTable()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("ci_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.ShortText(TextCollation.CaseInsensitive).NotNull());
        }
    }

    private async Task InsertCaseInsensitive(params (int id, string value)[] rows)
    {
        foreach (var (id, value) in rows)
        {
            await DB.InsertIntoTable("ci_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbText>("value"), value);
                })
                .Execute();
        }
    }

    [Fact]
    public async Task CaseInsensitive_Where_Equal_DifferentCase_Matches()
    {
        CreateCaseInsensitiveTable();
        await InsertCaseInsensitive((1, "hello"), (2, "world"));

        var rs = await DB.SelectFromTable("ci_test")
            .Where(x => x.Column<DbText>("value") == "HELLO")
            .Project(x => x.Column<DbInt>("id"))
            .FetchArray();

        Assert.Equal([1], rs);
    }

    [Fact]
    public async Task CaseInsensitive_OrderBy_SortsLinguistically()
    {
        CreateCaseInsensitiveTable();
        await InsertCaseInsensitive((1, "b"), (2, "C"), (3, "a"), (4, "ü"), (5, "z"));

        var rs = await DB.SelectFromTable("ci_test")
            .OrderBy(x => [x.Column<DbText>("value")])
            .Project(x => x.Column<DbText>("value"))
            .FetchArray();

        Assert.Equal(["a", "b", "C", "ü", "z"], rs);
    }
#else
    [Fact(Skip = "TextCollation is only available on MySQL 5.x (MySQL 8+ and Postgres are always case-sensitive).")]
    public async Task CaseInsensitive_Where_Equal_DifferentCase_Matches()
    {
    }

    [Fact(Skip = "TextCollation is only available on MySQL 5.x (MySQL 8+ and Postgres are always case-sensitive).")]
    public async Task CaseInsensitive_OrderBy_SortsLinguistically()
    {
    }
#endif

}
