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

internal sealed class VersionsTable
{
    private readonly DbInstaller _installer;
    private readonly QueryFactory _db;
    private readonly string _tableName;

    private bool? _tableExists;

    private class TableDefinition(DbTable table)
    {

        public DbColumn<DbText> ModuleName => table.Column<DbText>("module");
        public DbColumn<DbInt> Version => table.Column<DbInt>("version");

    }

    private TableDefinition Reference(DbTable table) => new TableDefinition(table);

    public VersionsTable(DbInstaller installer, QueryFactory db, string tableName)
    {
        _installer = installer;
        _db = db;
        _tableName = tableName;
    }

    private bool TableExists()
    {
        if (_tableExists == null)
            _tableExists = _installer.Table(_tableName).Exists();

        return _tableExists.Value;
    }

    public Task<VersionMigration[]> GetCurrentVersions()
    {
        if (TableExists())
        {
            return _db.SelectFromTable(_tableName)
                .Project((rs, table) => new
                {
                    ModuleName = rs.Get(Reference(table).ModuleName),
                    Version = rs.Get(Reference(table).Version)
                })
                .FetchArray((rs, table) => new VersionMigration(rs.Get(table.ModuleName), rs.Get(table.Version)));
        }
        else
        {
            return Task.FromResult(Array.Empty<VersionMigration>());
        }
    }

    public async Task SetCurrentVersion(string moduleName, int version)
    {
        if (!TableExists())
        {
            _installer.Table(_tableName)
                .CreateWithPrimaryKey("module", ColumnType.VarChar(36))
                .CreateColumn("version", ColumnType.Int().NotNull());

            _tableExists = true;
        }

        var updated = await _db.UpdateTable(_tableName)
            .Where(table => Reference(table).ModuleName == moduleName)
            .With((query, table) => query.Set(Reference(table).Version, version))
            .Execute();

        if (updated == 0)
        {
            await _db.InsertIntoTable(_tableName)
                .AddRow((query, table) =>
                {
                    query.Set(Reference(table).ModuleName, moduleName);
                    query.Set(Reference(table).Version, version);
                })
                .Execute();
        }
    }
    
}