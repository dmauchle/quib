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

public sealed class ProjectedMap<TQuery, TObj>
{

    private readonly ProjectedQuery<IReaderMapping< TObj>> _projection;

    internal ProjectedMap(ProjectedQuery<IReaderMapping<TObj>> projection)
    {
        _projection = projection;
    }

    public IAsyncEnumerable<TObj> FetchStream(CancellationToken cancellationToken = default)
    {
        return _projection.FetchStream((rs, x) => x.Map(rs), cancellationToken);
    }

    public Task<TObj[]> FetchArray(CancellationToken cancellationToken = default)
    {
        return _projection.FetchArray((rs, x) => x.Map(rs), cancellationToken);
    }

    public Task<SingleQueryResult<TObj>> FetchOne(CancellationToken cancellationToken = default)
    {
        return _projection.FetchOne((rs, x) => x.Map(rs), cancellationToken);
    }

}
