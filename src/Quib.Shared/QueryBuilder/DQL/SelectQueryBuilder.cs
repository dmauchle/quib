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

internal sealed class SelectQueryBuilder
{

    private readonly FromClause? _from;
    private readonly ParameterBuilder _parameters;

    public SelectQueryBuilder(ParameterBuilder parameters, FromClause? from = null)
    {
        _parameters = parameters;
        _from = from;
    }

    public ParameterBuilder Parameters => _parameters;

    public void ToSql(QueryWriter query)
    {
        if (_from != null)
            query.Write(_from);
    }

}