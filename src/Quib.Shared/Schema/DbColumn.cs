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
/// <typeparam name="T"></typeparam>
public sealed class DbColumn<T> : IQueryExpression<T>
    where T : struct, IExpressionType
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