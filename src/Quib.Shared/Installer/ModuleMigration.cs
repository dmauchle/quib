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

public sealed class ModuleMigration
{

    private readonly List<VersionMigration> _versions = new List<VersionMigration>();

    internal ModuleMigration(string moduleName)
    {
        this.ModuleName = moduleName;
    }

    internal string ModuleName { get; }
    internal IReadOnlyList<VersionMigration> Versions => field ??= _versions.AsReadOnly();

    public VersionMigration AddVersion(int version)
    {
        var migration = new VersionMigration(this.ModuleName, version);
        _versions.Add(migration);
        return migration;
    }

}