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
/// Represents a single part of a (potentially multi-table) FROM clause, like the `employees e` in `SELECT * FROM employees e, departments d`.
/// </summary>
/// <param name="tableOrSubquery"></param>
internal sealed class FromClause(ITableOrSubquery tableOrSubquery, TableAlias alias) : IQueryPart
{

    public TableAlias Alias => alias;

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write(tableOrSubquery);
        query.Write(" AS ");
        query.Write(Alias);
    }

}