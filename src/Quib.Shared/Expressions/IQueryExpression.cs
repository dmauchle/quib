#if MYSQL56
namespace Quib.MySql56.Expressions;
#elif MYSQL57
namespace Quib.MySql57.Expressions;
#elif MYSQL80
namespace Quib.MySql80.Expressions;
#elif MYSQL84
namespace Quib.MySql84.Expressions;
#elif POSTGRES18
namespace Quib.Postgres18.Expressions;
#endif

/// <summary>
/// Represents a SQL expression (untyped) that can be used in a query. Use <see cref="NullableExpression{T}"/> and 
/// <see cref="NotNullExpression{T}"/> as typed wrappers when needed.
/// </summary>
internal interface IQueryExpression : IQueryPart, ISortExpression
{

}
