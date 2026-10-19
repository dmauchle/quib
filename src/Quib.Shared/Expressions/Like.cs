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


/// <summary>
/// LIKE operator with '!' as escape character. Case sensitivity is determined by the collation of the value expression.
/// </summary>
internal sealed class Like : IQueryExpression
{

    private const char ESCAPE = '!';

    private readonly IQueryExpression _value;
    private readonly IQueryExpression _pattern;

    public Like(IQueryExpression value, string pattern)
    {
        _value = value;
        _pattern = SQL.Parameter(pattern).SQL;
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
        query.Write("(");
        query.Write(_value);
        query.Write(" LIKE ");
        query.Write(_pattern);
        query.Write($" ESCAPE '{ESCAPE}')");
    }

    /// <summary>
    /// The term is automatically escaped so it never contains wildcards.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="term"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    public static IQueryExpression Contains(IQueryExpression value, string term, TextMatch match)
    {
        return new Like(Collate.Apply(value, match), $"%{EscapePattern(term)}%");
    }

    /// <summary>
    /// The term is automatically escaped so it never contains wildcards.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="term"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    public static IQueryExpression StartsWith(IQueryExpression value, string term, TextMatch match)
    {
        return new Like(Collate.Apply(value, match), $"{EscapePattern(term)}%");
    }

    /// <summary>
    /// The term is automatically escaped so it never contains wildcards.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="term"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    public static IQueryExpression EndsWith(IQueryExpression value, string term, TextMatch match)
    {
        return new Like(Collate.Apply(value, match), $"%{EscapePattern(term)}");
    }

    public static string EscapePattern(string pattern)
    {
        ArgumentNullException.ThrowIfNull(pattern);

        return pattern
            .Replace($"{ESCAPE}", $"{ESCAPE}{ESCAPE}")
            .Replace("%", $"{ESCAPE}%")
            .Replace("_", $"{ESCAPE}_");
    }

}
