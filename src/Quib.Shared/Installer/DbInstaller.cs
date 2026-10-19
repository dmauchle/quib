using System.Data.Common;

#if MYSQL56
namespace Quib.MySql56.Installer;
#elif MYSQL57
namespace Quib.MySql57.Installer;
#elif MYSQL80
namespace Quib.MySql80.Installer;
#elif MYSQL84
namespace Quib.MySql84.Installer;
#elif POSTGRES18
namespace Quib.Postgres18.Installer;
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
        ArgumentNullException.ThrowIfNull(tablename);

        return new TableInstaller(_db, tablename);
    }

#if POSTGRES
    /// <summary>
    /// Creates the database objects required for <see cref="TextMatch.IgnoreCaseAndAccents"/>, if they don't exist yet.
    /// Can be called multiple times.
    /// </summary>
    public void InstallTextSearchExtensions()
    {
        _db.ExecuteNonQuery("CREATE COLLATION IF NOT EXISTS quib_ignore_case_accents (provider = icu, locale = 'und-u-ks-level1', deterministic = false);");
    }
#endif

    /// <summary>
    /// 
    /// </summary>
    /// <param name="migration"></param>
    /// <exception cref="MigrationException"></exception>
    public async Task RunMigration(DbMigration migration)
    {
        ArgumentNullException.ThrowIfNull(migration);

        var versionsTable = new VersionsTable(this, _db, migration.VersionsTable);
        var initialVersions = await versionsTable.GetCurrentVersions();

        var migrations = migration.CreateMigrationRun(initialVersions);
        foreach (var version in migrations)
        {
            await version.Run(this, _db);
            await versionsTable.SetCurrentVersion(version.ModuleName, version.Version);
        }
    }

    public void Dispose()
    {

    }


}