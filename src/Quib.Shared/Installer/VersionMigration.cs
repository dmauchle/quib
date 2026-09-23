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

public sealed class VersionMigration
{

    private readonly List<ModuleVersion> _requirements = new List<ModuleVersion>();
    private Func<DbInstaller, QueryFactory, ValueTask>? _installScript = null;

    internal VersionMigration(string moduleName, int version)
    {
        this.ModuleName = moduleName;
        this.Version = version;
    }

    internal string ModuleName { get; }
    internal int Version { get; }

    internal IReadOnlyList<ModuleVersion> Requirements => field ??= _requirements.AsReadOnly();

    public VersionMigration Requires<T>(int version) where T : IModuleMigration
    {
        _requirements.Add(new ModuleVersion(T.ModuleName, version));
        return this;
    }

    public void With(Action<DbInstaller, QueryFactory> installScript)
    {
        With((install, db) =>
        {
            installScript(install, db);
            return ValueTask.CompletedTask;
        });
    }

    public void With(Func<DbInstaller, QueryFactory, ValueTask> installScript)
    {
        _installScript = installScript;
    }

    internal ValueTask Run(DbInstaller installer, QueryFactory db)
    {
        if (_installScript != null)
            return _installScript(installer, db);
        else
            return ValueTask.CompletedTask;
    }

}