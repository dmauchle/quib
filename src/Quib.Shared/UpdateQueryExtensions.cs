#if MYSQL56
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

public static class UpdateQueryExtensions
{

    public static void Set<TQuery>(this TQuery query, DbColumn<DbText> column, string value)
        where TQuery : IUpdateQueryBuilder
    {
        query.Set(column, query.Parameters.Create(value));
    }

}