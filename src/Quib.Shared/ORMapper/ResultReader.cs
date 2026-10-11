using System.Data;

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

public sealed class ResultReader
{

    private readonly IDataReader _reader;
    private readonly QueryMappingCache _mappingCache;

    internal ResultReader(IDataReader reader)
    {
        _reader = reader;
        _mappingCache = new QueryMappingCache();
    }

    public bool IsDBNull(IProjectionField field)
    {
        return _reader.IsDBNull(field.Index);
    }

    /// <summary>
    /// If the object has already been mapped, it will be returned from the query context. Otherwise, a new object will be created
    /// using the mapping. The key will be used to identify the object (primary key).
    /// </summary>
    /// <typeparam name="TObj"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="key"></param>
    /// <param name="mapping"></param>
    /// <returns></returns>
    public TObj Map<TObj>(IReaderMapping<TObj> mapping)
        where TObj : notnull
    {
        // If we don't have an object identity, we always map to new object.
        var id = mapping.ObjectIdentity;
        if (id.Length == 0)
            return mapping.Map(this);

        var keyValues = mapping.ObjectIdentity.Select(x => _reader.GetValue(x.Index)).ToArray();
        var key = new ObjectIdentity(keyValues);

        if (_mappingCache.TryGet(key, out TObj? obj))
            return obj;

        var newObj = mapping.Map(this);
        _mappingCache.Add(key, newObj);
        return newObj;
    }

    /// <inheritdoc cref="Map{TObj}(IReaderMapping{TObj})"/>
    public TObj? MapNullable<TObj>(IReaderMapping<TObj> mapping)
        where TObj : class
    {
        // If we don't have an object identity, we always map to new object (because we wouldn't have the chance to check the identity fields for null).
        var id = mapping.ObjectIdentity;
        if (id.Length == 0)
            return mapping.Map(this);

        if (mapping.ObjectIdentity.All(x => _reader.IsDBNull(x.Index)))
            return null;

        var keyValues = mapping.ObjectIdentity.Select(x => _reader.GetValue(x.Index)).ToArray();
        var key = new ObjectIdentity(keyValues);

        if (_mappingCache.TryGet(key, out TObj? obj))
            return obj;

        var newObj = mapping.Map(this);
        _mappingCache.Add(key, newObj);
        return newObj;
    }

    public string Get(NotNullProjection<DbText> field)
    {
        return _reader.GetString(field.Index);
    }

    /// <summary>
    /// Use the method if the projection could be null, even if the column itself is not nullable (e.g. due to a join).
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public string? GetNullable(NotNullProjection<DbText> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetString(field.Index);
    }

    public string? Get(NullableProjection<DbText> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetString(field.Index);
    }

    public int Get(NotNullProjection<DbInt> field)
    {
        return _reader.GetInt32(field.Index);
    }

    /// <summary>
    /// Use the method if the projection could be null, even if the column itself is not nullable (e.g. due to a join).
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public int? GetNullable(NotNullProjection<DbInt> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetInt32(field.Index);
    }

    public int? Get(NullableProjection<DbInt> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetInt32(field.Index);
    }

    public bool Get(NotNullProjection<DbBool> field)
    {
        return _reader.GetBoolean(field.Index);
    }

    /// <summary>
    /// Use the method if the projection could be null, even if the column itself is not nullable (e.g. due to a join).
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public bool? GetNullable(NotNullProjection<DbBool> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetBoolean(field.Index);
    }

    public bool? Get(NullableProjection<DbBool> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetBoolean(field.Index);
    }

    public Guid Get(NotNullProjection<DbUuid> field)
    {
        return _reader.GetGuid(field.Index);
    }

    /// <summary>
    /// Use the method if the projection could be null, even if the column itself is not nullable (e.g. due to a join).
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public Guid? GetNullable(NotNullProjection<DbUuid> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetGuid(field.Index);
    }

    public Guid? Get(NullableProjection<DbUuid> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetGuid(field.Index);
    }

    public decimal Get(NotNullProjection<DbDecimal> field)
    {
        return _reader.GetDecimal(field.Index);
    }

    /// <summary>
    /// Use the method if the projection could be null, even if the column itself is not nullable (e.g. due to a join).
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public decimal? GetNullable(NotNullProjection<DbDecimal> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetDecimal(field.Index);
    }

    public decimal? Get(NullableProjection<DbDecimal> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetDecimal(field.Index);
    }

    // Dates and date-times are read as DateTime, as older drivers don't support DateOnly

    public DateOnly Get(NotNullProjection<DbDateOnly> field)
    {
        return DateOnly.FromDateTime(_reader.GetDateTime(field.Index));
    }

    /// <summary>
    /// Use the method if the projection could be null, even if the column itself is not nullable (e.g. due to a join).
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public DateOnly? GetNullable(NotNullProjection<DbDateOnly> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return DateOnly.FromDateTime(_reader.GetDateTime(field.Index));
    }

    public DateOnly? Get(NullableProjection<DbDateOnly> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return DateOnly.FromDateTime(_reader.GetDateTime(field.Index));
    }

    public DateTime Get(NotNullProjection<DbDateTime> field)
    {
        return GetLocalDateTime(field.Index);
    }

    /// <summary>
    /// Use the method if the projection could be null, even if the column itself is not nullable (e.g. due to a join).
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public DateTime? GetNullable(NotNullProjection<DbDateTime> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return GetLocalDateTime(field.Index);
    }

    public DateTime? Get(NullableProjection<DbDateTime> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return GetLocalDateTime(field.Index);
    }

    // Values are stored without time zone, so local time is assumed
    private DateTime GetLocalDateTime(int index)
    {
        return DateTime.SpecifyKind(_reader.GetDateTime(index), DateTimeKind.Local);
    }

}