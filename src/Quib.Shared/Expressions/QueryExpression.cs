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
/// Base class for all expressions in a SQL query, including columns, parameters, operators and SQL functions. 
/// Whenever possible <see cref="QueryExpression{T}"/> is used to denote the type of the expression.
/// </summary>
public abstract class QueryExpression : IQueryPart, ISortExpression
{

    // This was intended to be an interface. Then the == operator would have to be declared as a static extension method on
    // the interface. But with the operator beeing declared as extesion method, this expression:
    // `DB.Select(x => x.Get(SQL.Parameter(true) == SQL.Parameter(true)))`
    // would not compile, because the compiler cannot infer the type of the expression. When we use an abstract class it works.

    internal QueryExpression()
    {

    }

    internal abstract void ToSql(QueryWriter query);

    void IQueryPart.ToSql(QueryWriter query)
    {
        this.ToSql(query);
    }

}

/// <summary>
/// Type safe variant of <see cref="QueryExpression"/>, denoting the type of the expression.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class QueryExpression<T> : QueryExpression, IQueryPart
    where T : IExpressionType
{

    // The == and != operators are declared on the class, not as extension operators. Class operators take precedence over
    // extension operators. Otherwise C# would use reference equality (returning bool) for operands of the same type.
    // Declared here and not on the non-generic base class, so only expressions of the same type can be compared. Different
    // types (e.g. DbInt and DbText) don't compile (intentional, because it wouldn't make sense and Postgres would also
    // throw an exception), as there's no matching operator and reference equality isn't allowed between unrelated classes.
    // Not-null and nullable variants of a type are compared by extension operators.

    public static QueryExpression<DbBoolNullable> operator ==(QueryExpression<T> left, QueryExpression<T> right)
    {
        return new BinaryOperator<DbBoolNullable>(left, "=", right);
    }

    public static QueryExpression<DbBoolNullable> operator !=(QueryExpression<T> left, QueryExpression<T> right)
    {
        return new BinaryOperator<DbBoolNullable>(left, "!=", right);
    }

}

#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()