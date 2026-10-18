using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.QueryTests;

[Collection(TestCollections.DATABASE)]
public class ObjectIdentityTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private class CompaniesTable(DbTable table) : IDbTable, IDbTable<CompaniesTable>
    {
        public static string TableName => "companies";
        public static CompaniesTable CreateFrom(DbTable table) => new CompaniesTable(table);

        public NotNullColumn<DbInt> ID => table.Column<DbInt>("id");
        public NotNullColumn<DbText> CompanyName => table.Column<DbText>("companyname");
        public NotNullColumn<DbText> City => table.Column<DbText>("city");
    }

    private class Company
    {
        public required string CompanyName { get; init; }
        public required string City { get; init; }
    }

    private class CompanyMapping(ProjectionBuilder rs, CompaniesTable company, bool withKey) : IReaderMapping<Company>
    {
        private readonly NotNullProjection<DbText> _companyName = rs.Get(company.CompanyName);
        private readonly NotNullProjection<DbText> _city = rs.Get(company.City);

        IProjectionField[] IReaderMapping<Company>.ObjectIdentity { get; } = withKey ? [rs.Get(company.ID)] : [];

        Company IReaderMapping<Company>.Map(ResultReader rs)
        {
            return new Company
            {
                CompanyName = rs.GetNullable(_companyName)!,
                City = rs.GetNullable(_city)!
            };
        }
    }

    private class EmployeesTable(DbTable table) : IDbTable, IDbTable<EmployeesTable>
    {
        public static string TableName => "employees";
        public static EmployeesTable CreateFrom(DbTable table) => new EmployeesTable(table);

        public NotNullColumn<DbText> Name => table.Column<DbText>("name");
        public NullableColumn<DbInt> CompanyId => table.NullableColumn<DbInt>("company_id");
    }

    private class EmployeesQuery(SelectQueryBuilder query) : ISelectContext<EmployeesQuery>
    {
        public static EmployeesQuery CreateFrom(SelectQueryBuilder query) => new EmployeesQuery(query);

        public EmployeesTable Employee => field ??= query.From<EmployeesTable>();
        public CompaniesTable Company => field ??= query.LeftOuterJoin<CompaniesTable>().On(c => c.ID == Employee.CompanyId);
    }

    private class Employee
    {
        public required string Name { get; init; }
        public required Company? Company { get; init; }
    }



    private class EmployeeMapping(ProjectionBuilder query, EmployeesQuery row, bool withKeys) : IReaderMapping<Employee>
    {

        public NotNullProjection<DbText> Name { get; } = query.Get(row.Employee.Name);
        public NullableProjection<DbInt> CompanyId { get; } = query.Get(row.Employee.CompanyId);

        public CompanyMapping Company { get; } = new CompanyMapping(query, row.Company, withKeys);

        public IProjectionField[] ObjectIdentity => [];

        public Employee Map(ResultReader rs)
        {
            return new Employee
            {
                Name = rs.Get(Name),
                Company = rs.MapNullable(Company)
            };
        }
    }



    protected override async ValueTask InitializeTestDatabase()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("companies")
                .CreateWithPrimaryKey("id", ColumnType.Int())
                .CreateColumn("companyname", ColumnType.VarChar(64).NotNull())
                .CreateColumn("city", ColumnType.VarChar(64).NotNull());
            install.Table("employees")
                .CreateWithPrimaryKey("name", ColumnType.VarChar(64))
                .CreateColumn("company_id", ColumnType.Int().Nullable())
                .CreateForeignKey("employee_company", ["company_id"], "companies", ["id"]);
        }

        await DB.InsertIntoTable("companies")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 1);
                query.Set(table.Column<DbText>("companyname"), "Microsoft");
                query.Set(table.Column<DbText>("city"), "Seattle");
            })
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbInt>("id"), 2);
                query.Set(table.Column<DbText>("companyname"), "Google");
                query.Set(table.Column<DbText>("city"), "Mountain View");
            })
            .Execute();
        await DB.InsertIntoTable("employees")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Alice");
                query.Set(table.NullableColumn<DbInt>("company_id"), 1);
            })
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Bob");
                query.Set(table.NullableColumn<DbInt>("company_id"), 2);
            })
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Charlie");
            })
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "David");
                query.Set(table.NullableColumn<DbInt>("company_id"), 1);
            })
            .Execute();
    }

    [Fact]
    public async Task PrimaryObjectIdentity_WithKey()
    {
        var rs = await DB.
            Select(query =>
            {
                var c1 = query.From<CompaniesTable>();
                var c2 = query.From<CompaniesTable>();
                return c1;
            })
            .Map((rs, x) => new CompanyMapping(rs, x, withKey: true))
            .FetchArray();

        Assert.Equal(4, rs.Length);  // Assert cross join

        var id1 = rs.Where(x => x.CompanyName == "Microsoft").ToArray();
        var id2 = rs.Where(x => x.CompanyName == "Google").ToArray();
        Assert.Equal(2, id1.Length);
        Assert.Equal(2, id2.Length);

        Assert.True(id1[0] == id1[1], "Object identity failed");
        Assert.True(id2[0] == id2[1], "Object identity failed");
        Assert.False(id1[0] == id2[0], "Object identity failed");
        Assert.False(id1[1] == id2[1], "Object identity failed");
    }

    [Fact]
    public async Task PrimaryObjectIdentity_WithoutKey()
    {
        var rs = await DB.
            Select(query =>
            {
                var c1 = query.From<CompaniesTable>();
                var c2 = query.From<CompaniesTable>();
                return c1;
            })
            .Map((rs, x) => new CompanyMapping(rs, x, withKey: false))
            .FetchArray();

        Assert.Equal(4, rs.Length);  // Assert cross join

        var id1 = rs.Where(x => x.CompanyName == "Microsoft").ToArray();
        var id2 = rs.Where(x => x.CompanyName == "Google").ToArray();
        Assert.Equal(2, id1.Length);
        Assert.Equal(2, id2.Length);

        Assert.False(id1[0] == id1[1], "Object identity failed");
        Assert.False(id2[0] == id2[1], "Object identity failed");
    }

    [Fact]
    public async Task JoinedObjectIdentity_WithKey()
    {
        var rs = await DB
            .Select<EmployeesQuery>()
            .Map((rs, x) => new EmployeeMapping(rs, x, withKeys: true))
            .FetchArray();

        Assert.Equal(4, rs.Length);

        var id1 = rs.Select(x => x.Company).Where(x => x?.CompanyName == "Microsoft").ToArray();
        Assert.Equal(2, id1.Length);
        Assert.True(id1[0] == id1[1], "Object identity failed");
    }

    [Fact]
    public async Task JoinedObjectIdentity_WithoutKey()
    {
        var rs = await DB
            .Select<EmployeesQuery>()
            .Map((rs, x) => new EmployeeMapping(rs, x, withKeys: false))
            .FetchArray();

        Assert.Equal(4, rs.Length);

        var id1 = rs.Select(x => x.Company).Where(x => x?.CompanyName == "Microsoft").ToArray();
        Assert.Equal(2, id1.Length);
        Assert.False(id1[0] == id1[1], "Object identity failed");
    }

}
