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


/// <inheritdoc cref="IReaderMapping{TObj}"/>
public interface IReaderMapping<TQuery, TObj> : IReaderMapping<TObj>
{
    public static abstract IReaderMapping<TObj> CreateFrom(IProjectionBuilder rs, TQuery query);
}

/// <summary>
/// Defines a mapping from an SQL query result (<see cref="ResultReader"/>) to a domain object.
/// </summary>
/// <typeparam name="TObj"></typeparam>
public interface IReaderMapping<TObj>
{
    /// <summary>
    /// Gets the key that uniquely identify the object. Will be used to maintain object identity 
    /// across the query. It can be a multi-column key. When returning an empty key, every entity
    /// is considered unique and will be created as a new object. 
    /// </summary>
    public IProjectionField[] ObjectIdentity { get; }

    public TObj Map(ResultReader rs);
}