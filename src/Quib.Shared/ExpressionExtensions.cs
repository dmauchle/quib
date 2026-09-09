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


public static class ExpressionExtensions
{

    extension(IQueryExpression<DbInt> value)
    {

        public IQueryExpression<DbIntNullable> AsNullable()
        {
            return new ImplicitCast<DbIntNullable>(value);
        }


        public static IQueryExpression<DbInt> operator +(IQueryExpression<DbInt> left, IQueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbInt>(left, "+", right);
        }

        public static IQueryExpression<DbIntNullable> operator +(IQueryExpression<DbInt> left, IQueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbIntNullable>(left.AsNullable(), "+", right);
        }

    }

    extension(IQueryExpression<DbIntNullable> value)
    {
        public static IQueryExpression<DbIntNullable> operator +(IQueryExpression<DbIntNullable> left, IQueryExpression<DbIntNullable> right)
        {
            return new BinaryOperator<DbIntNullable>(left, "+", right);
        }

        public static IQueryExpression<DbIntNullable> operator +(IQueryExpression<DbIntNullable> left, IQueryExpression<DbInt> right)
        {
            return new BinaryOperator<DbIntNullable>(left, "+", right.AsNullable());
        }

    }

}