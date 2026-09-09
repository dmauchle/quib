using System.Data.Common;
using System.Text;

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



internal sealed class QueryWriter
{

    private readonly StringBuilder _sb = new StringBuilder();
    private readonly Dictionary<IQueryParameter, int> _parameters = new Dictionary<IQueryParameter, int>();

    public void Write(string str)
    {
        _sb.Append(str);
    }

    public void Write(int num)
    {
        _sb.Append(num);
    }

    public void Write<T>(T part)
        where T : IQueryPart
    {
        part.ToSql(this);
    }

    public void WriteParameter(IQueryParameter parameter)
    {
        if (!_parameters.TryGetValue(parameter, out var parameterIndex))
        {
            parameterIndex = _parameters.Count+1;
            _parameters.Add(parameter, parameterIndex);
        }
        _sb.Append("@p");
        _sb.Append(parameterIndex);
    }

    public void WriteCommaSeparated<T>(T[] parts)
        where T : IQueryPart
    {

        for (var i = 0; i < parts.Length; i++)
        {
            if (i > 0)
                Write(", ");
            Write(parts[i]);
        }
    }

    public void WriteCommaSeparated<T>(IEnumerable<T> parts)
        where T : IQueryPart
    {
        var first = true;
        foreach (var part in parts)
        {
            if (!first)
                Write(", ");
            Write(part);
            first = false;
        }
    }

    public void Build(DbCommand command)
    {
        command.CommandText = _sb.ToString();
        foreach (var p in _parameters)
        {
            var cmdParameter = command.CreateParameter();
            cmdParameter.ParameterName = $"@p{p.Value}";
            cmdParameter.Value = p.Key.Value ?? DBNull.Value;
            command.Parameters.Add(cmdParameter);
        }
    }

    public override string ToString()
    {
        return _sb.ToString();
    }

}