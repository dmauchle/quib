using System.Data.Common;
using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class VarCharTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task VarChar_WithinLimit_Succeeds()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("products")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("name", ColumnType.VarChar(10).NotNull());
        }

        // Arrange: Create a string exactly at the limit (10 characters)
        var productName = "LimitedPro"; // exactly 10 characters

        // Act: Insert the product with name at the limit
        var inserted = await DB.InsertIntoTable("products")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbText>("name"), productName);
            })
            .Execute();

        // Assert: Verify insertion succeeded
        Assert.Equal(1, inserted);

        // Verify the data was stored correctly
        var rs = await DB.SelectFromTable("products")
            .Project(x => x.Column<DbText>("name"))
            .FetchOne();

        Assert.Equal(productName, rs.Result);
    }

    [Fact]
    public async Task VarChar_ExceedsLimit_ThrowsException()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("products")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("name", ColumnType.VarChar(10).NotNull());
        }

        // Arrange: Create a string exceeding the limit (11 characters for varchar(10))
        var productName = "ExceedsLimi"; // 11 characters, exceeds limit of 10

        // Act & Assert: Expect database exception when inserting oversized string
        await Assert.ThrowsAnyAsync<DbException>(async () =>
        {
            await DB.InsertIntoTable("products")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), 2);
                    query.Set(table.Column<DbText>("name"), productName);
                })
                .Execute();
        });
    }

    [Fact]
    public async Task VarChar_ExceedsRowSizeLimit_ThrowsException()
    {
        // MySQL has a 65,535-byte row size limit across all columns in a table.
        // VarChar columns larger than this limit should fail at table creation time.
        // PostgreSQL has no such limit and should allow the table creation to succeed.

#if MYSQL
        // Act & Assert: For MySQL, expect exception during table creation
        // A VarChar(16384) is above MySQL's 65,535-byte row size limit (bytes, not characters, we are using utf8 4 bytes)
        var ex = await Assert.ThrowsAnyAsync<DbException>(async () =>
        {
            using (var install = new DbInstaller(DbConnection))
            {
                install.Table("large_varchar_test")
                    .CreateWithPrimaryKey("id", ColumnType.Int())
                    .CreateColumn("large_text", ColumnType.VarChar(16384).NotNull());
            }
        });
        Assert.Contains("Column length too big", ex.Message);
#elif POSTGRES
        // For PostgreSQL, VarChar can exceed 65,535 characters without issue
        // (PostgreSQL's limit is 1GB per value and has no row-size limit like MySQL)
        using (var install = new DbInstaller(DbConnection))
        {
            install.Table("large_varchar_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("large_text", ColumnType.VarChar(70000).NotNull());
        }

        // Act: Create a string with some data
        var testString = "Test content";

        var inserted = await DB.InsertIntoTable("large_varchar_test")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbText>("large_text"), testString);
            })
            .Execute();

        // Assert: Verify insertion succeeded for PostgreSQL
        Assert.Equal(1, inserted);

        var rs = await DB.SelectFromTable("large_varchar_test")
            .Project(x => x.Column<DbText>("large_text"))
            .FetchOne();

        Assert.Equal(testString, rs.Result);
#else
        Assert.Fail("Test missing");
#endif

    }

    [Fact]
    public async Task VarChar_WithinRowSizeLimit_Succeeds()
    {
        // MySQL has a 65,535-byte row size limit across all columns in a table.
        // VarChar columns larger than this limit should fail at table creation time.
        // PostgreSQL has no such limit and should allow the table creation to succeed.

#if MYSQL
        // Act & Assert: For MySQL, expect exception during table creation
        // A VarChar(16383) is within MySQL's 65,535-byte row size limit (bytes, not characters, we are using utf8 4 bytes)
        // But the int primary key also needs 4 bytes, so we end up with 16382 maximum characters.

        using (var install = new DbInstaller(DbConnection))
        {
            install.Table("large_varchar_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("large_text", ColumnType.VarChar(16382).NotNull());
        }

        // Act: Create a string with some data
        var testString = "Test content";

        var inserted = await DB.InsertIntoTable("large_varchar_test")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbText>("large_text"), testString);
            })
            .Execute();

        // Assert: Verify insertion succeeded for PostgreSQL
        Assert.Equal(1, inserted);

        var rs = await DB.SelectFromTable("large_varchar_test")
            .Project(x => x.Column<DbText>("large_text"))
            .FetchOne();

        Assert.Equal(testString, rs.Result);

#elif POSTGRES
        // For PostgreSQL, VarChar can exceed 65,535 characters without issue
        // (PostgreSQL's limit is 1GB per value and has no row-size limit like MySQL)
        using (var install = new DbInstaller(DbConnection))
        {
            install.Table("large_varchar_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("large_text", ColumnType.VarChar(70000).NotNull());
        }

        // Act: Create a string with some data
        var testString = "Test content";

        var inserted = await DB.InsertIntoTable("large_varchar_test")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbText>("large_text"), testString);
            })
            .Execute();

        // Assert: Verify insertion succeeded for PostgreSQL
        Assert.Equal(1, inserted);

        var rs = await DB.SelectFromTable("large_varchar_test")
            .Project(x => x.Column<DbText>("large_text"))
            .FetchOne();

        Assert.Equal(testString, rs.Result);
#else
        Assert.Fail("Test missing");
#endif

    }


    // UTF-8 Compatibility Tests
    // These tests verify that the database properly handles UTF-8 encoded strings
    // with various character types and multi-byte sequences

    [Fact]
    public async Task VarChar_UTF8_SingleTypes_Succeeds()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("utf8_single_chars")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("text_value", ColumnType.VarChar(50).NotNull());
        }

        // Arrange: Test strings with different UTF-8 character types
        var testCases = new[]
        {
            ("id:1", "😀", "Emoji - grinning face"),                           // 1 char, 4 bytes in UTF-8
            ("id:2", "中", "CJK - Chinese character"),                        // 1 char, 3 bytes in UTF-8
            ("id:3", "Д", "Cyrillic - Cyrillic capital letter DE"),           // 1 char, 2 bytes in UTF-8
            ("id:4", "ب", "Arabic - Arabic letter Beh"),                        // 1 char, 2 bytes in UTF-8
            ("id:5", "ñ", "Extended Latin - n with tilde"),                   // 1 char, 2 bytes in UTF-8
            ("id:6", "ä", "German umlaut - a with diaeresis"),                // 1 char, 2 bytes in UTF-8
            ("id:7", "ö", "German umlaut - o with diaeresis"),                // 1 char, 2 bytes in UTF-8
            ("id:8", "ü", "German umlaut - u with diaeresis"),                // 1 char, 2 bytes in UTF-8
            ("id:9", "ß", "German eszett - sharp s"),                          // 1 char, 2 bytes in UTF-8
            ("id:10", "日本", "CJK - Japanese characters"),                     // 2 chars, 6 bytes in UTF-8
            ("id:11", "Привет", "Cyrillic - Russian 'Hello'"),                // 6 chars, 12 bytes in UTF-8
        };

        foreach (var (description, value, comment) in testCases)
        {
            var idStr = description.Split(':')[1];
            var id = int.Parse(idStr);

            // Act: Insert UTF-8 string
            var inserted = await DB.InsertIntoTable("utf8_single_chars")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbText>("text_value"), value);
                })
                .Execute();

            // Assert: Verify insertion succeeded
            Assert.Equal(1, inserted);

            // Verify the data was stored and retrieved correctly
            var rs = await DB.SelectFromTable("utf8_single_chars")
                .Where(x => x.Column<DbInt>("id") == id)
                .Project(x => x.Column<DbText>("text_value"))
                .FetchOne();

            Assert.Equal(value, rs.Result);
        }
    }

    [Fact]
    public async Task VarChar_UTF8_Emoji_WithinLimit_Succeeds()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("emoji_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("emoji_str", ColumnType.VarChar(10).NotNull());
        }

        // Arrange: Create emoji string exactly at character limit (10 emoji characters)
        // Each emoji is typically 1 character but takes 4 bytes in UTF-8 (multi-byte encoding)
        var emojiString = "😀😁😂😃😄😅😆😇😈😉"; // Exactly 10 emoji characters

        // Act: Insert emoji string at limit
        var inserted = await DB.InsertIntoTable("emoji_test")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbText>("emoji_str"), emojiString);
            })
            .Execute();

        // Assert: Verify insertion succeeded
        Assert.Equal(1, inserted);

        // Verify the emoji string was stored and retrieved correctly
        var rs = await DB.SelectFromTable("emoji_test")
            .Where(x => x.Column<DbInt>("id") == 1)
            .Project(x => x.Column<DbText>("emoji_str"))
            .FetchOne();

        Assert.Equal(emojiString, rs.Result);
        // Note: Character count may vary depending on UTF-8 handling (UTF-16 surrogate pairs in .NET)
        // The important assertion above is that the emoji string retrieved matches what was inserted
    }

    [Fact]
    public async Task VarChar_UTF8_Emoji_ExceedsLimit_ThrowsException()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("emoji_overflow_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("emoji_str", ColumnType.VarChar(10).NotNull());
        }

        // Arrange: Create emoji string exceeding the character limit (11 emoji for VARCHAR(10))
        var emojiString = "😀😁😂😃😄😅😆😇😈😉😊"; // 11 emoji characters, exceeds limit of 10

        // Act & Assert: Expect database exception when inserting oversized emoji string
        await Assert.ThrowsAnyAsync<DbException>(async () =>
        {
            await DB.InsertIntoTable("emoji_overflow_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), 1);
                    query.Set(table.Column<DbText>("emoji_str"), emojiString);
                })
                .Execute();
        });
    }

    [Fact]
    public async Task VarChar_UTF8_Length_Boundary_By_CharacterType()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("length_boundary_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("text_ascii_1", ColumnType.VarChar(1).Nullable())
                .CreateColumn("text_umlaut_1", ColumnType.VarChar(1).Nullable())
                .CreateColumn("text_cjk_1", ColumnType.VarChar(1).Nullable())
                .CreateColumn("text_emoji_1", ColumnType.VarChar(1).Nullable())
                .CreateColumn("text_cjk_3", ColumnType.VarChar(3).Nullable())
                .CreateColumn("text_emoji_5", ColumnType.VarChar(5).Nullable());
        }

        // Arrange: Test cases that expose whether database counts characters or bytes
        // These tests verify if the database properly handles multi-byte UTF-8 characters
        // at their exact boundaries
        var testCases = new[]
        {
            // (columnName, value, description)
            ("text_ascii_1", "A", "ASCII character in VARCHAR(1)"),                   // 1 char, 1 byte - baseline
            ("text_umlaut_1", "ä", "German umlaut in VARCHAR(1)"),                    // 1 char, 2 bytes - tests character counting
            ("text_cjk_1", "中", "Single CJK character in VARCHAR(1)"),               // 1 char, 3 bytes - tests character counting
            ("text_emoji_1", "😀", "Single emoji character in VARCHAR(1)"),          // 1 char, 4 bytes - critical test
            ("text_cjk_3", "日本語", "Three CJK characters in VARCHAR(3)"),           // 3 chars, 9 bytes
            ("text_emoji_5", "😀😁😂😃😄", "Five emoji in VARCHAR(5)"),             // 5 chars, 20 bytes
        };

        foreach (var (columnName, value, description) in testCases)
        {
            // Delete previous row to avoid primary key conflicts
            await DB.DeleteFromTable("length_boundary_test")
                .AllRows()
                .Execute();

            // Act: Insert the value
            var inserted = await DB.InsertIntoTable("length_boundary_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), 1);
                    query.Set(table.Column<DbText>(columnName), value);
                })
                .Execute();

            // Assert: Verify insertion succeeded
            Assert.Equal(1, inserted);

            // Verify the data was stored and retrieved correctly
            var rs = await DB.SelectFromTable("length_boundary_test")
                .Project(x => x.Column<DbText>(columnName))
                .FetchOne();

            Assert.Equal(value, rs.Result);
        }
    }

    [Fact]
    public async Task VarChar_UTF8_Length_Boundary_Exceeds_Throws_Exception()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("length_boundary_overflow_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("text_ascii_1", ColumnType.VarChar(1).NotNull())
                .CreateColumn("text_umlaut_1", ColumnType.VarChar(1).NotNull())
                .CreateColumn("text_cjk_1", ColumnType.VarChar(1).NotNull())
                .CreateColumn("text_emoji_1", ColumnType.VarChar(1).NotNull())
                .CreateColumn("text_cjk_3", ColumnType.VarChar(3).NotNull())
                .CreateColumn("text_emoji_5", ColumnType.VarChar(5).NotNull());
        }

        // Arrange: Test cases with one extra simple ASCII character exceeding the limit
        // Each should throw an exception because it exceeds the VARCHAR limit
        var testCases = new[]
        {
            // (columnName, value, description)
            ("text_ascii_1", "AA", "ASCII at limit + one more ASCII in VARCHAR(1)"),        // Already 1 char, +1 simple byte = exceeds
            ("text_umlaut_1", "äA", "Umlaut at limit + one simple ASCII in VARCHAR(1)"),   // Already 1 char, +1 simple byte = exceeds
            ("text_cjk_1", "中A", "CJK at limit + one simple ASCII in VARCHAR(1)"),        // Already 1 char, +1 simple byte = exceeds
            ("text_emoji_1", "😀A", "Emoji at limit + one simple ASCII in VARCHAR(1)"),    // Already 1 char, +1 simple byte = exceeds
            ("text_cjk_3", "中中中A", "Three CJK at limit + one simple ASCII in VARCHAR(3)"),  // Already 3 chars, +1 simple byte = exceeds
            ("text_emoji_5", "😀😀😀😀😀A", "Five emoji at limit + one simple ASCII in VARCHAR(5)"), // Already 5 chars, +1 simple byte = exceeds
        };

        foreach (var (columnName, value, description) in testCases)
        {
            // Delete previous row to avoid primary key conflicts
            await DB.DeleteFromTable("length_boundary_overflow_test")
                .Where(x => x.Column<DbInt>("id") == 1)
                .Execute();

            // Act & Assert: Expect database exception when inserting oversized value
            await Assert.ThrowsAnyAsync<DbException>(async () =>
            {
                await DB.InsertIntoTable("length_boundary_overflow_test")
                    .AddRow((query, table) =>
                    {
                        query.Set(table.Column<DbInt>("id"), 1);
                        query.Set(table.Column<DbText>(columnName), value);
                    })
                    .Execute();
            });
        }
    }

    [Fact]
    public async Task VarChar_UTF8_Combining_Marks_Succeeds()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("combining_marks_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("text_value", ColumnType.VarChar(50).NotNull());
        }

        // Arrange: Test strings with combining diacritical marks
        // These represent complex Unicode normalization scenarios
        var testCases = new[]
        {
            ("id:1", "e\u0301", "Latin small letter e with combining acute accent"), // é composed from e + combining acute
            ("id:2", "c\u0327", "Latin small letter c with combining cedilla"),      // ç composed from c + combining cedilla
            ("id:3", "A\u0308", "Latin capital letter A with combining diaeresis"),  // Ä composed from A + combining diaeresis
            ("id:4", "o\u0308", "Latin small letter o with combining diaeresis"),    // ö composed from o + combining diaeresis
            ("id:5", "n\u0303", "Latin small letter n with combining tilde"),        // ñ composed from n + combining tilde
        };

        foreach (var (description, value, comment) in testCases)
        {
            var idStr = description.Split(':')[1];
            var id = int.Parse(idStr);

            // Act: Insert combining marks string
            var inserted = await DB.InsertIntoTable("combining_marks_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbText>("text_value"), value);
                })
                .Execute();

            // Assert: Verify insertion succeeded
            Assert.Equal(1, inserted);

            // Verify the data was stored and retrieved correctly (byte-for-byte)
            var rs = await DB.SelectFromTable("combining_marks_test")
                .Where(x => x.Column<DbInt>("id") == id)
                .Project(x => x.Column<DbText>("text_value"))
                .FetchOne();

            Assert.Equal(value, rs.Result);
        }
    }

    [Fact]
    public async Task VarChar_UTF8_ByteVsCharacter_Boundary()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("byte_boundary_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("text_value", ColumnType.VarChar(30).NotNull());
        }

        // Arrange: Test cases that expose byte vs. character limit differences
        // This helps identify if the database counts characters or bytes
        var testCases = new[]
        {
            ("id:1", "ABC" , "ASCII: 3 chars, 3 bytes"),                          // 3 characters, 3 bytes
            ("id:2", "café", "ASCII + extended: 4 chars, 5 bytes"),              // 4 characters, 5 bytes (é is 2 bytes)
            ("id:3", "日本語", "CJK: 3 chars, 9 bytes"),                          // 3 characters, 9 bytes (each CJK char is 3 bytes)
            ("id:4", "😀😀😀", "Emoji: 3 chars, 12 bytes"),                       // 3 characters, 12 bytes (each emoji is 4 bytes)
            ("id:5", "ABC😀", "Mixed: 4 chars, 7 bytes"),                         // 4 characters, 7 bytes
            ("id:6", "Привет", "Cyrillic: 6 chars, 12 bytes"),                    // 6 characters, 12 bytes (each Cyrillic is 2 bytes)
        };

        foreach (var (description, value, comment) in testCases)
        {
            var idStr = description.Split(':')[1];
            var id = int.Parse(idStr);

            // Act: Insert boundary test string
            var inserted = await DB.InsertIntoTable("byte_boundary_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbText>("text_value"), value);
                })
                .Execute();

            // Assert: Verify insertion succeeded
            Assert.Equal(1, inserted);

            // Verify the data was stored and retrieved correctly
            var rs = await DB.SelectFromTable("byte_boundary_test")
                .Where(x => x.Column<DbInt>("id") == id)
                .Project(x => x.Column<DbText>("text_value"))
                .FetchOne();

            Assert.Equal(value, rs.Result);
            // Log the byte count to help diagnose byte vs. character semantics
            var byteCount = System.Text.Encoding.UTF8.GetByteCount(value);
            Assert.True(byteCount <= 120);
        }
    }

    [Fact]
    public async Task VarChar_UTF8_RightToLeft_Succeeds()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("rtl_text_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("text_value", ColumnType.VarChar(50).NotNull());
        }

        // Arrange: Test strings with right-to-left (RTL) languages
        var testCases = new[]
        {
            ("id:1", "مرحبا", "Arabic: Marhaba (Hello)"),                         // Arabic text
            ("id:2", "שלום", "Hebrew: Shalom (Hello)"),                          // Hebrew text
            ("id:3", "السلام عليكم", "Arabic: As-salamu alaykum (Peace be upon you)"), // Arabic phrase
            ("id:4", "עברית", "Hebrew: Ivrit (Hebrew language)"),                // Hebrew word
        };

        foreach (var (description, value, comment) in testCases)
        {
            var idStr = description.Split(':')[1];
            var id = int.Parse(idStr);

            // Act: Insert RTL text string
            var inserted = await DB.InsertIntoTable("rtl_text_test")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbInt>("id"), id);
                    query.Set(table.Column<DbText>("text_value"), value);
                })
                .Execute();

            // Assert: Verify insertion succeeded
            Assert.Equal(1, inserted);

            // Verify the data was stored and retrieved correctly
            // RTL text should be stored as-is without reordering by the database
            var rs = await DB.SelectFromTable("rtl_text_test")
                .Where(x => x.Column<DbInt>("id") == id)
                .Project(x => x.Column<DbText>("text_value"))
                .FetchOne();

            Assert.Equal(value, rs.Result);
        }
    }


    // Fetch and update tests
    // These tests verify the getters (ResultReader), mapping extensions (MappingExtensions)
    // and setters (UpdateQueryExtensions) for VARCHAR columns.

    private void CreateValuesTable()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("values_test")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("value", ColumnType.VarChar(50).NotNull())
                .CreateColumn("value_nullable", ColumnType.VarChar(50).Nullable());
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
                .CreateColumn("value", ColumnType.VarChar(50, TextCollation.CaseInsensitive).NotNull());
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
