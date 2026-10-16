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

internal sealed class UpdateQueryBuilder : IUpdateQueryBuilder
{

    private readonly List<(DbColumnForWriting column, IQueryExpression value)> _values;
    private readonly TableReference _table;
    private List<IQueryExpression>? _filters;

    internal UpdateQueryBuilder(TableReference table)
    {
        _table = table;
        _values = [];
    }

    internal void AddFilter(IQueryExpression filter)
    {
        (_filters ??= []).Add(filter);
    }

    public void Set<T>(NotNullColumn<T> column, NotNullExpression<T> value)
        where T : struct, IExpressionType
    {
        ArgumentNullException.ThrowIfNull(column);
        ArgumentNullException.ThrowIfNull(value);

        _values.Add((column.Writer, value.SQL));
    }

    public void Set<T>(NullableColumn<T> column, NullableExpression<T> value)
        where T : struct, IExpressionType
    {
        ArgumentNullException.ThrowIfNull(column);
        ArgumentNullException.ThrowIfNull(value);

        _values.Add((column.Writer, value.SQL));
    }

    internal void ToSql(QueryWriter query)
    {
        query.Write("UPDATE ");
        query.Write(_table);
        query.Write(" t0 ");

        {
            query.Write(" SET ");
            var first = true;
            foreach (var val in _values)
            {
                if (!first)
                    query.Write(", ");
                query.Write(val.column);
                query.Write(" = ");
                query.Write(val.value);
                first = false;
            }
        }

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