using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.SchemaTests;

[Collection(TestCollections.DATABASE)]
public class NullCheckExpressionTests(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private class UsersTable(DbTable table) : IDbTable, IDbTableFactory<UsersTable>
    {

        public static string TableName => "users";
        public static UsersTable Factory(DbTable table) => new UsersTable(table);

        public DbColumn<DbText> Name => table.Column<DbText>("name");
        public DbColumn<DbTextNullable> Email => table.Column<DbTextNullable>("email");
    }

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var installer = new DbInstaller(DB.Connection))
        {
            installer.Table("users")
                .CreateWithPrimaryKey("name", ColumnType.VarChar(64))
                .AddColumn("email", ColumnType.VarChar(64).Nullable());
        }

        await DB.InsertInto<UsersTable>()
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Alice");
                row.Set(user.Email, "alice@example.com");
            })
            .AddRow((row, user) =>
            {
                row.Set(user.Name, "Bob");
            })
            .Execute();
    }

    [Fact]
    public async Task IsNull()
    {
        var rs = await DB.SelectFrom<UsersTable>()
            .Project((rs, user) => new { Name = rs.Get(user.Name), HasEmail = rs.Get(user.Email.IsNull()) })
            .FetchArray((rs, user) => new { Name = rs.Get(user.Name), HasEmail = rs.Get(user.HasEmail) });

        Assert.Equal(2, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.HasEmail == false);
        Assert.Contains(rs, x => x.Name == "Bob" && x.HasEmail == true);
    }


    [Fact]
    public async Task IsNotNull()
    {
        var rs = await DB.SelectFrom<UsersTable>()
            .Project((rs, user) => new { Name = rs.Get(user.Name), HasEmail = rs.Get(user.Email.IsNotNull()) })
            .FetchArray((rs, user) => new { Name = rs.Get(user.Name), HasEmail = rs.Get(user.HasEmail) });

        Assert.Equal(2, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.HasEmail == true);
        Assert.Contains(rs, x => x.Name == "Bob" && x.HasEmail == false);
    }



}
