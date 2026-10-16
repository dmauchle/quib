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
    public ColumnInstaller CreateWithType(IColumnType type)
    {
        ArgumentNullException.ThrowIfNull(type);

        string sql;
#if MYSQL
        sql = $"ALTER TABLE `{_tablename}` ADD COLUMN `{_columnname}` {type.TypeWithOptions} {(type.IsNullable ? "NULL" : "NOT NULL")};";
#elif POSTGRES
        sql = $"ALTER TABLE \"{_tablename}\" ADD COLUMN \"{_columnname}\" {type.TypeWithOptions} {(type.IsNullable ? "NULL" : "NOT NULL")};";
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
    public ColumnInstaller ChangeType(IColumnType type)
    {
        ArgumentNullException.ThrowIfNull(type);

        string sql;
#if MYSQL
        sql = $"ALTER TABLE `{_tablename}` MODIFY `{_columnname}` {type.TypeWithOptions} {(type.IsNullable ? "NULL" : "NOT NULL")};";
#elif POSTGRES
        sql = $"ALTER TABLE \"{_tablename}\" ALTER COLUMN \"{_columnname}\" TYPE {type.TypeWithOptions}, ALTER COLUMN \"{_columnname}\" {(type.IsNullable ? "DROP NOT NULL" : "SET NOT NULL")};";
#endif
        _db.ExecuteNonQuery(sql);
        return this;
    }

#if !MYSQL56 && !MYSQL57
    /// <summary>
    /// Execute DDL statement to rename column.
    /// </summary>
    /// <param name="newColumnName"></param>
    /// <returns></returns>
    public ColumnInstaller RenameTo(string newColumnName)
    {
        ArgumentNullException.ThrowIfNull(newColumnName);

        string sql;
#if MYSQL
        sql = $"ALTER TABLE `{_tablename}` RENAME COLUMN `{_columnname}` TO `{newColumnName}`";
#elif POSTGRES
        sql = $"ALTER TABLE \"{_tablename}\" RENAME COLUMN \"{_columnname}\" TO \"{newColumnName}\";";
#endif
        _db.ExecuteNonQuery(sql);
        return this;
    }
#endif

    /// <summary>
    /// Execute DDL statement to change column name and/or type.
    /// </summary>
    /// <param name="newColumnname"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public ColumnInstaller ChangeTo(string newColumnname, IColumnType type)
    {
        ArgumentNullException.ThrowIfNull(newColumnname);
        ArgumentNullException.ThrowIfNull(type);

#if MYSQL
        var sql = $"ALTER TABLE `{_tablename}` CHANGE `{_columnname}` `{newColumnname}` {type.TypeWithOptions} {(type.IsNullable ? "NULL" : "NOT NULL")};";
        _db.ExecuteNonQuery(sql);
#elif POSTGRES
        this.ChangeType(type);
        // Postgres fails when renaming a column to its current name
        if (newColumnname != _columnname)
            this.RenameTo(newColumnname);
#endif
        return this;
    }

}