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

    private readonly ProjectedQuery<IReaderMapping<TQuery, TObj>> _projection;

    internal ProjectedMap(ProjectedQuery<IReaderMapping<TQuery, TObj>> projection)
    {
        _projection = projection;
    }

    public IAsyncEnumerable<TObj> FetchStreamAsync()
    {
        return _projection.FetchStream((rs, x) => x.Map(rs));
    }

    public Task<TObj[]> FetchArrayAsync()
    {
        return _projection.FetchArray((rs, x) => x.Map(rs));
    }

}
