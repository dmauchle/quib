#if MYSQL
namespace Quib.MySql.ORMapper;
#elif POSTGRES
namespace Quib.Postgres.ORMapper;
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
    where T : struct, IDbType
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