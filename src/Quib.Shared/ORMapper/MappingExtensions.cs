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

    public static IAsyncEnumerable<string> FetchStream(this ProjectedQuery<NotNullProjection<DbText>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<string[]> FetchArray(this ProjectedQuery<NotNullProjection<DbText>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<string>> FetchOne(this ProjectedQuery<NotNullProjection<DbText>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<string?> FetchStream(this ProjectedQuery<NullableProjection<DbText>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<string?[]> FetchArray(this ProjectedQuery<NullableProjection<DbText>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<string?>> FetchOne(this ProjectedQuery<NullableProjection<DbText>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<int> FetchStream(this ProjectedQuery<NotNullProjection<DbInt>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<int[]> FetchArray(this ProjectedQuery<NotNullProjection<DbInt>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<int>> FetchOne(this ProjectedQuery<NotNullProjection<DbInt>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<int?> FetchStream(this ProjectedQuery<NullableProjection<DbInt>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<int?[]> FetchArray(this ProjectedQuery<NullableProjection<DbInt>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<int?>> FetchOne(this ProjectedQuery<NullableProjection<DbInt>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<bool> FetchStream(this ProjectedQuery<NotNullProjection<DbBool>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<bool[]> FetchArray(this ProjectedQuery<NotNullProjection<DbBool>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<bool>> FetchOne(this ProjectedQuery<NotNullProjection<DbBool>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<bool?> FetchStream(this ProjectedQuery<NullableProjection<DbBool>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<bool?[]> FetchArray(this ProjectedQuery<NullableProjection<DbBool>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<bool?>> FetchOne(this ProjectedQuery<NullableProjection<DbBool>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<Guid> FetchStream(this ProjectedQuery<NotNullProjection<DbUuid>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<Guid[]> FetchArray(this ProjectedQuery<NotNullProjection<DbUuid>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<Guid>> FetchOne(this ProjectedQuery<NotNullProjection<DbUuid>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<Guid?> FetchStream(this ProjectedQuery<NullableProjection<DbUuid>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<Guid?[]> FetchArray(this ProjectedQuery<NullableProjection<DbUuid>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<Guid?>> FetchOne(this ProjectedQuery<NullableProjection<DbUuid>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<decimal> FetchStream(this ProjectedQuery<NotNullProjection<DbDecimal>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<decimal[]> FetchArray(this ProjectedQuery<NotNullProjection<DbDecimal>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<decimal>> FetchOne(this ProjectedQuery<NotNullProjection<DbDecimal>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<decimal?> FetchStream(this ProjectedQuery<NullableProjection<DbDecimal>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<decimal?[]> FetchArray(this ProjectedQuery<NullableProjection<DbDecimal>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<decimal?>> FetchOne(this ProjectedQuery<NullableProjection<DbDecimal>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<DateOnly> FetchStream(this ProjectedQuery<NotNullProjection<DbDateOnly>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<DateOnly[]> FetchArray(this ProjectedQuery<NotNullProjection<DbDateOnly>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<DateOnly>> FetchOne(this ProjectedQuery<NotNullProjection<DbDateOnly>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<DateOnly?> FetchStream(this ProjectedQuery<NullableProjection<DbDateOnly>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<DateOnly?[]> FetchArray(this ProjectedQuery<NullableProjection<DbDateOnly>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<DateOnly?>> FetchOne(this ProjectedQuery<NullableProjection<DbDateOnly>> query, CancellationToken cancellationToken = default)
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<DateTime> FetchStream<TTimeZone>(this ProjectedQuery<NotNullProjection<DbDateTime<TTimeZone>>> query, CancellationToken cancellationToken = default)
        where TTimeZone : IDateTimeStorage
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<DateTime[]> FetchArray<TTimeZone>(this ProjectedQuery<NotNullProjection<DbDateTime<TTimeZone>>> query, CancellationToken cancellationToken = default)
        where TTimeZone : IDateTimeStorage
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<DateTime>> FetchOne<TTimeZone>(this ProjectedQuery<NotNullProjection<DbDateTime<TTimeZone>>> query, CancellationToken cancellationToken = default)
        where TTimeZone : IDateTimeStorage
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

    public static IAsyncEnumerable<DateTime?> FetchStream<TTimeZone>(this ProjectedQuery<NullableProjection<DbDateTime<TTimeZone>>> query, CancellationToken cancellationToken = default)
        where TTimeZone : IDateTimeStorage
    {
        return query.FetchStream((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<DateTime?[]> FetchArray<TTimeZone>(this ProjectedQuery<NullableProjection<DbDateTime<TTimeZone>>> query, CancellationToken cancellationToken = default)
        where TTimeZone : IDateTimeStorage
    {
        return query.FetchArray((rs, x) => rs.Get(x), cancellationToken);
    }

    public static Task<SingleQueryResult<DateTime?>> FetchOne<TTimeZone>(this ProjectedQuery<NullableProjection<DbDateTime<TTimeZone>>> query, CancellationToken cancellationToken = default)
        where TTimeZone : IDateTimeStorage
    {
        return query.FetchOne((rs, x) => rs.Get(x), cancellationToken);
    }

}