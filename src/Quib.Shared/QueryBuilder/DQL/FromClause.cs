#if MYSQL
namespace Quib.MySql.QueryBuilder;
#elif POSTGRES
namespace Quib.Postgres.QueryBuilder;
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