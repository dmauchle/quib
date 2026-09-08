#if MYSQL56
namespace Quib.MySql56.ORMapper;
#elif MYSQL57
namespace Quib.MySql57.ORMapper;
#elif MYSQL80
namespace Quib.MySql80.ORMapper;
#elif MYSQL84
namespace Quib.MySql84.ORMapper;
#elif POSTGRES18
namespace Quib.Postgres18.ORMapper;
#endif

internal interface IProjectedValue
{
    public int Index { get; }
}

/// <summary>
/// Represents the f1 in "SELECT name as f1 FROM users u";
/// </summary>
/// <typeparam name="T"></typeparam>
public class ProjectedValue<T> : IProjectedValue, IQueryExpression<T>
    where T : struct, IExpressionType
{

    private readonly int _fieldIndex;

    internal ProjectedValue(int fieldIndex)
    {
        _fieldIndex = fieldIndex;
    }

    internal int Index => _fieldIndex;
    int IProjectedValue.Index => _fieldIndex;

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("f");
        query.Write(_fieldIndex.ToString());
    }

}