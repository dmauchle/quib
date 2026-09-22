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
/// Represents a part of the ORDER BY clause in a SQL query, like `column_name ASC` or `column_name DESC` or just `column_name`.
/// </summary>
public interface ISortExpression : IQueryPart
{

}

/// <summary>
/// Sort direction for the ORDER BY clause, either ascending (ASC) or descending (DESC).
/// </summary>
public enum Sort { ASC, DESC }

/// <summary>
/// Represents a part in the ORDER BY clause, consisting of a value (column or expression) and a direction (ascending or descending).
/// </summary>
internal class SortClause : ISortExpression, IQueryPart
{

    private readonly QueryExpression _value;
    private readonly Sort _direction;

    public SortClause(QueryExpression value, Sort direction)
    {
        _value = value;
        _direction = direction;
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write(_value);
        query.Write(" ");
        query.Write(_direction switch
        {
            Sort.ASC => "ASC",
            Sort.DESC => "DESC",
            _ => ""
        });
    }
}