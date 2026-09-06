#if MYSQL
namespace Quib.MySql.QueryBuilder;
#elif POSTGRES
namespace Quib.Postgres.QueryBuilder;
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