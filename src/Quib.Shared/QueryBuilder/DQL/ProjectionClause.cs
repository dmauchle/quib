#if MYSQL
namespace Quib.MySql.QueryBuilder;
#elif POSTGRES
namespace Quib.Postgres.QueryBuilder;
#endif

internal interface IProjectionClause : IQueryPart
{

}

internal sealed class ProjectionClause<T>(IQueryExpression<T> value, ProjectedValue<T> alias) : IProjectionClause, IQueryPart
    where T : struct, IDbType
{
    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write(value);
        query.Write(" ");
        query.Write(alias);
    }
}