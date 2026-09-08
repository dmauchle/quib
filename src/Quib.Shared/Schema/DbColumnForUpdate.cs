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

internal readonly record struct DbColumnForUpdate : IQueryPart
{
    private readonly TableAlias _table;
    private readonly string _fieldname;

    internal DbColumnForUpdate(TableAlias table, string fieldname)
    {
        _table = table;
        _fieldname = fieldname;
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        if (_table.TableIndex > 0)
            throw new InvalidOperationException("Cannot use joined tables in an update query.");

        query.WriteIdentifier(_fieldname);
    }
}
