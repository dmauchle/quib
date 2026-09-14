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

    public bool IsDBNull<T>(ProjectedValue<T> field)
        where T : struct, IExpressionType
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

    public string Get(ProjectedValue<DbText> field)
    {
        return _reader.GetString(field.Index);
    }

    /// <summary>
    /// Use the method if the projection could be null, even if the column itself is not nullable (e.g. due to a join).
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public string? GetNullable(ProjectedValue<DbText> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetString(field.Index);
    }

    public string? Get(ProjectedValue<DbTextNullable> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetString(field.Index);
    }

    public int Get(ProjectedValue<DbInt> field)
    {
        return _reader.GetInt32(field.Index);
    }

    /// <summary>
    /// Use the method if the projection could be null, even if the column itself is not nullable (e.g. due to a join).
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public int? GetNullable(ProjectedValue<DbInt> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetInt32(field.Index);
    }

    public int? Get(ProjectedValue<DbIntNullable> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetInt32(field.Index);
    }

    public bool Get(ProjectedValue<DbBool> field)
    {
        return _reader.GetBoolean(field.Index);
    }

    /// <summary>
    /// Use the method if the projection could be null, even if the column itself is not nullable (e.g. due to a join).
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public bool? GetNullable(ProjectedValue<DbBool> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetBoolean(field.Index);
    }

    public bool? Get(ProjectedValue<DbBoolNullable> field)
    {
        if (this.IsDBNull(field))
            return null;
        else
            return _reader.GetBoolean(field.Index);
    }

}