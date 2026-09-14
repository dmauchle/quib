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

    internal ResultReader(IDataReader reader)
    {
        _reader = reader;
    }

    public bool IsDBNull<T>(ProjectedValue<T> field)
        where T : struct, IExpressionType
    {
        return _reader.IsDBNull(field.Index);
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