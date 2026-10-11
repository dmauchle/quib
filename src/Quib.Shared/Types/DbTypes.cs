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

public readonly struct DbInt : IExpressionType { }

public readonly struct DbBool : IExpressionType { }

public readonly struct DbUuid : IExpressionType { }

public readonly struct DbDecimal : IExpressionType { }

public readonly struct DbDateOnly : IExpressionType { }

public readonly struct DbDateTime : IExpressionType { }
