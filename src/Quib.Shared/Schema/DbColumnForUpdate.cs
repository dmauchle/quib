#if MYSQL
namespace Quib.MySql.Schema;
#elif POSTGRES
namespace Quib.Postgres.Schema;
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
