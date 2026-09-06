using System.Data.Common;

namespace Quib.UnitTests.Infrastructure;

public abstract class DbTestBase(DatabaseFixture env) : IAsyncLifetime
{

    private DbConnection _con = null!;
    private QueryFactory _db = null!;

    protected QueryFactory DB => _db;
    protected DbConnection DbConnection => _con;

    public virtual async Task InitializeAsync()
    {
        await env.ResetDatabase();
        _con = await env.OpenConnection();
        _db = new QueryFactory(_con);

        await InitializeTestDatabase();
    }

    public virtual async Task DisposeAsync()
    {
        _con?.Dispose();
    }

    protected virtual ValueTask InitializeTestDatabase()
    {
        return ValueTask.CompletedTask;
    }

}

