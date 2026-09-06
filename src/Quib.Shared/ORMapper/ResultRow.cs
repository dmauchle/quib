using System.Data;

#if MYSQL
namespace Quib.MySql.ORMapper;
#elif POSTGRES
namespace Quib.Postgres.ORMapper;
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