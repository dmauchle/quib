#if MYSQL
namespace Quib.MySql.QueryBuilder;
#elif POSTGRES
namespace Quib.Postgres.QueryBuilder;
#endif


/// <summary>
/// Represents the "u" in a query like "SELECT * FROM users u". Could not only be used for tables, but also for views or subqueries.
/// </summary>
internal readonly record struct TableAlias : IQueryPart
{

    private readonly int _tableIndex;

    internal TableAlias(int tableIndex)
    {
        _tableIndex = tableIndex;
    }

    public int TableIndex => _tableIndex;

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("t");
        query.Write(_tableIndex);
    }

}