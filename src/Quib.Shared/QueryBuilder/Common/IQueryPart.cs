#if MYSQL
namespace Quib.MySql.QueryBuilder;
#elif POSTGRES
namespace Quib.Postgres.QueryBuilder;
#endif

public interface IQueryPart
{
    internal void ToSql(QueryWriter query);
}

