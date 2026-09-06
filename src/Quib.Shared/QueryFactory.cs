using System.Data.Common;

#if MYSQL
namespace Quib.MySql;
#elif POSTGRES
namespace Quib.Postgres;
#endif

public sealed class QueryFactory(DbConnection con)
{

    public DbConnection Connection => con;

    internal int ExecuteNonQuery(string sql, params IEnumerable<IQueryParameter> parameters)
    {
        using var cmd = con.CreateCommand();
        cmd.CommandText = sql;
        foreach (var p in parameters)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = p.Name;
            param.Value = p.Value ?? DBNull.Value;
            cmd.Parameters.Add(param);
        }
        return cmd.ExecuteNonQuery();
    }

    internal Task<int> ExecuteNonQueryAsync(string sql)
    {
        var cmd = con.CreateCommand();
        cmd.CommandText = sql;
        return cmd.ExecuteNonQueryAsync();
    }

    internal object? ExecuteScalar(string sql)
    {
        using var cmd = con.CreateCommand();
        cmd.CommandText = sql;
        return cmd.ExecuteScalar();
    }

    internal Task<object?> ExecuteScalarAsync(string sql)
    {
        var cmd = con.CreateCommand();
        cmd.CommandText = sql;
        return cmd.ExecuteScalarAsync();
    }

    public SelectQuery<DbTable> SelectFromTable(string tablename)
    {
        var table = new TableReference(tablename);
        var from = new FromClause(table);
        var builder = new SelectQueryBuilder(from);
        var accessor = new DbTable(from.Alias);
        return new SelectQuery<DbTable>(this, builder, accessor);
    }

    public SelectQuery<T> SelectFrom<T>() where T : IDbTable, IDbTableFactory<T>
    {
        return this.SelectFrom(T.Factory);
    }

    public SelectQuery<T> SelectFrom<T>(Func<DbTable, T> tableFactory) where T : IDbTable
    {
        var table = new TableReference(T.TableName);
        var from = new FromClause(table);
        var builder = new SelectQueryBuilder(from);
        var accessor = new DbTable(from.Alias);
        return new SelectQuery<T>(this, builder, tableFactory(accessor));
    }

    public InsertQuery<DbTable> InsertInto(string tablename)
    {
        var table = new TableReference(tablename);
        var accessor = new DbTable(new TableAlias(0));
        var builder = new InsertQueryBuilder(table);
        return new InsertQuery<DbTable>(this, builder, accessor);
    }

    public InsertQuery<T> InsertInto<T>() where T : IDbTable, IDbTableFactory<T>
    {
        return this.InsertInto(T.Factory);
    }

    public InsertQuery<T> InsertInto<T>(Func<DbTable, T> tableFactory) where T : IDbTable
    {
        var table = new TableReference(T.TableName);
        var accessor = new DbTable(new TableAlias(0));
        var builder = new InsertQueryBuilder(table);
        return new InsertQuery<T>(this, builder, tableFactory(accessor));
    }

}