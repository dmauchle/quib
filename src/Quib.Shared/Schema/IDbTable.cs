#if MYSQL
namespace Quib.MySql.Schema;
#elif POSTGRES
namespace Quib.Postgres.Schema;
#endif


public interface IDbTableFactory<T> where T : IDbTable
{
    public static abstract T Factory(DbTable table);
}

public interface IDbTable
{

    public static abstract string TableName { get; }

}