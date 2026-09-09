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


public static class MappingExtensions
{

    public static IAsyncEnumerable<string> FetchStream(this ProjectedQuery<ProjectedValue<DbText>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<string[]> FetchArray(this ProjectedQuery<ProjectedValue<DbText>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<string>> FetchOne(this ProjectedQuery<ProjectedValue<DbText>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<string?> FetchStream(this ProjectedQuery<ProjectedValue<DbTextNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<string?[]> FetchArray(this ProjectedQuery<ProjectedValue<DbTextNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<string?>> FetchOne(this ProjectedQuery<ProjectedValue<DbTextNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<int> FetchStream(this ProjectedQuery<ProjectedValue<DbInt>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<int[]> FetchArray(this ProjectedQuery<ProjectedValue<DbInt>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<int>> FetchOne(this ProjectedQuery<ProjectedValue<DbInt>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<int?> FetchStream(this ProjectedQuery<ProjectedValue<DbIntNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<int?[]> FetchArray(this ProjectedQuery<ProjectedValue<DbIntNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<int?>> FetchOne(this ProjectedQuery<ProjectedValue<DbIntNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

}