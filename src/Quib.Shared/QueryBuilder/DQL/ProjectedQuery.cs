using System.Runtime.CompilerServices;

#if MYSQL
namespace Quib.MySql.QueryBuilder;
#elif POSTGRES
namespace Quib.Postgres.QueryBuilder;
#endif

public class ProjectedQuery<TProj> : IQueryPart
{

    private readonly QueryFactory _db;
    private readonly TProj _accessor;
    private readonly ISelectQuery _baseQuery;
    private readonly IProjectionClause[] _projection;

    internal ProjectedQuery(QueryFactory con, TProj accessor, ISelectQuery baseQuery, IProjectionClause[] projection)
    {
        _db = con;
        _accessor = accessor;
        _baseQuery = baseQuery;
        _projection = projection;
    }

    public async IAsyncEnumerable<TObj> FetchStreamAsync<TObj>(Func<ResultRow, TProj, TObj> map, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var sql = this.ToSql();
        using (var cmd = _db.Connection.CreateCommand())
        {
            cmd.CommandText = sql;

            using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    var row = new ResultRow(reader);
                    var obj = map(row, _accessor);
                    yield return obj;
                }
            }
        }
    }

    public async Task<TObj[]> FetchArray<TObj>(Func<ResultRow, TProj, TObj> map, CancellationToken cancellationToken = default)
    {
        var sql = this.ToSql();
        using (var cmd = _db.Connection.CreateCommand())
        {
            cmd.CommandText = sql;

            using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
            {
                var results = new List<TObj>();
                while (await reader.ReadAsync(cancellationToken))
                {
                    var row = new ResultRow(reader);
                    var obj = map(row, _accessor);
                    results.Add(obj);
                }
                return results.ToArray();
            }
        }
    }

    internal string ToSql()
    {
        var writer = new QueryWriter();
        writer.Write(this);
        return writer.ToString();
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("SELECT ");
        query.WriteCommaSeparated(_projection ?? []);
        query.Write(" ");
        query.Write(_baseQuery);
    }

}