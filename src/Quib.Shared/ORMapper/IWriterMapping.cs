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


public interface IReaderMappingFactory<TQuery, TObj> : IReaderMapping<TQuery, TObj>
{
    public static abstract IReaderMapping<TQuery, TObj> Factory(ProjectionBuilder rs, TQuery query);
}
