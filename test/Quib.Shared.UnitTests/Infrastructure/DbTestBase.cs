using System.Data.Common;
using Xunit.Abstractions;

namespace Quib.UnitTests.Infrastructure;

public abstract class DbTestBase(DatabaseFixture env, ITestOutputHelper log) : IAsyncLifetime
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
        _db.Logger = LogDatabaseQuery;

        await InitializeTestDatabase();
    }

    private void LogDatabaseQuery(string sql)
    {
        log.WriteLine(sql);
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

