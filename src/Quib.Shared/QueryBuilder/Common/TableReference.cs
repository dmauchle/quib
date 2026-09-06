#if MYSQL
namespace Quib.MySql.QueryBuilder;
#elif POSTGRES
namespace Quib.Postgres.QueryBuilder;
#endif

/// <summary>
/// Represents a table name, e.g. the users in "SELECT * FROM users u"
/// </summary>
/// <param name="tablename"></param>
internal sealed class TableReference(string tablename) : ITableOrSubquery
{
    void IQueryPart.ToSql(QueryWriter query)
    {
        query.WriteIdentifier(tablename);
    }
}