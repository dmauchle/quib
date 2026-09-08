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

public sealed class ResultRow
{

    private readonly IDataReader _reader;

    internal ResultRow(IDataReader reader)
    {
        _reader = reader;
    }

    public string Get(ProjectedValue<DbText> field)
    {
        return _reader.GetString(field.Index);
    }

}