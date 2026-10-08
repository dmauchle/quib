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


public interface IDbText : IExpressionType { }
public readonly struct DbText : IExpressionType, INotNullExpressionType, IDbText { }
public readonly struct DbTextNullable : IExpressionType, INullableExpressionType, IDbText { }

public interface IDbInt : IExpressionType { }
public readonly struct DbInt : IExpressionType, INotNullExpressionType, IDbInt { }
public readonly struct DbIntNullable : IExpressionType, INullableExpressionType, IDbInt { }

public interface IDbBool : IExpressionType { }
public readonly struct DbBool : IExpressionType, INotNullExpressionType, IDbBool { }
public readonly struct DbBoolNullable : IExpressionType, INullableExpressionType, IDbBool { }

public interface IDbUuid : IExpressionType { }
public readonly struct DbUuid : IExpressionType, INotNullExpressionType, IDbUuid { }
public readonly struct DbUuidNullable : IExpressionType, INullableExpressionType, IDbUuid { }

public interface IDbDecimal : IExpressionType { }
public readonly struct DbDecimal : IExpressionType, INotNullExpressionType, IDbDecimal { }
public readonly struct DbDecimalNullable : IExpressionType, INullableExpressionType, IDbDecimal { }
