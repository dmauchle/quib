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
/// A DB type of a <see cref="QueryExpression"/> like <see cref="DbInt"/>, <see cref="DbText"/> etc. We also distinguish 
/// between nullable and not-nullable types. For example, <see cref="DbInt"/> is not-nullable, while <see cref="DbIntNullable"/> 
/// is nullable. <see cref="INotNullExpressionType"/> and <see cref="INullableExpressionType"/> /// are used to distinguish 
/// between them.
/// </summary>
public interface IExpressionType
{

}

/// <summary>
/// I not-nullable <see cref="IExpressionType"/>.
/// </summary>
public interface INotNullExpressionType : IExpressionType
{
}

/// <summary>
/// I nullable <see cref="IExpressionType"/>.
/// </summary>
public interface INullableExpressionType : IExpressionType
{
}
