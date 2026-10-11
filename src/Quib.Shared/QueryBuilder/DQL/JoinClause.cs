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



internal sealed class JoinClause : IQueryPart
{

    private readonly string _joinType;
    private readonly ITableOrSubquery _tableOrSubquery;
    private readonly TableAlias _alias;
    private IQueryExpression? _condition;

    public JoinClause(string joinType, ITableOrSubquery tableOrSubquery, TableAlias alias)
    {
        _joinType = joinType;
        _tableOrSubquery = tableOrSubquery;
        _alias = alias;
    }

    public TableAlias Alias => _alias;

    /// <summary>
    /// Sets/replaces the ON condition for this join clause.
    /// </summary>
    /// <param name="condition"></param>
    public void On(IQueryExpression condition)
    {
        _condition = condition;
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write(_joinType);
        query.Write(" ");
        query.Write(_tableOrSubquery);
        query.Write(" AS ");
        query.Write(_alias);

        if (_condition is not null)
        {
            query.Write(" ON ");
            query.Write(_condition);
        }
    }

}