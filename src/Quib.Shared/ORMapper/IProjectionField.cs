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
/// Represents the f1 in "SELECT name as f1 FROM users u", untyped. It can either be a <see cref="NotNullProjection{T}"/>
/// or a <see cref="NullableProjection{T}"/>.
/// </summary>
public interface IProjectionField
{
    internal int Index { get; }
}
