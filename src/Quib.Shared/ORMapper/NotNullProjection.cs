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
/// Represents the f1 in "SELECT name as f1 FROM users u" for a not-null value.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class NotNullProjection<T> : NotNullExpression<T>, IProjectionField
    where T : struct, IExpressionType
{

    internal NotNullProjection(int fieldIndex)
        : base(new ProjectionAlias(fieldIndex))
    {
    }

    internal ProjectionAlias Alias => (ProjectionAlias)base.SQL;
    internal int Index => this.Alias.Index;
    int IProjectionField.Index => this.Alias.Index;



}
