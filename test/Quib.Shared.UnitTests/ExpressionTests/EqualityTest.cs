using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class EqualityTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private class UsersTable(DbTable table) : IDbTable, IDbTable<UsersTable>
    {

        public static string TableName => "users";
        public static UsersTable CreateFrom(DbTable table) => new UsersTable(table);

        public NotNullColumn<DbText> Name => table.Column<DbText>("name");
        public NullableColumn<DbText> Email => table.NullableColumn<DbText>("email");
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
    public async Task SameTypeEquality()
    {
        var rs = await DB.SelectFrom<UsersTable>()
            .Where(x => x.Name == SQL.Parameter("Alice"))
            .Project(x => x.Name)
            .FetchArray();

        Assert.Single(rs);
        Assert.Equal("Alice", rs[0]);
    }

    //[Fact]
    //public async Task DifferentTypeEquality()
    //{
    //    var rs = await DB.SelectFrom<UsersTable>()
    //        .Where(x => x.Name == SQL.Parameter(23))  // Compiler error, because it's not the same column type! 
    //        .Project(x => x.Name)
    //        .FetchArray();

    //    Assert.Empty(rs);
    //}


}
