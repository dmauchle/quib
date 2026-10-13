namespace Quib.Postgres18.QueryBuilder;

internal static class SqlSyntax
{

    private const string IDENTIFIER_QUOTE = "\"";

    public static void WriteIdentifier(this QueryWriter writer, string identifier)
    {
        writer.Write(IDENTIFIER_QUOTE);
        writer.Write(identifier);
        writer.Write(IDENTIFIER_QUOTE);
    }

    public static string Identifier(string identifier)
    {
        return $"{IDENTIFIER_QUOTE}{identifier}{IDENTIFIER_QUOTE}";
    }

    /// <summary>
    /// Integer division truncating toward zero (like C#).
    /// </summary>
    public static string IntegerDivision()
    {
        return "/";
    }

}
