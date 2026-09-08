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


public interface IDbTableFactory<T> where T : IDbTable
{
    public static abstract T Factory(DbTable table);
}

public interface IDbTable
{

    public static abstract string TableName { get; }

}