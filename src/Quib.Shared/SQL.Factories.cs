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

public static partial class SQL
{

    public static readonly DbNull Null = DbNull.Value;

    public static ISortExpression Sort(QueryExpression value, Sort direction)
    {
        return new SortClause(value, direction);
    }

    public static QueryExpression<DbInt> Constant(int value)
    {
        return new Constant<DbInt>(value.ToString());
    }

    public static QueryExpression<DbBool> Constant(bool value)
    {
        return new Constant<DbBool>(value ? "true" : "false");
    }

    public static QueryExpression<DbText> Parameter(string value)
    {
        return new QueryParameter<DbText>(value);
    }

    public static QueryExpression<DbTextNullable> NullableParameter(string? value)
    {
        return new QueryParameter<DbTextNullable>(value);
    }

    public static QueryExpression<DbInt> Parameter(int value)
    {
        return new QueryParameter<DbInt>(value);
    }

    public static QueryExpression<DbIntNullable> NullableParameter(int? value)
    {
        return new QueryParameter<DbIntNullable>(value);
    }

    public static QueryExpression<DbBool> Parameter(bool value)
    {
        return new QueryParameter<DbBool>(value);
    }

    public static QueryExpression<DbBoolNullable> NullableParameter(bool? value)
    {
        return new QueryParameter<DbBoolNullable>(value);
    }


    public static QueryExpression<DbUuid> Parameter(Guid value)
    {
#if MYSQL
        // MySQL stores UUIDs as CHAR(36): pass the lowercase "D" format to keep text comparison and V7 sort order consistent ("D" would be the default anyway)
        return new QueryParameter<DbUuid>(value.ToString("D"));
#elif POSTGRES
        return new QueryParameter<DbUuid>(value);
#endif
    }

    public static QueryExpression<DbUuidNullable> NullableParameter(Guid? value)
    {
#if MYSQL
        // MySQL stores UUIDs as CHAR(36): pass the lowercase "D" format to keep text comparison and V7 sort order consistent ("D" would be the default anyway)
        return new QueryParameter<DbUuidNullable>(value?.ToString("D"));
#elif POSTGRES
        return new QueryParameter<DbUuidNullable>(value);
#endif
    }

}