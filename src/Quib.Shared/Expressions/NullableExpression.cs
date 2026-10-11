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
/// Typed SQL expression of type <typeparamref name="T"/> that may evaluate to NULL.
/// </summary>
/// <typeparam name="T"></typeparam>
public class NullableExpression<T> : IQueryExpression
    where T : struct, IExpressionType
{

    // This was intended to be an interface. Then the == operator would have to be declared as a static extension method on
    // the interface. But with the operator beeing declared as extesion method, this expression:
    // `DB.Select(x => x.Get(SQL.Parameter(true) == SQL.Parameter(true)))`
    // would not compile, because the compiler cannot infer the type of the expression. When we use an abstract class it works.

    // The == and != operators are declared on the class, not as extension operators. Class operators take precedence over
    // extension operators. Otherwise C# would use reference equality (returning bool) for operands of the same type.
    // Declared on the generic class, so only expressions of the same type can be compared. Different
    // types (e.g. DbInt and DbText) don't compile (intentional, because it wouldn't make sense and Postgres would also
    // throw an exception), as there's no matching operator and reference equality isn't allowed
    // between unrelated classes. Comparing a not-null with a nullable expression uses these operators, as the not-null
    // expression converts to its base class.

    // Designed as a wrapper around an IQueryExpression, so that the underlying expression can be of any type.

    internal NullableExpression(IQueryExpression sql)
    {
        this.SQL = sql;
    }


    internal IQueryExpression SQL { get; }

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write(SQL);
    }

    public static NullableExpression<DbBool> operator ==(NullableExpression<T> left, NullableExpression<T> right)
    {
        // In case nullable compiler errors are turned off in the consuming project.
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "=", right.SQL));
    }

    public static NullableExpression<DbBool> operator !=(NullableExpression<T> left, NullableExpression<T> right)
    {
        // In case nullable compiler errors are turned off in the consuming project.
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new NullableExpression<DbBool>(new BinaryOperator(left.SQL, "!=", right.SQL));
    }

}

#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
