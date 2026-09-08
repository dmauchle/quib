using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class NullableColumnsTest(DatabaseFixture env) : DbTestBase(env)
{

    [Fact]
    public async Task Varchar_Value_Works()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users").CreateWithPrimaryKey("name", ColumnType.VarChar(64));
        }

        var inserted = await DB.InsertInto("users")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Alice");
            })
            .Execute();

        Assert.Equal(1, inserted);

        var rs = await DB.SelectFromTable("users")
            .Project(x => x.Column<DbText>("name"))
            .FetchOne();

        Assert.Equal("Alice", rs.Result);
    }

    [Fact]
    public async Task NullableColumn_WithNullValue_Works()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users")
                .CreateWithPrimaryKey("username", ColumnType.VarChar(64))
                .AddColumn("nickname", ColumnType.VarChar(64).Nullable());
        }

        var inserted = await DB.InsertInto("users")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("username"), "Jack");
                query.SetNull(table.Column<DbTextNullable>("nickname"));
            })
            .Execute();

        Assert.Equal(1, inserted);

        var rs = await DB.SelectFromTable("users")
            .Project(x => x.Column<DbTextNullable>("nickname"))
            .FetchOne();

        Assert.True(rs.HasResult);
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotNullColumn_WithNullValue_ThrowsException()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users")
                .CreateWithPrimaryKey("username", ColumnType.VarChar(64))
                .AddColumn("nickname", ColumnType.VarChar(64).NotNull());
        }

        await Assert.ThrowsAnyAsync<System.Data.Common.DbException>(async () =>
        {
            await DB.InsertInto("users")
                .AddRow((query, table) =>
                {
                    query.Set(table.Column<DbText>("username"), "Jack");
                    query.SetNull(table.Column<DbTextNullable>("nickname"));
                })
                .Execute();
        });
    }

    [Fact]
    public async Task NullableColumn_WithValue_Works()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users")
                .CreateWithPrimaryKey("username", ColumnType.VarChar(64))
                .AddColumn("nickname", ColumnType.VarChar(64).Nullable());
        }

        var inserted = await DB.InsertInto("users")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("username"), "Jack");
                query.Set(table.Column<DbTextNullable>("nickname"), "Fish");
            })
            .Execute();

        Assert.Equal(1, inserted);

        var rs = await DB.SelectFromTable("users")
            .Project(x => x.Column<DbTextNullable>("nickname"))
            .FetchOne();

        Assert.Equal("Fish", rs.Result);
    }

}