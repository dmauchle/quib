#if MYSQL56
namespace Quib.MySql56.QueryBuilder;
#elif MYSQL57
namespace Quib.MySql57.QueryBuilder;
#elif MYSQL80
namespace Quib.MySql80.QueryBuilder;
#elif MYSQL84
namespace Quib.MySql84.QueryBuilder;
#elif POSTGRES18
namespace Quib.Postgres18.QueryBuilder;
#endif


public sealed class SelectQueryBuilder
{

    private List<FromClause>? _from;
    private List<JoinClause>? _joins;
    private List<IQueryExpression>? _filters;
    private ISortExpression[]? _sort;

    internal SelectQueryBuilder()
    {
    }

    internal FromClause AddFromClause(ITableOrSubquery table)
    {
        var index = GetNextTableIndex();
        var from = new FromClause(table, new TableAlias(index));
        (_from ??= []).Add(from);
        return from;
    }

    internal JoinClause AddJoinClause(ITableOrSubquery table, string joinType)
    {
        var index = GetNextTableIndex();
        var join = new JoinClause(joinType, table, new TableAlias(index));
        (_joins ??= []).Add(join);
        return join;
    }

    private int GetNextTableIndex()
    {
        return (_from?.Count ?? 0) + (_joins?.Count ?? 0);
    }

    internal void AddFilter(IQueryExpression filter)
    {
        (_filters ??= []).Add(filter);
    }

    internal void SetOrder(ISortExpression[] sort)
    {
        _sort = sort;
    }

    public DbTable FromTable(string tablename)
    {
        var table = new TableReference(tablename);
        var from = this.AddFromClause(table);
        var accessor = new DbTable(from.Alias);
        return accessor;
    }

    public T From<T>() where T : IDbTable, IDbTable<T>
    {
        return this.From(T.CreateFrom);
    }

    public T From<T>(Func<DbTable, T> tableFactory) where T : IDbTable
    {
        var table = this.FromTable(T.TableName);
        return tableFactory(table);
    }

    public JoinBuilder<DbTable> InnerJoin(string tablename)
    {
        var table = new TableReference(tablename);
        var join = this.AddJoinClause(table, "INNER JOIN");
        var accessor = new DbTable(join.Alias);
        return new JoinBuilder<DbTable>(join, accessor);
    }

    public JoinBuilder<T> InnerJoin<T>(Func<DbTable, T> tableFactory)
        where T : IDbTable
    {
        return this.InnerJoin(T.TableName).Select(tableFactory);
    }

    public JoinBuilder<T> InnerJoin<T>()
      where T : IDbTable, IDbTable<T>
    {
        return this.InnerJoin(T.CreateFrom);
    }

    public JoinBuilder<DbTable> LeftOuterJoin(string tablename)
    {
        var table = new TableReference(tablename);
        var join = this.AddJoinClause(table, "LEFT OUTER JOIN");
        var accessor = new DbTable(join.Alias);
        return new JoinBuilder<DbTable>(join, accessor);
    }

    public JoinBuilder<T> LeftOuterJoin<T>(Func<DbTable, T> tableFactory)
        where T : IDbTable
    {
        return this.LeftOuterJoin(T.TableName).Select(tableFactory);
    }

    public JoinBuilder<T> LeftOuterJoin<T>()
      where T : IDbTable, IDbTable<T>
    {
        return this.LeftOuterJoin(T.CreateFrom);
    }

    public JoinBuilder<DbTable> RightOuterJoin(string tablename)
    {
        var table = new TableReference(tablename);
        var join = this.AddJoinClause(table, "RIGHT OUTER JOIN");
        var accessor = new DbTable(join.Alias);
        return new JoinBuilder<DbTable>(join, accessor);
    }

    public JoinBuilder<T> RightOuterJoin<T>(Func<DbTable, T> tableFactory)
        where T : IDbTable
    {
        return this.RightOuterJoin(T.TableName).Select(tableFactory);
    }

    public JoinBuilder<T> RightOuterJoin<T>()
      where T : IDbTable, IDbTable<T>
    {
        return this.RightOuterJoin(T.CreateFrom);
    }

    internal void ToSql(QueryWriter query)
    {
        if (_from != null)
        {
            query.Write("FROM ");

            var first = true;
            foreach (var from in _from)
            {
                if (!first)
                    query.Write(", ");
                query.Write(from);
                first = false;
            }
        }

        if (_joins != null)
        {
            foreach (var join in _joins)
            {
                query.Write(" ");
                query.Write(join);
            }
        }

        if (_filters != null)
        {
            query.Write(" WHERE ");

            var first = true;
            foreach (var filter in _filters)
            {
                if (!first)
                    query.Write(" AND ");
                query.Write(filter);
                first = false;
            }
        }

        if (_sort != null)
        {
            query.Write(" ORDER BY ");

            var first = true;
            foreach (var sort in _sort)
            {
                if (!first)
                    query.Write(", ");
                query.Write(sort);
                first = false;
            }
        }
    }

}