#if MYSQL56
namespace Quib.MySql56.QueryBuilder;
#elif MYSQL57
namespace Quib.MySql57.QueryBuilder;
#elif MYSQL80
namespace Quib.MySql80.QueryBuilder;
#elif MYSQL84
namespace Quib.MySql84.QueryBuilder;
#elif POSTGRES18
namespace Quib.Postgres18.QueryBuilder;
#endif

internal interface IProjectionClause : IQueryPart
{

}

internal sealed class ProjectionClause<T>(IQueryExpression<T> value, ProjectedValue<T> alias) : IProjectionClause, IQueryPart
    where T : struct, IExpressionType
{
    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write(value);
        query.Write(" ");
        query.Write(alias);
    }
}