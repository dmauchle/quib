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
        public static ColumnType VarChar(int length)
        {
            return new ColumnType($"VARCHAR({length})", defaultValue: "''");
        }
    }

}
