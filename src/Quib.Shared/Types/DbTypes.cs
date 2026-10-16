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


public readonly struct DbText : IExpressionType { }

public readonly struct DbBinary : IExpressionType { }

public readonly struct DbInt : IExpressionType { }

public readonly struct DbLong : IExpressionType { }

public readonly struct DbDecimal : IExpressionType { }

public readonly struct DbDouble : IExpressionType { }

public readonly struct DbBool : IExpressionType { }

public readonly struct DbUuid : IExpressionType { }

public readonly struct DbDateOnly : IExpressionType { }

/// <summary>
/// Date and time without time zone. <typeparamref name="TTimeZone"/> defines the time zone the values are stored in.
/// </summary>
public readonly struct DbDateTime<TTimeZone> : IExpressionType where TTimeZone : IDateTimeStorage { }
