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
        /// MySQL: you must not exceed 65,535-byte row size limit (all columns), one character takes 4 bytes.<br />
        /// Postgres: you must not exceed 1GB size limit for values, one character takes 4 bytes.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static ColumnType VarChar(int length)
        {
            return new ColumnType($"VARCHAR({length})", defaultValue: "''");
        }
    }

}
