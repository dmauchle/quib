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

    public override string ToString()
    {
        return _sb.ToString();
    }

}