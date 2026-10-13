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
/// Defines how date-times of a <see cref="DbDateTime{TTimeZone}"/> are stored. Date-time columns have no time zone, so the 
/// storage decides which time zone the stored values are in (e.g. <see cref="Utc"/> or <see cref="LocalTime"/>).
/// </summary>
public interface IDateTimeStorage
{

    /// <summary>
    /// Converts the value to the time zone it's stored in. The <see cref="DateTime.Kind"/> of the result is ignored.
    /// Values with <see cref="DateTimeKind.Unspecified"/> are expected to be in that time zone already.
    /// </summary>
    public static abstract DateTime ToDatabase(DateTime value);

    /// <summary>
    /// Converts a stored value (with <see cref="DateTimeKind.Unspecified"/>) back to a <see cref="DateTime"/> with the 
    /// corresponding <see cref="DateTime.Kind"/>.
    /// </summary>
    public static abstract DateTime FromDatabase(DateTime value);

}
