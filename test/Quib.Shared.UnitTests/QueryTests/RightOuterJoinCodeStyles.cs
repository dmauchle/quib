using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.QueryTests;

[Collection(TestCollections.DATABASE)]
public class RightOuterJoinCodeStyles(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private class CompaniesTable(DbTable table) : IDbTable, IDbTableFactory<CompaniesTable>
    {
        public static string TableName => "companies";
        public static CompaniesTable Factory(DbTable table) => new CompaniesTable(table);

        public DbColumn<DbText> CompanyName => table.Column<DbText>("companyname");
        public DbColumn<DbText> City => table.Column<DbText>("city");
    }

    private class EmployeesTable(DbTable table) : IDbTable, IDbTableFactory<EmployeesTable>
    {
        public static string TableName => "employees";
        public static EmployeesTable Factory(DbTable table) => new EmployeesTable(table);

        public DbColumn<DbText> Name => table.Column<DbText>("name");
        public DbColumn<DbTextNullable> CompanyRef => table.Column<DbTextNullable>("company");
    }

    private class CompaniesQuery(SelectQueryBuilder query, CompaniesTable company) : ISelectQueryFactory<CompaniesQuery>
    {
        public static CompaniesQuery Factory(SelectQueryBuilder query) => new CompaniesQuery(query, query.From<CompaniesTable>());
        public static CompaniesQuery Factory(SelectQueryBuilder query, CompaniesTable company) => new CompaniesQuery(query, company);

        public CompaniesTable Company => company;
        public EmployeesTable Employee => field ??= query.RightOuterJoin<EmployeesTable>().On(e => e.CompanyRef == Company.CompanyName);

    }

    private class Employee
    {
        public required string Name { get; init; }
        public Company? Company { get; init; }
    }

    private class Company
    {
        public required string CompanyName { get; init; }
        public required string City { get; init; }
    }

    private class EmployeeMapping(ProjectionBuilder query, CompaniesQuery row) : IReaderMappingFactory<CompaniesQuery, Employee>
    {

        public static IReaderMapping<Employee> Factory(ProjectionBuilder query, CompaniesQuery user) => new EmployeeMapping(query, user);

        public ProjectedValue<DbText> Name { get; } = query.Get(row.Employee.Name);
        public CompanyMapping Company { get; } = new CompanyMapping(query, row.Company);

        public Employee Map(ResultReader rs)
        {

            return new Employee
            {
                Name = rs.Get(Name),
                Company = rs.IsDBNull(Company.CompanyName) ? null : Company.Map(rs)
            };
        }

    }

    private class CompanyMapping(ProjectionBuilder query, CompaniesTable row) : IReaderMappingFactory<CompaniesTable, Company>
    {
        public static IReaderMapping<Company> Factory(ProjectionBuilder query, CompaniesTable user) => new CompanyMapping(query, user);

        public ProjectedValue<DbText> CompanyName { get; } = query.Get(row.CompanyName);
        public ProjectedValue<DbText> City { get; } = query.Get(row.City);

        public Company Map(ResultReader rs)
        {
            return new Company
            {
                CompanyName = rs.Get(CompanyName),
                City = rs.Get(City)
            };
        }
    }


    protected override async ValueTask InitializeTestDatabase()
    {
        using (var install = new DbInstaller(DB.Connection))
        {
            install.Table("companies")
                .CreateWithPrimaryKey("companyname", ColumnType.VarChar(64))
                .AddColumn("city", ColumnType.VarChar(64).NotNull());
            install.Table("employees")
                .CreateWithPrimaryKey("name", ColumnType.VarChar(64))
                .AddColumn("company", ColumnType.VarChar(64).Nullable())
                .AddForeignKey("employee_company", ["company"], "companies", ["companyname"]);
        }

        await DB.InsertIntoTable("companies")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("companyname"), "Microsoft");
                query.Set(table.Column<DbText>("city"), "Seattle");
            })
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("companyname"), "Google");
                query.Set(table.Column<DbText>("city"), "Mountain View");
            })
            .Execute();
        await DB.InsertIntoTable("employees")
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Alice");
                query.Set(table.Column<DbTextNullable>("company"), "Microsoft");
            })
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Bob");
                query.Set(table.Column<DbTextNullable>("company"), "Google");
            })
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "Charlie");
            })
            .AddRow((query, table) =>
            {
                query.Set(table.Column<DbText>("name"), "David");
                query.Set(table.Column<DbTextNullable>("company"), "Microsoft");
            })
            .Execute();
    }

    [Fact]
    public async Task ThroughAnonymousType()
    {
        var rs = await DB
            .Select(x =>
            {
                var companies = x.FromTable("companies");
                var employees = x.RightOuterJoin("employees").On(e => e.Column<DbTextNullable>("company") == companies.Column<DbText>("companyname"));
                return new
                {
                    employee = employees,
                    company = companies
                };
            })
            .Project((rs, x) => new
            {
                Name = rs.Get(x.employee.Column<DbTextNullable>("name")),
                Company = rs.Get(x.company.Column<DbTextNullable>("companyname")),
                City = rs.Get(x.company.Column<DbTextNullable>("city"))
            })
            .FetchArray((rs, x) => new
            {
                Name = rs.Get(x.Name),
                Company = rs.Get(x.Company),
                City = rs.Get(x.City)
            });

        Assert.Equal(4, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company == "Microsoft" && x.City == "Seattle");
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company == "Google" && x.City == "Mountain View");
        Assert.Contains(rs, x => x.Name == "David" && x.Company == "Microsoft" && x.City == "Seattle");
        Assert.Contains(rs, x => x.Name == "Charlie" && x.Company is null && x.City is null);
    }

    [Fact]
    public async Task ThroughISelectQueryFactory()
    {
        var rs = await DB
            .Select<CompaniesQuery>()
            .Map<EmployeeMapping, Employee>()
            .FetchArray();

        AssertEmployeeRows(rs);
    }

    [Fact]
    public async Task ThroughFactoryMethod()
    {
        var rs = await DB
            .Select(CompaniesQuery.Factory)
            .Map(EmployeeMapping.Factory)
            .FetchArray();

        AssertEmployeeRows(rs);
    }

    private static void AssertEmployeeRows(Employee[] rs)
    {
        Assert.Equal(4, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company is { CompanyName: "Google", City: "Mountain View" });
        Assert.Contains(rs, x => x.Name == "David" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Charlie" && x.Company is null);
    }

}
