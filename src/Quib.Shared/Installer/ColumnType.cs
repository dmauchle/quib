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


public interface IColumnType
{
    internal string ToSql();
}

public sealed class ColumnType : IColumnTypeFactory
{
    private readonly string _type;
    private readonly string _defaultValue;
    private readonly string? _options;

    internal ColumnType(string type, string defaultValue, string? options = null)
    {
        _type = type;
        _defaultValue = defaultValue;
        _options = options;
    }

    public string DbType => _type;
    public string? Options => _options;

    internal string DefaultValue => _defaultValue;

    public NotNullColumnType NotNull()
    {
        return new NotNullColumnType(this);
    }

    public NullableColumnType Nullable()
    {
        return new NullableColumnType(this);
    }

}

public sealed class NotNullColumnType : IColumnType
{

    private readonly ColumnType _type;

    internal NotNullColumnType(ColumnType type)
    {
        _type = type;
    }

    public string DbType => _type.DbType;
    public string? Options => _type.Options;

    internal string ToSql()
    {
        return $"{_type.DbType} {_type.Options} NOT NULL";
    }

    string IColumnType.ToSql()
    {
        return this.ToSql();
    }

}

public sealed class NullableColumnType : IColumnType
{
    private readonly ColumnType _type;

    internal NullableColumnType(ColumnType type)
    {
        _type = type;
    }

    public string DbType => _type.DbType;
    public string? Options => _type.Options;

    internal string ToSql()
    {
        return $"{_type.DbType} {_type.Options} NULL";
    }

    string IColumnType.ToSql()
    {
        return this.ToSql();
    }

}
