#if MYSQL56
namespace Quib.MySql56.Schema;
#elif MYSQL57
namespace Quib.MySql57.Schema;
#elif MYSQL80
namespace Quib.MySql80.Schema;
#elif MYSQL84
namespace Quib.MySql84.Schema;
#elif POSTGRES18
namespace Quib.Postgres18.Schema;
#endif


/// <summary>
/// Physical not-null column in a database table. Other than the <see cref="NotNullExpression{T}"/> it can be written to.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class NotNullColumn<T> : NotNullExpression<T>
    where T : struct, IExpressionType
{

    internal NotNullColumn(TableAlias table, string fieldname)
        : base(new DbColumnForReading(table, fieldname))
    {
    }

    internal DbColumnForReading Reader => (DbColumnForReading)base.SQL;

    internal DbColumnForWriting Writer => field ??= new DbColumnForWriting(Reader.Table, Reader.FieldName);

}
