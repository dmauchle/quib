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
/// Physical nullable column in a database table. Other than the <see cref="NullableExpression{T}"/> it can be written to.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class NullableColumn<T> : NullableExpression<T>
    where T : struct, IExpressionType
{

    internal NullableColumn(TableAlias table, string fieldname)
        : base(new DbColumnForReading(table, fieldname))
    {
    }

    internal DbColumnForReading Reader => (DbColumnForReading)base.SQL;

    internal DbColumnForWriting Writer => field ??= new DbColumnForWriting(Reader.Table, Reader.FieldName);

}
