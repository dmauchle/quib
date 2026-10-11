#if MYSQL56
namespace Quib.MySql56.Schema;
#elif MYSQL57
namespace Quib.MySql57.Schema;
#elif MYSQL80
namespace Quib.MySql80.Schema;
#elif MYSQL84
namespace Quib.MySql84.Schema;
#elif POSTGRES18
namespace Quib.Postgres18.Schema;
#endif


/// <summary>
/// Physical column in a database table.
/// </summary>
internal sealed class DbColumnForReading : IQueryExpression
{

    private readonly TableAlias _table;
    private readonly string _fieldname;

    internal DbColumnForReading(TableAlias table, string fieldname)
    {
        _table = table;
        _fieldname = fieldname;
    }

    public TableAlias Table => _table;
    public string FieldName => _fieldname;

    /// <summary>
    /// Reading part
    /// </summary>
    /// <param name="query"></param>
    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write(_table);
        query.Write(".");
        query.WriteIdentifier(_fieldname);
    }


}