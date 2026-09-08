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



internal sealed class FromClause(ITableOrSubquery tableOrSubquery) : IQueryPart
{

    public TableAlias Alias => new TableAlias(0);

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("FROM ");
        query.Write(tableOrSubquery);
        query.Write(" AS ");
        query.Write(Alias);
    }

}