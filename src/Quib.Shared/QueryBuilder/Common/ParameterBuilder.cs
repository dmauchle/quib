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

internal sealed class ParameterBuilder
{

    private readonly List<IQueryParameter> _parameters = [];

    public QueryParameter<DbText> CreateParameter(string value)
    {
        var index = _parameters.Count + 1;
        var parameter = new QueryParameter<DbText>(index, value);
        _parameters.Add(parameter);
        return parameter;
    }

    public IEnumerable<IQueryParameter> Build()
    {
        return _parameters;
    }

}