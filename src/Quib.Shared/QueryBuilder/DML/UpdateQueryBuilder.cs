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

public sealed class UpdateQueryBuilder : IUpdateQueryBuilder
{

    private readonly List<(DbColumnForUpdate column, IQueryExpression value)> _values = [];
    private readonly TableReference _table;

    internal UpdateQueryBuilder(TableReference table)
    {
        _table = table;
    }

    public void Set<T>(DbColumn<T> column, IQueryExpression<T> value)
        where T : struct, IExpressionType
    {
        var updateColumn = column.ForUpdate();
        _values.Add((updateColumn, value));
    }

    internal void ToSql(QueryWriter query)
    {
        query.Write("UPDATE ");
        query.Write(_table);
        query.Write(" t0 ");
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

}