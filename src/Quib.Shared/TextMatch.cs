#if MYSQL56
using Quib.MySql56.Installer;
namespace Quib.MySql56;
#elif MYSQL57
namespace Quib.MySql57;
#elif MYSQL80
namespace Quib.MySql80;
#elif MYSQL84
namespace Quib.MySql84;
#elif POSTGRES18
namespace Quib.Postgres18;
#endif


/// <summary>
/// How text is compared by text functions like <c>Contains</c>, <c>StartsWith</c>, <c>EndsWith</c> and <c>Equals</c>.
/// </summary>
public enum TextMatch
{

#if MYSQL56 || MYSQL57

    /// <summary>
    /// Compared with the collation of the column (or of the connection, if there's no column). For columns created by
    /// Quib, it depends on the <see cref="TextCollation" /> setting (MySQL 5.x). Can use indexes in some cases.
    /// </summary>
    ColumnDefault,

    /// <summary>
    /// Case- and accent-sensitive, no matter the collation of the column. Indexes can't be used (MySQL).
    /// </summary>
    Exact,

    /// <summary>
    /// Case- and accent-insensitive, e.g. "muller" matches "Müller". Indexes can't be used.
    /// </summary>
    IgnoreCaseAndAccents

#elif MYSQL

    /// <summary>
    /// Compared with the collation of the column (or of the connection, if there's no column). For columns created by
    /// Quib, it's case- and accent-sensitive. Can use indexes in some cases (e.g. for <c>Equals</c> and <c>StartsWith</c> 
    /// with Quib columns).
    /// </summary>
    ColumnDefault,

    /// <summary>
    /// Case- and accent-sensitive, no matter the collation of the column. Indexes can't be used (MySQL).
    /// </summary>
    Exact,

    /// <summary>
    /// Case- and accent-insensitive, e.g. "muller" matches "Müller". Indexes can't be used.
    /// </summary>
    IgnoreCaseAndAccents

#elif POSTGRES

    /// <summary>
    /// Compared with the collation of the column (or of the connection, if there's no column). For columns created by
    /// Quib, it's case- and accent-sensitive. Can use indexes in some cases (e.g. for <c>Equals</c> and <c>StartsWith</c> 
    /// with Quib columns).
    /// </summary>
    ColumnDefault,

    /// <summary>
    /// Case- and accent-sensitive, no matter the collation of the column. Indexes can be used (Postgres).
    /// </summary>
    Exact,

    /// <summary>
    /// Case- and accent-insensitive, e.g. "muller" matches "Müller". Requires the collation created by 
    /// <see cref="Installer.DbInstaller.InstallTextSearchExtensions"/>.
    /// </summary>
    IgnoreCaseAndAccents

#endif

}
