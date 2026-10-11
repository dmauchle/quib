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


/// <summary>
/// Field alias of a projection, the f1 in "SELECT name as f1 FROM users u".
/// </summary>
internal sealed class ProjectionAlias : IQueryExpression, IProjectionField
{

    private readonly int _index;

    internal ProjectionAlias(int index)
    {
        _index = index;
    }

    public int Index => _index;

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("f");
        query.Write(_index.ToString());
    }

}
