#if !MYSQL && !POSTGRES
#error Define compilation symbol MYSQL or POSTGRES
#endif

#if MYSQL
namespace Quib.MySql.Installer;
#elif POSTGRES
namespace Quib.Postgres.Installer;
#endif

public sealed class ColumnInstaller
{

    private readonly QueryFactory _db;
    private readonly string _tablename;
    private readonly string _columnname;

    internal ColumnInstaller(QueryFactory db, string tablename, string columnname)
    {
        _db = db;
        _tablename = tablename;
        _columnname = columnname;
    }

    /// <summary>
    /// Execute DDL statement to create column.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public ColumnInstaller CreateWithType(DbType type)
    {
        string sql;
#if MYSQL
        sql = $"ALTER TABLE `{_tablename}` ADD COLUMN `{_columnname}` {type.ToSql()};";
#elif POSTGRES
        sql = $"ALTER TABLE \"{_tablename}\" ADD COLUMN \"{_columnname}\" {type.ToSql()};";
#endif
        _db.ExecuteNonQuery(sql);
        return this;
    }

    /// <summary>
    /// Execute DDL statement to drop column.
    /// </summary>
    /// <returns></returns>
    public ColumnInstaller Drop()
    {
        string sql;
#if MYSQL
        sql = $"ALTER TABLE `{_tablename}` DROP COLUMN `{_columnname}`;";
#elif POSTGRES
        sql = $"ALTER TABLE \"{_tablename}\" DROP COLUMN \"{_columnname}\";";
#endif
        _db.ExecuteNonQuery(sql);
        return this;
    }

    /// <summary>
    /// Execute DDL statement to change column type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public ColumnInstaller ChangeType(DbType type)
    {
        string sql;
#if MYSQL
        sql = $"ALTER TABLE `{_tablename}` MODIFY `{_columnname}` {type.ToSql()};";
#elif POSTGRES
        sql = $"ALTER TABLE \"{_tablename}\" ALTER COLUMN \"{_columnname}\" TYPE {type.ToSql()};";
#endif
        _db.ExecuteNonQuery(sql);
        return this;
    }

    /// <summary>
    /// Execute DDL statement to rename column. Requires MySQL version 8+. For older versions, 
    /// use <see cref="ChangeTo(string, DbType)"/> instead.
    /// </summary>
    /// <param name="newColumnName"></param>
    /// <returns></returns>
    public ColumnInstaller RenameTo(string newColumnName)
    {
        string sql;
#if MYSQL
        sql = $"ALTER TABLE `{_tablename}` RENAME COLUMN `{_columnname}` TO `{newColumnName}`";
#elif POSTGRES
        sql = $"ALTER TABLE \"{_tablename}\" RENAME COLUMN \"{_columnname}\" TO \"{newColumnName}\";";
#endif
        _db.ExecuteNonQuery(sql);
        return this;
    }

}