#if MYSQL
namespace Quib.MySql.Installer;
#elif POSTGRES
namespace Quib.Postgres.Installer;
#endif

public sealed class PrimaryKey
{

    private record Column(string ColumnName, DbType Type);

    private readonly List<Column> _columns = new List<Column>();

    public PrimaryKey()
    {

    }

    public PrimaryKey AddColumn(string columnName, DbType type)
    {
        _columns.Add(new Column(columnName, type));
        return this;
    }

    internal string ColumnDefinitionsToSql()
    {
        if (_columns.Count == 0)
            throw new ArgumentException("Missing columns in primary key.");

        return string.Join(", ", _columns.Select(f => $"{SqlSyntax.Identifier(f.ColumnName)} {f.Type.ToSql()}"));
    }

    internal string KeyDefinitionToSql()
    {
        if (_columns.Count == 0)
            throw new ArgumentException("Missing columns in primary key.");

        return string.Join(", ", _columns.Select(k => $"{SqlSyntax.Identifier(k.ColumnName)}"));
    }



}