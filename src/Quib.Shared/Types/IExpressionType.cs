#if MYSQL56
namespace Quib.MySql56.Types;
#elif MYSQL57
namespace Quib.MySql57.Types;
#elif MYSQL80
namespace Quib.MySql80.Types;
#elif MYSQL84
namespace Quib.MySql84.Types;
#elif POSTGRES18
namespace Quib.Postgres18.Types;
#endif


/// <summary>
/// A DB type of an expression like <see cref="DbInt"/>, <see cref="DbText"/> etc. The DB type doesn't denote nullability.
/// It's expressed by the expression itself (<see cref="NotNullExpression{T}"/> or <see cref="NullableExpression{T}"/>).
/// </summary>
public interface IExpressionType
{
}
