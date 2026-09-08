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

internal static class Collation
{

    public static string Default => "utf8mb4_unicode_520_ci";

}

public sealed class TableInstaller
{

    private readonly QueryFactory _db;
    private readonly string _tablename;

    internal TableInstaller(QueryFactory con, string tablename)
    {
        _db = con;
        _tablename = tablename;
    }

    /// <summary>
    /// Query database if a table exists
    /// </summary>
    /// <returns></returns>
    public bool Exists()
    {
        using (var cmd = _db.Connection.CreateCommand())
        {
#if MYSQL
            var sql = $"SHOW TABLES LIKE '{_tablename}'";
            var rs = _db.ExecuteScalar(sql);
            return rs != null;
#elif POSTGRES
            var sql = $"SELECT to_regclass('{_tablename}') IS NOT NULL;";
            var rs = _db.ExecuteScalar(sql);
            return rs is true;
#endif

        }
    }

    /// <summary>
    /// Execute DDL statement to create table
    /// </summary>
    /// <param name="primaryKey"></param>
    public TableInstaller CreateWithPrimaryKey(string columnname, DbType type)
    {
        CreateWithPrimaryKey(new PrimaryKey().AddColumn(columnname, type));
        return this;
    }

    /// <summary>
    /// Execute DDL statement to create table
    /// </summary>
    /// <param name="primaryKey"></param>
    public TableInstaller CreateWithPrimaryKey(PrimaryKey primaryKey)
    {
#if MYSQL
        var sql = $"CREATE TABLE `{_tablename}` ({primaryKey.ColumnDefinitionsToSql()}, PRIMARY KEY ({primaryKey.KeyDefinitionToSql()})) COLLATE {Collation.Default} ENGINE = InnoDB;";
#elif POSTGRES
        var sql = $"CREATE TABLE \"{_tablename}\" ({primaryKey.ColumnDefinitionsToSql()}, PRIMARY KEY ({primaryKey.KeyDefinitionToSql()}));";
#endif
        _db.ExecuteNonQuery(sql);
        return this;
    }

    /// <summary>
    /// Execute DDL statement to drop table
    /// </summary>
    public void Drop()
    {
#if MYSQL
        var sql = $"DROP TABLE `{_tablename}`;";
#elif POSTGRES
        var sql = $"DROP TABLE \"{_tablename}\";";
#endif
        _db.ExecuteNonQuery(sql);
    }

    /// <summary>
    /// Execute DDL statement to rename table
    /// </summary>
    /// <param name="newTableName"></param>
    public void RenameTo(string newTableName)
    {
#if MYSQL
        var sql = $"ALTER TABLE `{_tablename}` RENAME `{newTableName}`;";
#elif POSTGRES
        var sql = $"ALTER TABLE \"{_tablename}\" RENAME TO \"{newTableName}\";";
#endif
        _db.ExecuteNonQuery(sql);
    }

    public TableInstaller AddColumn(string columnname, DbType type)
    {
        this.Column(columnname).CreateWithType(type);
        return this;
    }

    public ColumnInstaller Column(string columnname)
    {
        return new ColumnInstaller(_db, _tablename, columnname);
    }

    public TableInstaller CreateForeignKey(string keyname, string[] columns, string foreignTable, string[] foreignColumns)
    {
        if (columns.Length == 0)
            throw new ArgumentException("Missing columns in primary key.", nameof(columns));
        if (foreignColumns.Length != columns.Length)
            throw new ArgumentException("Number of foreign key columns must match primary key columns.", nameof(foreignColumns));

        var localColumnSql = string.Join(", ", columns.Select(SqlSyntax.Identifier));
        var foreignColumnSql = string.Join(", ", foreignColumns.Select(SqlSyntax.Identifier));
#if MYSQL
        var sql = $"ALTER TABLE `{_tablename}` ADD CONSTRAINT `{keyname}` FOREIGN KEY ({localColumnSql}) REFERENCES `{foreignTable}`({foreignColumnSql})";
#elif POSTGRES
        var sql = $"ALTER TABLE \"{_tablename}\" ADD CONSTRAINT \"{keyname}\" FOREIGN KEY ({localColumnSql}) REFERENCES \"{foreignTable}\"({foreignColumnSql});";
#endif
        _db.ExecuteNonQuery(sql);
        return this;
    }

    public TableInstaller DropForeignKey(string keyname)
    {
#if MYSQL
        var sql = $"ALTER TABLE `{_tablename}` DROP FOREIGN KEY `{keyname}`;";
#elif POSTGRES
        var sql = $"ALTER TABLE \"{_tablename}\" DROP CONSTRAINT \"{keyname}\";";
#endif
        _db.ExecuteNonQuery(sql);
        return this;
    }

    public TableInstaller CreateIndex(string indexname, params string[] columns)
    {
        if (columns == null || columns.Length == 0)
            throw new ArgumentNullException(nameof(columns));

        var columnParameters = string.Join(", ", columns.Select(x => $"{SqlSyntax.Identifier(x)} ASC"));
#if MYSQL
        var sql = $"ALTER TABLE `{_tablename}` ADD INDEX `{indexname}` ({columnParameters});";
#elif POSTGRES
        var sql = $"CREATE INDEX \"{indexname}\" ON \"{_tablename}\" ({columnParameters});";
#endif
        _db.ExecuteNonQuery(sql);
        return this;
    }

    public TableInstaller DropIndex(string indexname)
    {
#if MYSQL
        var sql = $"ALTER TABLE `{_tablename}` DROP INDEX `{indexname}`;";
#elif POSTGRES
        var sql = $"DROP INDEX \"{indexname}\";";
#endif
        _db.ExecuteNonQuery(sql);
        return this;
    }

}