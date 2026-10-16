#if MYSQL56
namespace Quib.MySql56.Installer;
#elif MYSQL57
namespace Quib.MySql57.Installer;
#elif MYSQL80
namespace Quib.MySql80.Installer;
#elif MYSQL84
namespace Quib.MySql84.Installer;
#elif POSTGRES18
namespace Quib.Postgres18.Installer;
#endif

public sealed class PrimaryKey
{

    private record Column(string ColumnName, NotNullColumnType Type);

    private readonly List<Column> _columns = new List<Column>();

    public PrimaryKey()
    {

    }

    public PrimaryKey AddColumn(string columnName, ColumnType type)
    {
        ArgumentNullException.ThrowIfNull(columnName);
        ArgumentNullException.ThrowIfNull(type);

        _columns.Add(new Column(columnName, type.NotNull()));
        return this;
    }

    internal string ColumnDefinitionsToSql()
    {
        if (_columns.Count == 0)
            throw new ArgumentException("Missing columns in primary key.");

        return string.Join(", ", _columns.Select(f => $"{SqlSyntax.Identifier(f.ColumnName)} {f.Type.TypeWithOptions}"));
    }

    internal string KeyDefinitionToSql()
    {
        if (_columns.Count == 0)
            throw new ArgumentException("Missing columns in primary key.");

        return string.Join(", ", _columns.Select(k => $"{SqlSyntax.Identifier(k.ColumnName)}"));
    }



}