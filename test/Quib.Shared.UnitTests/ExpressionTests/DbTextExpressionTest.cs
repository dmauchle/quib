using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.ExpressionTests;

[Collection(TestCollections.DATABASE)]
public class DbTextExpressionTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    [Fact]
    public async Task Parameter()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello"))).FetchOne();
        Assert.Equal("Hello", rs.Result);
    }

    [Fact]
    public async Task Concat()
    {
        var left = SQL.Parameter("Hello");
        var right = " World";
        var rs = await DB.SelectValues(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Equal("Hello World", rs.Result);
    }

    [Fact]
    public async Task ConcatWithNullable()
    {
        var left = SQL.Parameter("Hello");
        var right = SQL.NullableParameter(" World");
        var rs = await DB.SelectValues(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Equal("Hello World", rs.Result);
    }

    [Fact]
    public async Task ConcatWithNull()
    {
        var left = SQL.Parameter("Hello");
        var right = SQL.NullableParameter((string?)null);
        var rs = await DB.SelectValues(x => x.Get(SQL.Concat(left, right))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") == SQL.Parameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") == SQL.NullableParameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equal_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") == SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task Equal_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") == "Hello")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") != SQL.Parameter("World"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") != SQL.NullableParameter("World"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") != SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task NotEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") != "World")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") < SQL.Parameter("World"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") < SQL.NullableParameter("World"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") < SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") < "World")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") <= SQL.Parameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") <= SQL.NullableParameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") <= SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task LessOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") <= "Hello")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("World") > SQL.Parameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("World") > SQL.NullableParameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("World") > SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterThan_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("World") > "Hello")).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") >= SQL.Parameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") >= SQL.NullableParameter("Hello"))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithNullableParameterRight_Null()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") >= SQL.NullableParameter((string?)null))).FetchOne();
        Assert.Null(rs.Result);
    }

    [Fact]
    public async Task GreaterOrEqual_WithValue_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello") >= "Hello")).FetchOne();
        Assert.True(rs.Result);
    }


    // Text functions (Contains, StartsWith, EndsWith, Equals)
    // Comparing parameters only uses the collation of the connection for TextMatch.ColumnDefault (MySQL: case-insensitive,
    // Postgres: case-sensitive). See VarCharTest/TextTest for columns.

#if POSTGRES
    protected override ValueTask InitializeTestDatabase()
    {
        using (var install = new DbInstaller(DB.Connection))
            install.InstallTextSearchExtensions();
        return ValueTask.CompletedTask;
    }
#endif

    [Fact]
    public async Task Contains_Exact_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello World").Contains("lo W", TextMatch.Exact))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Contains_Exact_False()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello World").Contains("xyz", TextMatch.Exact))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Contains_Exact_DifferentCase_False()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello World").Contains("hello", TextMatch.Exact))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Contains_Exact_DifferentAccent_False()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Café").Contains("Cafe", TextMatch.Exact))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Contains_IgnoreCaseAndAccents_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Café Müller").Contains("MULLER", TextMatch.IgnoreCaseAndAccents))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Contains_IgnoreCaseAndAccents_False()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Café Müller").Contains("xyz", TextMatch.IgnoreCaseAndAccents))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Contains_ColumnCollation_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello World").Contains("lo W", TextMatch.ColumnDefault))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Contains_ColumnCollation_DifferentCase()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello World").Contains("hello", TextMatch.ColumnDefault))).FetchOne();
#if MYSQL
        Assert.True(rs.Result);
#elif POSTGRES
        Assert.False(rs.Result);
#endif
    }

    [Fact]
    public async Task Contains_EmptyTerm_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello").Contains("", TextMatch.Exact))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Contains_PercentInTerm_MatchedLiterally()
    {
        var rsMatch = await DB.SelectValues(x => x.Get(SQL.Parameter("50% off").Contains("0%", TextMatch.Exact))).FetchOne();
        var rsNoMatch = await DB.SelectValues(x => x.Get(SQL.Parameter("500 off").Contains("0%", TextMatch.Exact))).FetchOne();
        Assert.True(rsMatch.Result);
        Assert.False(rsNoMatch.Result);
    }

    [Fact]
    public async Task Contains_UnderscoreInTerm_MatchedLiterally()
    {
        var rsMatch = await DB.SelectValues(x => x.Get(SQL.Parameter("a_b").Contains("a_", TextMatch.Exact))).FetchOne();
        var rsNoMatch = await DB.SelectValues(x => x.Get(SQL.Parameter("abb").Contains("a_", TextMatch.Exact))).FetchOne();
        Assert.True(rsMatch.Result);
        Assert.False(rsNoMatch.Result);
    }

    [Fact]
    public async Task Contains_EscapeCharacterInTerm_MatchedLiterally()
    {
        // The exclamation mark is used as escape character internally
        var rsMatch = await DB.SelectValues(x => x.Get(SQL.Parameter("Hi!%").Contains("!%", TextMatch.Exact))).FetchOne();
        var rsNoMatch = await DB.SelectValues(x => x.Get(SQL.Parameter("Hi%").Contains("!%", TextMatch.Exact))).FetchOne();
        Assert.True(rsMatch.Result);
        Assert.False(rsNoMatch.Result);
    }

    [Fact]
    public async Task StartsWith_Exact_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello World").StartsWith("Hel", TextMatch.Exact))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task StartsWith_Exact_False()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello World").StartsWith("World", TextMatch.Exact))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task StartsWith_IgnoreCaseAndAccents_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Éclair").StartsWith("ecl", TextMatch.IgnoreCaseAndAccents))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task StartsWith_IgnoreCaseAndAccents_False()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Éclair").StartsWith("lair", TextMatch.IgnoreCaseAndAccents))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task EndsWith_Exact_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello World").EndsWith("rld", TextMatch.Exact))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task EndsWith_Exact_False()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Hello World").EndsWith("Hello", TextMatch.Exact))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task EndsWith_IgnoreCaseAndAccents_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Crème Brûlée").EndsWith("BRULEE", TextMatch.IgnoreCaseAndAccents))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task EndsWith_IgnoreCaseAndAccents_False()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Crème Brûlée").EndsWith("creme", TextMatch.IgnoreCaseAndAccents))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Equals_Exact_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Müller").Equals("Müller", TextMatch.Exact))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equals_Exact_False()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Müller").Equals("Meier", TextMatch.Exact))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Equals_Exact_DifferentCase_False()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Müller").Equals("müller", TextMatch.Exact))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Equals_IgnoreCaseAndAccents_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Müller").Equals("MULLER", TextMatch.IgnoreCaseAndAccents))).FetchOne();
        Assert.True(rs.Result);
    }

    [Fact]
    public async Task Equals_IgnoreCaseAndAccents_False()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Müller").Equals("Muller AG", TextMatch.IgnoreCaseAndAccents))).FetchOne();
        Assert.False(rs.Result);
    }

    [Fact]
    public async Task Equals_ColumnCollation_True()
    {
        var rs = await DB.SelectValues(x => x.Get(SQL.Parameter("Müller").Equals("Müller", TextMatch.ColumnDefault))).FetchOne();
        Assert.True(rs.Result);
    }

}
