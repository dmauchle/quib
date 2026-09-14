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

public interface ISortExpression : IQueryPart
{

}

public enum Sort { ASC, DESC }

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