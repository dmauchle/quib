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

internal sealed class InsertQueryBuilder : IUpdateQueryBuilder
{

    private readonly List<DbColumnForWriting> _columns = [];
    private readonly List<Dictionary<DbColumnForWriting, IQueryExpression>> _rows = [];
    private readonly TableReference _table;

    private Dictionary<DbColumnForWriting, IQueryExpression>? _currentRow = null;

    internal InsertQueryBuilder(TableReference table)
    {
        _table = table;
    }

    public void Set<T>(NotNullColumn<T> column, NotNullExpression<T> value)
        where T : struct, IExpressionType
    {
        Set(column.Writer, value.SQL);
    }

    public void Set<T>(NullableColumn<T> column, NullableExpression<T> value)
        where T : struct, IExpressionType
    {
        Set(column.Writer, value.SQL);
    }

    private void Set(DbColumnForWriting updateColumn, IQueryExpression value)
    {
        if (!_columns.Contains(updateColumn))
            _columns.Add(updateColumn);
        _currentRow ??= new Dictionary<DbColumnForWriting, IQueryExpression>();
        _currentRow[updateColumn] = value;
    }

    internal void NextRow()
    {
        if (_currentRow != null && _currentRow.Count > 0)
        {
            _rows.Add(_currentRow);
            _currentRow = null;
        }
    }

    internal void ToSql(QueryWriter query)
    {
        query.Write("INSERT INTO ");
        query.Write(_table);
        query.Write(" (");
        query.WriteCommaSeparated(_columns);
        query.Write(") VALUES ");

        var firstRow = true;
        foreach (var row in _rows)
        {
            if (!firstRow)
                query.Write(", ");

            query.Write("(");

            var firstCol = true;
            foreach (var col in _columns)
            {
                if (!firstCol)
                    query.Write(", ");

                if (row.TryGetValue(col, out var value))
                {
                    query.Write(value);
                }
                else
                {
                    query.Write("NULL");
                }
                firstCol = false;
            }

            query.Write(")");
            firstRow = false;
        }
    }

}