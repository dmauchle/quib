using System.Data.Common;

#if MYSQL
namespace Quib.MySql.Installer;
#elif POSTGRES
namespace Quib.Postgres.Installer;
#endif

public sealed class DbInstaller : IDisposable
{

    private readonly QueryFactory _db;

    public DbInstaller(DbConnection con)
        : this(new QueryFactory(con))
    {

    }

    internal DbInstaller(QueryFactory db)
    {
        _db = db;
    }

    public TableInstaller Table(string tablename)
    {
        return new TableInstaller(_db, tablename);
    }

    public void Dispose()
    {

    }
}