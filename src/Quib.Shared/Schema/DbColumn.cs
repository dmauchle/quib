#if MYSQL
namespace Quib.MySql.Schema;
#elif POSTGRES
namespace Quib.Postgres.Schema;
#endif


/// <summary>
/// Physical column in a database table.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class DbColumn<T> : IQueryExpression<T>
    where T : struct, IDbType
{

    private readonly TableAlias _table;
    private readonly string _fieldname;

    internal DbColumn(TableAlias table, string fieldname)
    {
        _table = table;
        _fieldname = fieldname;
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write(_table);
        query.Write(".");
        query.WriteIdentifier(_fieldname);
    }

    internal DbColumnForUpdate ForUpdate()
    {
        return new DbColumnForUpdate(_table, _fieldname);
    }

}