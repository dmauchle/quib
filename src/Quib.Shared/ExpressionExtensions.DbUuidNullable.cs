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


public static partial class ExpressionExtensions
{

    extension(QueryExpression<DbUuidNullable> value)
    {

        public static QueryExpression<DbBoolNullable> operator ==(QueryExpression<DbUuidNullable> left, Guid right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBoolNullable> operator !=(QueryExpression<DbUuidNullable> left, Guid right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "!=", SQL.Parameter(right));
        }

        public static QueryExpression<DbBoolNullable> operator ==(QueryExpression<DbUuidNullable> left, QueryExpression<DbUuid> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "=", right);
        }

        public static QueryExpression<DbBoolNullable> operator !=(QueryExpression<DbUuidNullable> left, QueryExpression<DbUuid> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "!=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbUuidNullable> left, QueryExpression<DbUuidNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbUuidNullable> left, QueryExpression<DbUuid> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<", right);
        }

        public static QueryExpression<DbBoolNullable> operator <(QueryExpression<DbUuidNullable> left, Guid right)
        {
            return left < SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbUuidNullable> left, QueryExpression<DbUuidNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbUuidNullable> left, QueryExpression<DbUuid> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">", right);
        }

        public static QueryExpression<DbBoolNullable> operator >(QueryExpression<DbUuidNullable> left, Guid right)
        {
            return left > SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbUuidNullable> left, QueryExpression<DbUuidNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbUuidNullable> left, QueryExpression<DbUuid> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, "<=", right);
        }

        public static QueryExpression<DbBoolNullable> operator <=(QueryExpression<DbUuidNullable> left, Guid right)
        {
            return left <= SQL.Parameter(right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbUuidNullable> left, QueryExpression<DbUuidNullable> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbUuidNullable> left, QueryExpression<DbUuid> right)
        {
            return new BinaryOperator<DbBoolNullable>(left, ">=", right);
        }

        public static QueryExpression<DbBoolNullable> operator >=(QueryExpression<DbUuidNullable> left, Guid right)
        {
            return left >= SQL.Parameter(right);
        }

    }

}
