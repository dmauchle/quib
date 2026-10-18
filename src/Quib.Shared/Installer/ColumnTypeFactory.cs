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

#if MYSQL

        /// <summary>
        /// You must not exceed 65,535-byte row size limit (all columns), one character takes 4 bytes (MySQL).
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static ColumnType VarChar(int length)
        {
            // Explicitly set UTF-8 charset and collation for MySQL to ensure full Unicode support
            // and case-insensitive comparison regardless of server/database default configuration
            return new ColumnType($"VARCHAR({length})", defaultValue: "''", options: "CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci");
        }

#elif POSTGRES

        /// <summary>
        /// You must not exceed 1GB size limit for values, one character takes 4 bytes (Postgres).
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static ColumnType VarChar(int length)
        {
            // Explicitly set UTF-8 collation for PostgreSQL to ensure case-insensitive comparison
            // consistent with MySQL's utf8mb4_unicode_ci behavior
            return new ColumnType($"VARCHAR({length})", defaultValue: "''", options: "COLLATE \"en_US.utf8\"");

        }

#endif


#if MYSQL
        /// <summary>
        /// Stores up to 65,535 bytes of text data. Separate from row size limit (MySQL).
        /// </summary>
        /// <returns></returns>
        public static ColumnType ShortText()
        {
            // Explicitly set UTF-8 charset and collation for MySQL to ensure full Unicode support
            // and case-insensitive comparison
            return new ColumnType($"TEXT", defaultValue: "''", options: "CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci");
        }

        /// <summary>
        /// Stores up to 16 MB of text data (MySQL).
        /// </summary>
        /// <returns></returns>
        public static ColumnType MediumText()
        {
            // Explicitly set UTF-8 charset and collation for MySQL
            return new ColumnType($"MEDIUMTEXT", defaultValue: "''", options: "CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci");
        }

        /// <summary>
        /// Stores up to 4 GB of text data (MySQL).
        /// </summary>
        /// <returns></returns>
        public static ColumnType LongText()
        {
            // Explicitly set UTF-8 charset and collation for MySQL
            return new ColumnType($"LONGTEXT", defaultValue: "''", options: "CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci");
        }

#elif POSTGRES

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
            // Explicitly set UTF-8 collation for PostgreSQL to ensure case-insensitive comparison
            return new ColumnType($"text", defaultValue: "''", options: "COLLATE \"en_US.utf8\"");
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
