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
        ArgumentNullException.ThrowIfNull(map);

        using (var cmd = _db.Connection.CreateCommand())
        {
            var query = new QueryWriter();
            query.Write(this);
            query.Build(cmd);
            _db.Log(cmd);

            using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
            {
                var row = new ResultReader(reader);
                while (await reader.ReadAsync(cancellationToken))
                {
                    var obj = map(row, _accessor);
                    yield return obj;
                }
            }
        }
    }

    public async Task<TObj[]> FetchArray<TObj>(Func<ResultReader, TProj, TObj> map, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(map);

        return await FetchStream(map, cancellationToken).ToArrayAsync(cancellationToken);
    }

    public async Task<SingleQueryResult<TObj>> FetchOne<TObj>(Func<ResultReader, TProj, TObj> map, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(map);

        return await this.FetchStream(map, cancellationToken)
            .Select(x => new SingleQueryResult<TObj>(x))
            .FirstOrDefaultAsync(cancellationToken);
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("SELECT ");
        query.WriteCommaSeparated(_projection ?? []);
        query.Write(" ");
        query.Write(_baseQuery);
    }

}