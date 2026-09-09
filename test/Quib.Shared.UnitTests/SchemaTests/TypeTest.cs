using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class TypeTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task Varchar_Value_Works()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("users").CreateWithPrimaryKey("name", ColumnType.VarChar(64));
        }

        var inserted = await DB.InsertIntoTable("users")
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

}