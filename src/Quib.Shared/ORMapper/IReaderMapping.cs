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


public interface IReaderMappingFactory<TQuery, TObj> : IReaderMapping<TObj>
{
    public static abstract IReaderMapping<TObj> Factory(ProjectionBuilder rs, TQuery query);
}


public interface IReaderMapping<TObj>
{
    /// <summary>
    /// Gets the key that uniquely identify the object. Will be used to maintain object identity 
    /// across the query. It can be a multi-column key. When returning an empty key, every entity
    /// is considered unique and will be created as a new object. 
    /// </summary>
    public IProjectedValue[] ObjectIdentity { get; }
    public TObj Map(ResultReader rs);
}