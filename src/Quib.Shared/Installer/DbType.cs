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

public readonly struct DbType
{

    private readonly string _definition;

    private DbType(string definition)
    {
        _definition = definition;
    }

    internal string ToSql()
    {
        return _definition;
    }

    private static DbType Create(string type, bool nullable, string? options = null, string? defaultValue = null)
    {
        if (nullable)
            return new DbType($"{type} {options} NULL");
        else
            return new DbType($"{type} {options} NOT NULL DEFAULT {defaultValue}");
    }

    public static DbType VarChar(int length, bool nullable)
    {
        return Create($"VARCHAR({length})", nullable, defaultValue: "''");
    }

}