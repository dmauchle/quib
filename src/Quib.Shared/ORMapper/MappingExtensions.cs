#if MYSQL56
namespace Quib.MySql56.ORMapper;
#elif MYSQL57
namespace Quib.MySql57.ORMapper;
#elif MYSQL80
namespace Quib.MySql80.ORMapper;
#elif MYSQL84
namespace Quib.MySql84.ORMapper;
#elif POSTGRES18
namespace Quib.Postgres18.ORMapper;
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

    public static IAsyncEnumerable<bool> FetchStream(this ProjectedQuery<ProjectedValue<DbBool>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<bool[]> FetchArray(this ProjectedQuery<ProjectedValue<DbBool>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<bool>> FetchOne(this ProjectedQuery<ProjectedValue<DbBool>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<bool?> FetchStream(this ProjectedQuery<ProjectedValue<DbBoolNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<bool?[]> FetchArray(this ProjectedQuery<ProjectedValue<DbBoolNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<bool?>> FetchOne(this ProjectedQuery<ProjectedValue<DbBoolNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

}