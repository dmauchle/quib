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

    public string Get(ProjectedValue<DbText> field)
    {
        return _reader.GetString(field.Index);
    }

    public string? Get(ProjectedValue<DbTextNullable> field)
    {
        if (_reader.IsDBNull(field.Index))
            return null;
        else
            return _reader.GetString(field.Index);
    }

    public int Get(ProjectedValue<DbInt> field)
    {
        return _reader.GetInt32(field.Index);
    }

    public int? Get(ProjectedValue<DbIntNullable> field)
    {
        if (_reader.IsDBNull(field.Index))
            return null;
        else
            return _reader.GetInt32(field.Index);
    }

}