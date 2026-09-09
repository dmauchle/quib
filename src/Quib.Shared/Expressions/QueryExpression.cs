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
public abstract class QueryExpression : IQueryPart
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

    public static QueryExpression<DbBoolNullable> operator ==(QueryExpression left, QueryExpression right)
    {
        return new BinaryOperator<DbBoolNullable>(left, "=", right);
    }
    
    public static QueryExpression<DbBoolNullable> operator !=(QueryExpression left, QueryExpression right)
    {
        return new BinaryOperator<DbBoolNullable>(left, "!=", right);
    }

}


public abstract class QueryExpression<T> : QueryExpression, IQueryPart
    where T : IExpressionType
{

}

#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()