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

    private readonly List<DbColumnForUpdate> _columns = [];
    private readonly List<Dictionary<DbColumnForUpdate, IQueryExpression>> _rows = [];
    private readonly ParameterBuilder _parameters = new ParameterBuilder();
    private readonly TableReference _table;

    private Dictionary<DbColumnForUpdate, IQueryExpression>? _currentRow = null;

    internal InsertQueryBuilder(TableReference table)
    {
        _table = table;
    }

    public ParameterBuilder Parameters => _parameters;

    public void Set<T>(DbColumn<T> column, IQueryExpression<T> value)
        where T : struct, IExpressionType
    {
        var updateColumn = column.ForUpdate();
        if (!_columns.Contains(updateColumn))
            _columns.Add(updateColumn);
        _currentRow ??= new Dictionary<DbColumnForUpdate, IQueryExpression>();
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

    internal IEnumerable<IQueryParameter> GetParameters()
    {
        return _parameters.Build();
    }
}