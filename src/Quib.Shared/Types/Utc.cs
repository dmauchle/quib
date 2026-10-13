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
/// Stores date-times as UTC. Local values are converted to UTC, read values have <see cref="DateTimeKind.Utc"/>.
/// </summary>
public readonly struct Utc : IDateTimeStorage
{

    public static DateTime ToDatabase(DateTime value)
    {
        return value.Kind == DateTimeKind.Local ? value.ToUniversalTime() : value;
    }

    public static DateTime FromDatabase(DateTime value)
    {
        return DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

}
