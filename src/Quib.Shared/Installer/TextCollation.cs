#if MYSQL56
namespace Quib.MySql56.Installer;
#elif MYSQL57
namespace Quib.MySql57.Installer;
#elif MYSQL80
namespace Quib.MySql80.Installer;
#elif MYSQL84
namespace Quib.MySql84.Installer;
#elif POSTGRES18
namespace Quib.Postgres18.Installer;
#endif

#if MYSQL56 || MYSQL57

/// <summary>
/// Collation of text columns
/// </summary>
public enum TextCollation
{
    /// <summary>
    /// Case- and accent-sensitive comparison (utf8mb4_bin). Sorted by code point, not linguistically (A B ... Z a b ... z ü).
    /// </summary>
    CaseSensitive,

    /// <summary>
    /// Case- and accent-insensitive comparison (utf8mb4_unicode_ci, a = A, u = ü). Linguistic sort order.
    /// </summary>
    CaseInsensitive
}

#endif
