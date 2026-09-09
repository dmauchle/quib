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

    extension(IQueryExpression<DbIntNullable> value)
    {
        public static IQueryExpression<DbIntNullable> operator +(IQueryExpression<DbIntNullable> left, IQueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbIntNullable>(left, "+", right);
        }

        public static IQueryExpression<DbIntNullable> operator +(IQueryExpression<DbIntNullable> left, IQueryExpression<DbInt> right)
        {
            return left + right.AsNullable();
        }

        public static IQueryExpression<DbIntNullable> operator +(IQueryExpression<DbIntNullable> left, int right)
        {
            return left + SQL.NullableParameter(right);
        }

    }


}