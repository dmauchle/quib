#if MYSQL56
namespace Quib.MySql56.Expressions;
#elif MYSQL57
namespace Quib.MySql57.Expressions;
#elif MYSQL80
namespace Quib.MySql80.Expressions;
#elif MYSQL84
namespace Quib.MySql84.Expressions;
#elif POSTGRES18
namespace Quib.Postgres18.Expressions;
#endif


#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()

/// <summary>
/// SQL expression of type <typeparamref name="T"/> that is not NULL. Except when using it with an OUTER JOIN.
/// </summary>
/// <typeparam name="T"></typeparam>
public class NotNullExpression<T> : NullableExpression<T>
    where T : struct, IExpressionType
{

    // See NullableExpression<T> for why the operators are declared on the class. If both operands are not-null, these
    // operators are more specific than the ones of the base class, so the result is not-null too.

    internal NotNullExpression(IQueryExpression sql)
        : base(sql)
    {

    }

    public static NotNullExpression<DbBool> operator ==(NotNullExpression<T> left, NotNullExpression<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "=", right.SQL));
    }

    public static NotNullExpression<DbBool> operator !=(NotNullExpression<T> left, NotNullExpression<T> right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new NotNullExpression<DbBool>(new BinaryOperator(left.SQL, "!=", right.SQL));
    }

}

#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
