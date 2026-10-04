using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class NullCheckExpressionTests(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private class UsersTable(DbTable table) : IDbTable, IDbTable<UsersTable>
    {

        public static string TableName => "users";
        public static UsersTable CreateFrom(DbTable table) => new UsersTable(table);

        public DbColumn<DbText> Name => table.Column<DbText>("name");
        public DbColumn<DbTextNullable> Email => table.Column<DbTextNullable>("email");
    }

    protected override async ValueTask InitializeTestDatabase()
    {
        using (var installer = new DbInstaller(DB.Connection))
        {
            installer.Table("users")
                .CreateWithPrimaryKey("name", ColumnType.VarChar(64))
                .CreateColumn("email", ColumnType.VarChar(64).Nullable());
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

    // Comparing to a null literal is only prevented by nullable reference type checks (CS8625).
    // Without them, it compiles, but must fail when building the expression.
#nullable disable
    [Fact]
    public async Task EqualNull_WithoutNullableChecks_Throws()
    {
        // Null check in projection
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await DB.SelectFrom<UsersTable>()
                .Project((rs, user) => rs.Get(user.Email == null))
                .FetchArray();
        });

        // Null check in where clause
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await DB.SelectFrom<UsersTable>()
                .Where(user => user.Name == null)
                .Project(user => user.Name)
                .FetchArray();
        });

        // Reversed null check in where clause
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await DB.SelectFrom<UsersTable>()
                .Where(user => null != user.Name)
                .Project(user => user.Name)
                .FetchArray();
        });
    }
#nullable restore

}
