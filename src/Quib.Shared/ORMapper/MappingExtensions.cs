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

    public static IAsyncEnumerable<Guid> FetchStream(this ProjectedQuery<ProjectedValue<DbUuid>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<Guid[]> FetchArray(this ProjectedQuery<ProjectedValue<DbUuid>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<Guid>> FetchOne(this ProjectedQuery<ProjectedValue<DbUuid>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<Guid?> FetchStream(this ProjectedQuery<ProjectedValue<DbUuidNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<Guid?[]> FetchArray(this ProjectedQuery<ProjectedValue<DbUuidNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<Guid?>> FetchOne(this ProjectedQuery<ProjectedValue<DbUuidNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<decimal> FetchStream(this ProjectedQuery<ProjectedValue<DbDecimal>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<decimal[]> FetchArray(this ProjectedQuery<ProjectedValue<DbDecimal>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<decimal>> FetchOne(this ProjectedQuery<ProjectedValue<DbDecimal>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<decimal?> FetchStream(this ProjectedQuery<ProjectedValue<DbDecimalNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<decimal?[]> FetchArray(this ProjectedQuery<ProjectedValue<DbDecimalNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<decimal?>> FetchOne(this ProjectedQuery<ProjectedValue<DbDecimalNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<DateOnly> FetchStream(this ProjectedQuery<ProjectedValue<DbDateOnly>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<DateOnly[]> FetchArray(this ProjectedQuery<ProjectedValue<DbDateOnly>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<DateOnly>> FetchOne(this ProjectedQuery<ProjectedValue<DbDateOnly>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<DateOnly?> FetchStream(this ProjectedQuery<ProjectedValue<DbDateOnlyNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<DateOnly?[]> FetchArray(this ProjectedQuery<ProjectedValue<DbDateOnlyNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<DateOnly?>> FetchOne(this ProjectedQuery<ProjectedValue<DbDateOnlyNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<DateTime> FetchStream(this ProjectedQuery<ProjectedValue<DbDateTime>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<DateTime[]> FetchArray(this ProjectedQuery<ProjectedValue<DbDateTime>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<DateTime>> FetchOne(this ProjectedQuery<ProjectedValue<DbDateTime>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<DateTime?> FetchStream(this ProjectedQuery<ProjectedValue<DbDateTimeNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<DateTime?[]> FetchArray(this ProjectedQuery<ProjectedValue<DbDateTimeNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<DateTime?>> FetchOne(this ProjectedQuery<ProjectedValue<DbDateTimeNullable>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

}