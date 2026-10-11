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

internal sealed class DeleteQueryBuilder
{

    private readonly TableReference _table;
    private List<IQueryExpression>? _filters;

    internal DeleteQueryBuilder(TableReference table)
    {
        _table = table;
    }

    internal void AddFilter(IQueryExpression filter)
    {
        (_filters ??= []).Add(filter);
    }

    internal void ToSql(QueryWriter query)
    {
#if MYSQL
        query.Write("DELETE t0 FROM ");
#elif POSTGRES
        query.Write("DELETE FROM ");
#endif
        query.Write(_table);
        query.Write(" t0 ");

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
    }

}