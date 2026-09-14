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

internal sealed class SelectQueryBuilder
{

    private readonly FromClause? _from;
    private List<QueryExpression>? _filters;
    private ISortExpression[]? _sort;

    public SelectQueryBuilder(FromClause? from = null)
    {
        _from = from;
    }

    public void Filter(QueryExpression filter)
    {
        (_filters ??= []).Add(filter);
    }

    internal void OrderBy(ISortExpression[] sort)
    {
        _sort = sort;
    }

    public void ToSql(QueryWriter query)
    {
        if (_from != null)
            query.Write(_from);

        if (_filters != null)
        {
            query.Write(" WHERE ");

            var first = true;
            foreach (var filter in _filters)
            {
                if (!first)
                    query.Write(" AND ");
                query.Write(filter);
                first = false;
            }
        }

        if (_sort != null)
        {
            query.Write(" ORDER BY ");

            var first = true;
            foreach (var sort in _sort)
            {
                if (!first)
                    query.Write(", ");
                query.Write(sort);
                first = false;
            }
        }
    }

}