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

using System.Collections.Generic;
using System.Linq;

public sealed class DbMigration
{
    private readonly List<ModuleMigration> _modules = new List<ModuleMigration>();

    public DbMigration(string versionsTableName)
    {
        ArgumentNullException.ThrowIfNull(versionsTableName);

        this.VersionsTable = versionsTableName;
    }

    internal string VersionsTable { get; }
    internal IReadOnlyList<ModuleMigration> Modules => field ??= _modules.AsReadOnly();

    public void Add<T>() where T : IModuleMigration
    {
        if (string.IsNullOrWhiteSpace(T.ModuleName))
            throw new MigrationException("Module name must not be empty.");

        if (T.ModuleName.Length > Installer.VersionsTable.MAX_MODULE_NAME_LENGTH)
            throw new MigrationException($"Module name '{T.ModuleName}' exceeds the maximum length of {Installer.VersionsTable.MAX_MODULE_NAME_LENGTH} characters.");

        var module = new ModuleMigration(T.ModuleName);
        _modules.Add(module);
        T.Configure(module);
    }

    internal VersionMigration[] CreateMigrationRun(VersionMigration[] initialVersions)
    {
        // flatten all migrations
        var configuredMigrations = this.Modules.SelectMany(x => x.Versions).ToArray();

        // build initial versions lookup by module name
        var initialVersionsByModule = new Dictionary<string, int>();
        foreach (var iv in initialVersions)
        {
            initialVersionsByModule[iv.ModuleName] = iv.Version;
        }

        // filter migrations: keep only versions > initial version for each module
        var filteredMigrations = configuredMigrations
            .Where(m => !initialVersionsByModule.ContainsKey(m.ModuleName) || m.Version > initialVersionsByModule[m.ModuleName])
            .ToArray();

        // merge with initial versions
        var migrations = initialVersions.Concat(filteredMigrations.OrderBy(x => x.Version)).ToArray();

        // map modules and versions
        var allModuleVersions = migrations
            .GroupBy(m => m.ModuleName)
            .ToDictionary(x => x.Key, x => x.Select(y => y.Version).ToArray());

        // map keys to migrations - validate for duplicate keys
        var map = new Dictionary<string, VersionMigration>();
        foreach (var m in migrations)
        {
            var key = $"{m.ModuleName}:{m.Version}";
            if (map.ContainsKey(key))
            {
                throw new MigrationException($"Duplicate migration key detected: '{key}'");
            }
            map[key] = m;
        }

        // adjacency list and indegree map
        var adj = new Dictionary<string, List<string>>();
        var indegree = new Dictionary<string, int>(map.Count);

        // initialize nodes
        foreach (var key in map.Keys)
        {
            adj[key] = new List<string>();
            indegree[key] = 0;
        }

        // add edges from explicit requirements
        foreach (var m in migrations)
        {
            var toKey = $"{m.ModuleName}:{m.Version}";
            foreach (var req in m.Requirements)
            {
                var fromKey = req.ToString();
                if (!map.ContainsKey(fromKey))
                {
                    if (allModuleVersions.TryGetValue(req.ModuleName, out var moduleVersions) && moduleVersions[0] > req.Version)
                        throw new MigrationException($"Migration '{fromKey}' required by '{toKey}' has already been installed before.");
                    else
                        throw new MigrationException($"Missing migration '{fromKey}' required by '{toKey}'.");
                }
                adj[fromKey].Add(toKey);
                indegree[toKey]++;

                // when version m requires a specific version of another module,
                // ensure that version is "locked" until all dependents complete.
                // add edge from m to the next version of the required module.
                var nextReqKey = $"{req.ModuleName}:{req.Version + 1}";
                if (map.ContainsKey(nextReqKey))
                {
                    if (!adj[toKey].Contains(nextReqKey))
                    {
                        adj[toKey].Add(nextReqKey);
                        indegree[nextReqKey]++;
                    }
                }
            }
        }

        // enforce version ordering within each module (v1 -> v2 -> v3 ...)
        foreach (var module in allModuleVersions.Keys)
        {
            var versions = allModuleVersions[module];
            for (var i = 0; i + 1 < versions.Length; i++)
            {
                var a = $"{module}:{versions[i]}";
                var b = $"{module}:{versions[i + 1]}";
                // avoid adding duplicate edges
                if (!adj[a].Contains(b))
                {
                    adj[a].Add(b);
                    indegree[b]++;
                }
            }
        }

        // Kahn's algorithm
        var q = new Queue<string>(indegree.Where(kv => kv.Value == 0).Select(kv => kv.Key));
        var ordered = new List<VersionMigration>(migrations.Length);

        while (q.Count > 0)
        {
            var key = q.Dequeue();
            ordered.Add(map[key]);

            foreach (var nb in adj[key])
            {
                indegree[nb]--;
                if (indegree[nb] == 0)
                {
                    q.Enqueue(nb);
                }
            }
        }

        if (ordered.Count != migrations.Length)
        {
            throw new MigrationException("Cycle detected in migration dependencies or missing dependency.");
        }

        return ordered.ToArray();
    }

}
