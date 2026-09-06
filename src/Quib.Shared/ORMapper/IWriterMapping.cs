#if MYSQL
namespace Quib.MySql.ORMapper;
#elif POSTGRES
namespace Quib.Postgres.ORMapper;
#endif


public interface IReaderMappingFactory<TQuery, TObj> : IReaderMapping<TQuery, TObj>
{
    public static abstract IReaderMapping<TQuery, TObj> Factory(ProjectionBuilder rs, TQuery query);
}
