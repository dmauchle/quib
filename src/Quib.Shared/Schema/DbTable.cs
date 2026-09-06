#if MYSQL
namespace Quib.MySql.Schema;
#elif POSTGRES
namespace Quib.Postgres.Schema;
#endif

public readonly struct DbTable
{

    private readonly TableAlias _alias;

    internal DbTable(TableAlias alias)
    {
        _alias = alias;
    }

    public DbColumn<T> Column<T>(string fieldName)
        where T : struct, IDbType
    {
        return new DbColumn<T>(_alias, fieldName);
    }

}