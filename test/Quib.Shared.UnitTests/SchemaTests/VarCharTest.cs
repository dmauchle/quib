using Quib.UnitTests.Infrastructure;
using System.Data.Common;
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

}
