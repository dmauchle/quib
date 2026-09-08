#if MYSQL56
namespace Quib.MySql56.QueryBuilder;
#elif MYSQL57
namespace Quib.MySql57.QueryBuilder;
#elif MYSQL80
namespace Quib.MySql80.QueryBuilder;
#elif MYSQL84
namespace Quib.MySql84.QueryBuilder;
#elif POSTGRES18
namespace Quib.Postgres18.QueryBuilder;
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