using System.Data.Common;

#if MYSQL
namespace Quib.MySql.QueryBuilder;
#elif POSTGRES
namespace Quib.Postgres.QueryBuilder;
#endif

internal sealed class SelectQueryBuilder
{

    private readonly FromClause _from;


    public SelectQueryBuilder(FromClause from)
    {
        _from = from;
    }

    public void ToSql(QueryWriter query)
    {
        query.Write(_from);
    }

}