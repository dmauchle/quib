using System.Data.Common;

#if MYSQL56
namespace Quib.MySql56;
#elif MYSQL57
namespace Quib.MySql57;
#elif MYSQL80
namespace Quib.MySql80;
#elif MYSQL84
namespace Quib.MySql84;
#elif POSTGRES18
namespace Quib.Postgres18;
#endif

public sealed class QueryFactory(DbConnection con)
{

    public DbConnection Connection => con;

    public Action<string> Logger { get; set; } = _ => { };

    internal void Log(DbCommand cmd)
    {
        Logger(cmd.CommandText);
        foreach (var parameter in cmd.Parameters)
        {
            if (parameter is DbParameter p)
            {
                string formatValue(object? value)
                {
                    return value switch
                    {
                        null or DBNull => "NULL",
                        _ => value.ToString() ?? ""
                    };
                }
                Logger($"  {p.ParameterName} = {formatValue(p.Value)}");
            }
        }
    }

    internal int ExecuteNonQuery(string sql)
    {
        using (var cmd = con.CreateCommand())
        {
            cmd.CommandText = sql;
            Log(cmd);
            return cmd.ExecuteNonQuery();
        }
    }

    internal Task<int> ExecuteNonQueryAsync(string sql, CancellationToken cancellationToken = default)
    {
        using (var cmd = con.CreateCommand())
        {
            cmd.CommandText = sql;
            Log(cmd);
            return cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    internal Task<int> ExecuteNonQueryAsync(QueryWriter query, CancellationToken cancellationToken = default)
    {
        using (var cmd = con.CreateCommand())
        {
            query.Build(cmd);
            Log(cmd);
            return cmd.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    internal object? ExecuteScalar(string sql)
    {
        using (var cmd = con.CreateCommand())
        {
            cmd.CommandText = sql;
            Log(cmd);
            return cmd.ExecuteScalar();
        }
    }

    internal Task<object?> ExecuteScalarAsync(string sql)
    {
        using (var cmd = con.CreateCommand())
        {
            cmd.CommandText = sql;
            Log(cmd);
            return cmd.ExecuteScalarAsync();
        }
    }

    public SelectQuery<T> Select<T>(Func<SelectQueryBuilder, T> queryFactory)
    {
        var builder = new SelectQueryBuilder();
        var accessor = queryFactory(builder);
        return new SelectQuery<T>(this, builder, accessor);
    }

    public SelectQuery<T> Select<T>()
        where T : ISelectContext<T>
    {
        return this.Select(T.CreateFrom);
    }

    public SelectQuery<DbTable> SelectFromTable(string tablename)
    {
        return this.Select(query => query.FromTable(tablename));
    }

    public SelectQuery<T> SelectFrom<T>() where T : IDbTable, IDbTable<T>
    {
        return this.Select(query => query.From<T>());
    }

    public SelectQuery<T> SelectFrom<T>(Func<DbTable, T> tableFactory) where T : IDbTable
    {
        return this.Select(query => query.From(tableFactory));
    }

    public ProjectedQuery<T> SelectValues<T>(Func<ProjectionBuilder, T> factory)
    {
        var builder = new SelectQueryBuilder();
        var select = new SelectQuery<object>(this, builder, new object());
        return select.Project((rs, _) => factory(rs));
    }

    public InsertQuery<DbTable> InsertIntoTable(string tablename)
    {
        var table = new TableReference(tablename);
        var accessor = new DbTable(new TableAlias(0));
        var builder = new InsertQueryBuilder(table);
        return new InsertQuery<DbTable>(this, builder, accessor);
    }

    public InsertQuery<T> InsertInto<T>() where T : IDbTable, IDbTable<T>
    {
        return this.InsertInto(T.CreateFrom);
    }

    public InsertQuery<T> InsertInto<T>(Func<DbTable, T> tableFactory) where T : IDbTable
    {
        var table = new TableReference(T.TableName);
        var accessor = new DbTable(new TableAlias(0));
        var builder = new InsertQueryBuilder(table);
        return new InsertQuery<T>(this, builder, tableFactory(accessor));
    }

    public IUpdateQuery<DbTable> UpdateTable(string tablename)
    {
        var table = new TableReference(tablename);
        var accessor = new DbTable(new TableAlias(0));
        var builder = new UpdateQueryBuilder(table);
        return new UpdateQuery<DbTable>(this, builder, accessor);
    }

    public IUpdateQuery<T> Update<T>() where T : IDbTable, IDbTable<T>
    {
        return this.Update(T.CreateFrom);
    }

    public IUpdateQuery<T> Update<T>(Func<DbTable, T> tableFactory) where T : IDbTable
    {
        var table = new TableReference(T.TableName);
        var accessor = new DbTable(new TableAlias(0));
        var builder = new UpdateQueryBuilder(table);
        return new UpdateQuery<T>(this, builder, tableFactory(accessor));
    }

    public IDeleteQuery<DbTable> DeleteFromTable(string tablename)
    {
        var table = new TableReference(tablename);
        var accessor = new DbTable(new TableAlias(0));
        var builder = new DeleteQueryBuilder(table);
        return new DeleteQuery<DbTable>(this, builder, accessor);
    }

    public IDeleteQuery<T> DeleteFrom<T>() where T : IDbTable, IDbTable<T>
    {
        return this.DeleteFrom(T.CreateFrom);
    }

    public IDeleteQuery<T> DeleteFrom<T>(Func<DbTable, T> tableFactory) where T : IDbTable
    {
        var table = new TableReference(T.TableName);
        var accessor = new DbTable(new TableAlias(0));
        var builder = new DeleteQueryBuilder(table);
        return new DeleteQuery<T>(this, builder, tableFactory(accessor));
    }

}