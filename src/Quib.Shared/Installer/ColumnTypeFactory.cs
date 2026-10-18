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


public interface IColumnTypeFactory
{

}


public static class ColumnTypeFactory
{

    extension(IColumnTypeFactory)
    {

        public static ColumnType Int()
        {
            return new ColumnType($"INTEGER", defaultValue: "0");
        }

        /// <summary>
        /// 64-bit integer (.NET <see cref="long"/>).
        /// </summary>
        /// <returns></returns>
        public static ColumnType BigInt()
        {
            return new ColumnType($"BIGINT", defaultValue: "0");
        }

        /// <summary>
        /// Exact fixed-point number, e.g. for money values.
        /// </summary>
        /// <param name="precision">Total number of digits (1 - 28).</param>
        /// <param name="scale">Number of digits after the decimal point (0 - precision).</param>
        /// <returns></returns>
        public static ColumnType Decimal(int precision, int scale)
        {
            // Precision is limited to 28 digits, so every value of the column fits into a .NET decimal
            ArgumentOutOfRangeException.ThrowIfLessThan(precision, 1);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(precision, 28);
            ArgumentOutOfRangeException.ThrowIfNegative(scale);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(scale, precision);
            return new ColumnType($"DECIMAL({precision},{scale})", defaultValue: "0");
        }

#if MYSQL

        /// <summary>
        /// Double-precision floating point number (.NET <see cref="double"/>), without NaN and infinity.
        /// </summary>
        /// <returns></returns>
        public static ColumnType Double()
        {
            return new ColumnType($"DOUBLE", defaultValue: "0");
        }

#elif POSTGRES

        /// <summary>
        /// Double-precision floating point number (.NET <see cref="double"/>), without NaN and infinity.
        /// </summary>
        /// <returns></returns>
        public static ColumnType Double()
        {
            return new ColumnType($"DOUBLE PRECISION", defaultValue: "0");
        }

#endif

        public static ColumnType Bool()
        {
            return new ColumnType($"BOOLEAN", defaultValue: "FALSE");
        }

        /// <summary>
        /// Date without time.
        /// </summary>
        /// <returns></returns>
        public static ColumnType DateOnly()
        {
            return new ColumnType($"DATE", defaultValue: "'1970-01-01'");
        }

#if MYSQL

        /// <summary>
        /// Date and time with whole seconds, without time zone.
        /// </summary>
        /// <returns></returns>
        public static ColumnType DateTime()
        {
            return new ColumnType($"DATETIME", defaultValue: "'1970-01-01 00:00:00'");
        }

#elif POSTGRES

        /// <summary>
        /// Date and time with whole seconds, without time zone.
        /// </summary>
        /// <returns></returns>
        public static ColumnType DateTime()
        {
            return new ColumnType($"timestamp(0)", defaultValue: "'1970-01-01 00:00:00'");
        }

#endif

#if MYSQL

        /// <summary>
        /// UUID stored as CHAR(36) with ascii charset and ascii_bin collation.
        /// </summary>
        /// <returns></returns>
        public static ColumnType Uuid()
        {
            // ASCII charset (1 byte per character) and binary collation for compact storage and fast, byte-wise comparison
            return new ColumnType($"CHAR(36)", defaultValue: "'00000000-0000-0000-0000-000000000000'", options: "CHARACTER SET ascii COLLATE ascii_bin");
        }

#elif POSTGRES

        /// <summary>
        /// Native UUID type (16 bytes).
        /// </summary>
        /// <returns></returns>
        public static ColumnType Uuid()
        {
            return new ColumnType($"uuid", defaultValue: "'00000000-0000-0000-0000-000000000000'");
        }

#endif

#if MYSQL56 || MYSQL57

        private static string GetCharset() => "utf8mb4";

        private static string GetCollation(TextCollation collation)
        {
            // utf8mb4_bin: case- and accent-sensitive, code point sort order. utf8mb4_unicode_ci: case- and
            // accent-insensitive, linguistic sort order. Both ignore trailing spaces (PAD SPACE).
            return collation switch
            {
                TextCollation.CaseInsensitive => "utf8mb4_unicode_ci",
                TextCollation.CaseSensitive => "utf8mb4_bin",
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// You must not exceed 65,535-byte row size limit (all columns), one character takes 4 bytes (MySQL).
        /// </summary>
        /// <param name="length"></param>
        /// <param name="collation">MySQL 5.x has no collation that is case-sensitive and sorts linguistically, see <see cref="TextCollation"/>.</param>
        /// <returns></returns>
        public static ColumnType VarChar(int length, TextCollation collation = TextCollation.CaseSensitive)
        {
            return new ColumnType($"VARCHAR({length})", defaultValue: "''", options: $"CHARACTER SET {GetCharset()} COLLATE {GetCollation(collation)}");
        }

        /// <summary>
        /// Stores up to 65,535 bytes of text data. Separate from row size limit (MySQL).
        /// </summary>
        /// <param name="collation">MySQL 5.x has no collation that is case-sensitive and sorts linguistically, see <see cref="TextCollation"/>.</param>
        /// <returns></returns>
        public static ColumnType ShortText(TextCollation collation = TextCollation.CaseSensitive)
        {
            return new ColumnType($"TEXT", defaultValue: "''", options: $"CHARACTER SET {GetCharset()} COLLATE {GetCollation(collation)}");
        }

        /// <summary>
        /// Stores up to 16 MB of text data (MySQL).
        /// </summary>
        /// <param name="collation">MySQL 5.x has no collation that is case-sensitive and sorts linguistically, see
        /// <see cref="TextCollation"/>.</param>
        /// <returns></returns>
        public static ColumnType MediumText(TextCollation collation = TextCollation.CaseSensitive)
        {
            return new ColumnType($"MEDIUMTEXT", defaultValue: "''", options: $"CHARACTER SET {GetCharset()} COLLATE {GetCollation(collation)}");
        }

        /// <summary>
        /// Stores up to 4 GB of text data (MySQL).
        /// </summary>
        /// <param name="collation">MySQL 5.x has no collation that is case-sensitive and sorts linguistically, see
        /// <see cref="TextCollation"/>.</param>
        /// <returns></returns>
        public static ColumnType LongText(TextCollation collation = TextCollation.CaseSensitive)
        {
            return new ColumnType($"LONGTEXT", defaultValue: "''", options: $"CHARACTER SET {GetCharset()} COLLATE {GetCollation(collation)}");
        }

#elif MYSQL

        // Explicitly set, independent of server/database defaults. Case- and accent-sensitive, linguistic sort order,
        // trailing spaces are significant (NO PAD). Consistent with Postgres.

        private static string GetCharset() => "utf8mb4";
        private static string GetCollation() => "utf8mb4_0900_as_cs";

        /// <summary>
        /// You must not exceed 65,535-byte row size limit (all columns), one character takes 4 bytes (MySQL).
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static ColumnType VarChar(int length)
        {
            return new ColumnType($"VARCHAR({length})", defaultValue: "''", options: $"CHARACTER SET {GetCharset()} COLLATE {GetCollation()}");
        }

        /// <summary>
        /// Stores up to 65,535 bytes of text data. Separate from row size limit (MySQL).
        /// </summary>
        /// <returns></returns>
        public static ColumnType ShortText()
        {
            return new ColumnType($"TEXT", defaultValue: "''", options: $"CHARACTER SET {GetCharset()} COLLATE {GetCollation()} ");
        }

        /// <summary>
        /// Stores up to 16 MB of text data (MySQL).
        /// </summary>
        /// <returns></returns>
        public static ColumnType MediumText()
        {
            return new ColumnType($"MEDIUMTEXT", defaultValue: "''", options: $"CHARACTER SET {GetCharset()} COLLATE {GetCollation()}");
        }

        /// <summary>
        /// Stores up to 4 GB of text data (MySQL).
        /// </summary>
        /// <returns></returns>
        public static ColumnType LongText()
        {
            return new ColumnType($"LONGTEXT", defaultValue: "''", options: $"CHARACTER SET {GetCharset()} COLLATE {GetCollation()}");
        }

#elif POSTGRES

        // Explicitly set UTF-8 collation for PostgreSQL to ensure case-insensitive comparison
        // consistent with MySQL's utf8mb4_unicode_ci behavior
        private static string GetCollation() => "en_US.utf8";

        /// <summary>
        /// You must not exceed 1GB size limit for values, one character takes 4 bytes.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static ColumnType VarChar(int length)
        {
            return new ColumnType($"VARCHAR({length})", defaultValue: "''", options: $"COLLATE \"{GetCollation()}\"");
        }

        /// <summary>
        /// Alias for <see cref="LongText"/> (1 GB, Postgres doesn't have a short text type).
        /// </summary>
        /// <returns></returns>
        public static ColumnType ShortText()
        {
            return LongText();
        }

        /// <summary>
        /// Alias for <see cref="LongText"/> (1 GB, Postgres doesn't have a short text type).
        /// </summary>
        /// <returns></returns>
        public static ColumnType MediumText()
        {
            return LongText();
        }

        /// <summary>
        /// Stores text data up to 1GB per value (Postgres).
        /// </summary>
        /// <returns></returns>
        public static ColumnType LongText()
        {
            return new ColumnType($"text", defaultValue: "''", options: $"COLLATE \"{GetCollation()}\"");
        }

#endif

#if MYSQL

        /// <summary>
        /// Variable-length binary data (number of bytes), max. 65,535 bytes (row size limit).
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static ColumnType VarBinary(int length)
        {
            return new ColumnType($"VARBINARY({length})", defaultValue: "''");
        }

        /// <summary>
        /// Stores up to 65,535 bytes of binary data. Separate from row size limit.
        /// </summary>
        /// <returns></returns>
        public static ColumnType ShortBinary()
        {
            return new ColumnType($"BLOB", defaultValue: "''");
        }

        /// <summary>
        /// Stores up to 16 MB of binary data.
        /// </summary>
        /// <returns></returns>
        public static ColumnType MediumBinary()
        {
            return new ColumnType($"MEDIUMBLOB", defaultValue: "''");
        }

        /// <summary>
        /// Stores up to 4 GB of binary data.
        /// </summary>
        /// <returns></returns>
        public static ColumnType LongBinary()
        {
            return new ColumnType($"LONGBLOB", defaultValue: "''");
        }

#elif POSTGRES

        /// <summary>
        /// Alias for <see cref="LongBinary"/> (1 GB, Postgres doesn't have a short binary type).
        /// </summary>
        /// <returns></returns>
        public static ColumnType ShortBinary()
        {
            return LongBinary();
        }

        /// <summary>
        /// Alias for <see cref="LongBinary"/> (1 GB, Postgres doesn't have a medium binary type).
        /// </summary>
        /// <returns></returns>
        public static ColumnType MediumBinary()
        {
            return LongBinary();
        }

        /// <summary>
        /// Stores binary data up to 1GB per value.
        /// </summary>
        /// <returns></returns>
        public static ColumnType LongBinary()
        {
            return new ColumnType($"BYTEA", defaultValue: "''");
        }

#endif
    }

}
