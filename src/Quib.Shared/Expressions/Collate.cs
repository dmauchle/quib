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
/// Applies a collation to a text expression, e.g. for case- and accent-insensitive comparisons.
/// </summary>
internal sealed class Collate : IQueryExpression
{

    private readonly IQueryExpression _value;
    private readonly string _collation;

    private Collate(IQueryExpression value, string collation)
    {
        _value = value;
        _collation = collation;
    }

    void IQueryPart.ToSql(QueryWriter query)
    {
#if MYSQL
        // NULL (literal or parameter) has the binary charset in MySQL, which doesn't accept text collations. Converting
        // to utf8mb4 first works for any value (no-op for text columns, as they are always utf8mb4).
        query.Write("(CONVERT(");
        query.Write(_value);
        query.Write(" USING utf8mb4) COLLATE ");
#elif POSTGRES
        query.Write("(");
        query.Write(_value);
        query.Write(" COLLATE ");
#endif
        query.Write(_collation);
        query.Write(")");
    }

    public static IQueryExpression Apply(IQueryExpression value, TextMatch match)
    {
        return match switch
        {
            TextMatch.ColumnDefault => value,
#if MYSQL56 || MYSQL57
            TextMatch.Exact => new Collate(value, "utf8mb4_bin"),
            TextMatch.IgnoreCaseAndAccents => new Collate(value, "utf8mb4_unicode_ci"),
#elif MYSQL
            TextMatch.Exact => new Collate(value, "utf8mb4_0900_as_cs"),
            TextMatch.IgnoreCaseAndAccents => new Collate(value, "utf8mb4_0900_ai_ci"),
#elif POSTGRES
            // Any deterministic collation compares exactly. The same as for columns created by Quib, so indexes can be used.
            TextMatch.Exact => new Collate(value, "\"en_US.utf8\""),
            // Created by DbInstaller.InstallTextSearchExtensions()
            TextMatch.IgnoreCaseAndAccents => new Collate(value, "quib_ignore_case_accents"),
#endif
            _ => throw new ArgumentOutOfRangeException(nameof(match), match, null)
        };
    }

}
