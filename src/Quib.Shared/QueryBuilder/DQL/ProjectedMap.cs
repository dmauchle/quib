#if MYSQL
namespace Quib.MySql.QueryBuilder;
#elif POSTGRES
namespace Quib.Postgres.QueryBuilder;
#endif

public sealed class ProjectedMap<TQuery, TObj>
{

    private readonly ProjectedQuery<IReaderMapping<TQuery, TObj>> _projection;

    internal ProjectedMap(ProjectedQuery<IReaderMapping<TQuery, TObj>> projection)
    {
        _projection = projection;
    }

    public IAsyncEnumerable<TObj> FetchStreamAsync()
    {
        return _projection.FetchStreamAsync((rs, x) => x.Map(rs));
    }

    public Task<TObj[]> FetchArrayAsync()
    {
        return _projection.FetchArray((rs, x) => x.Map(rs));
    }

}
