#if MYSQL
namespace Quib.MySql.ORMapper;
#elif POSTGRES
namespace Quib.Postgres.ORMapper;
#endif


public interface IWriterMapping<TQuery, TObj>
{
    public static abstract void Write(InsertQueryBuilder query, TQuery table, TObj obj);
}

public interface IReaderMapping<TQuery, TObj>
{
    public TObj Map(ResultRow rs);
}