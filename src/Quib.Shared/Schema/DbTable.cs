#if MYSQL56
namespace Quib.MySql56.Schema;
#elif MYSQL57
namespace Quib.MySql57.Schema;
#elif MYSQL80
namespace Quib.MySql80.Schema;
#elif MYSQL84
namespace Quib.MySql84.Schema;
#elif POSTGRES18
namespace Quib.Postgres18.Schema;
#endif

public readonly struct DbTable
{

    private readonly TableAlias _alias;

    internal DbTable(TableAlias alias)
    {
        _alias = alias;
    }

    public NotNullColumn<T> Column<T>(string fieldName)
        where T : struct, IExpressionType
    {
        ArgumentNullException.ThrowIfNull(fieldName);

        return new NotNullColumn<T>(_alias, fieldName);
    }

    public NullableColumn<T> NullableColumn<T>(string fieldName)
        where T : struct, IExpressionType
    {
        ArgumentNullException.ThrowIfNull(fieldName);

        return new NullableColumn<T>(_alias, fieldName);
    }

}