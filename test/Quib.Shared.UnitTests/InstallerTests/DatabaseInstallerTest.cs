using Quib.UnitTests.Infrastructure;

namespace Quib.UnitTests.InstallerTests;

[Collection(TestCollections.DATABASE)]
public class DatabaseInstallerTest(DatabaseFixture env) : DbTestBase(env)
{

    /// <summary>
    /// DB will be set up by the base test. Just check if the connection is working and we can query the database.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task DatabaseAvailable()
    {
        using (var cmd = DbConnection.CreateCommand())
        {
            cmd.CommandText = $"SELECT 1";
            var rs = await cmd.ExecuteScalarAsync();
#if MYSQL
            Assert.Equal(1L, rs);
#elif POSTGRES
            Assert.Equal(1, rs);
#endif
        }
    }


}