#if MYSQL56
namespace Quib.MySql56.ORMapper;
#elif MYSQL57
namespace Quib.MySql57.ORMapper;
#elif MYSQL80
namespace Quib.MySql80.ORMapper;
#elif MYSQL84
namespace Quib.MySql84.ORMapper;
#elif POSTGRES18
namespace Quib.Postgres18.ORMapper;
#endif


/// <summary>
/// Defines a mapping from a domain object to a db table. The mapping can be used for INSERT and UPDATE queries.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TObj"></typeparam>
public interface IWriterMapping<TQuery, TObj>
{
    public static abstract void Write(IUpdateQueryBuilder query, TQuery table, TObj obj);
}

