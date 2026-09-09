#if MYSQL56
namespace Quib.MySql56.Schema;
#elif MYSQL57
namespace Quib.MySql57.Schema;
#elif MYSQL80
namespace Quib.MySql80.Schema;
#elif MYSQL84
namespace Quib.MySql84.Schema;
#elif POSTGRES18
namespace Quib.Postgres18.Schema;
#endif


public interface IDbText : IExpressionType { }
public readonly struct DbText : IExpressionType, INotNullExpressionType, IDbText { }
public readonly struct DbTextNullable : IExpressionType, INullableExpressionType, IDbText { }

public interface IDbInt : IExpressionType { }
public readonly struct DbInt : IExpressionType, INotNullExpressionType, IDbInt { }
public readonly struct DbIntNullable : IExpressionType, INullableExpressionType, IDbInt { }

public interface IDbBool : IExpressionType { }
public readonly struct DbBool : IExpressionType, INotNullExpressionType, IDbBool { }
public readonly struct DbBoolNullable : IExpressionType, INullableExpressionType, IDbBool { }