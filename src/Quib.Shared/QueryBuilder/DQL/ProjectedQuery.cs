using System.Runtime.CompilerServices;

#if MYSQL56
namespace Quib.MySql56.QueryBuilder;
#elif MYSQL57
namespace Quib.MySql57.QueryBuilder;
#elif MYSQL80
namespace Quib.MySql80.QueryBuilder;
#elif MYSQL84
namespace Quib.MySql84.QueryBuilder;
#elif POSTGRES18
namespace Quib.Postgres18.QueryBuilder;
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

    public async IAsyncEnumerable<TObj> FetchStream<TObj>(Func<ResultReader, TProj, TObj> map, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var sql = this.ToSql();
        using (var cmd = _db.Connection.CreateCommand())
        {
            cmd.CommandText = sql;

            using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    var row = new ResultReader(reader);
                    var obj = map(row, _accessor);
                    yield return obj;
                }
            }
        }
    }

    public async Task<SingleQueryResult<TObj>> FetchOne<TObj>(Func<ResultReader, TProj, TObj> map, CancellationToken cancellationToken = default)
    {
        return await this.FetchStream(map, cancellationToken)
            .Select(x => new SingleQueryResult<TObj>(x))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TObj[]> FetchArray<TObj>(Func<ResultReader, TProj, TObj> map, CancellationToken cancellationToken = default)
    {
        return await FetchStream(map, cancellationToken).ToArrayAsync(cancellationToken);
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