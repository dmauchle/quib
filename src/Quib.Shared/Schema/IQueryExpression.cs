#if MYSQL
namespace Quib.MySql.Schema;
#elif POSTGRES
namespace Quib.Postgres.Schema;
#endif

public interface IQueryExpression<T> : IQueryPart
    where T : IDbType
{

}