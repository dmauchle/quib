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
    // the module name is the primary key: keep it within the InnoDB index limit of 767 bytes (utf8mb4) on MySQL 5.6
    public const int MAX_MODULE_NAME_LENGTH = 128;

    private readonly DbInstaller _installer;
    private readonly QueryFactory _db;
    private readonly string _tableName;

    private bool? _tableExists;

    private class TableDefinition(DbTable table)
    {

        public NotNullColumn<DbText> ModuleName => table.Column<DbText>("module");
        public NotNullColumn<DbInt> Version => table.Column<DbInt>("version");

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
                .CreateWithPrimaryKey("module", ColumnType.VarChar(MAX_MODULE_NAME_LENGTH))
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