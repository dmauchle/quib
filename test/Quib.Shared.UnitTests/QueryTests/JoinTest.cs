using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.QueryTests;

[Collection(TestCollections.DATABASE)]
public class JoinTest(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
{

    private class CompaniesTable(DbTable table) : IDbTable, IDbTable<CompaniesTable>
    {
        public static string TableName => "companies";
        public static CompaniesTable CreateFrom(DbTable table) => new CompaniesTable(table);

        public NotNullColumn<DbInt> ID => table.Column<DbInt>("id");
        public NotNullColumn<DbText> CompanyName => table.Column<DbText>("companyname");
        public NotNullColumn<DbText> City => table.Column<DbText>("city");
    }

    private class EmployeesTable(DbTable table) : IDbTable, IDbTable<EmployeesTable>
    {
        public static string TableName => "employees";
        public static EmployeesTable CreateFrom(DbTable table) => new EmployeesTable(table);

        public NotNullColumn<DbText> Name => table.Column<DbText>("name");
        public NullableColumn<DbInt> CompanyId => table.NullableColumn<DbInt>("company_id");
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

    private class EmployeeMapping(ProjectionBuilder query, EmployeesTable row, CompaniesTable company) : IReaderMapping<Employee>
    {
        public NotNullProjection<DbText> Name { get; } = query.Get(row.Name);
        public NotNullProjection<DbText> CompanyName { get; } = query.Get(company.CompanyName);
        public NotNullProjection<DbText> City { get; } = query.Get(company.City);

        public IProjectionField[] ObjectIdentity => [];

        public Employee Map(ResultReader rs)
        {
            return new Employee
            {
                Name = rs.Get(Name),
                Company = rs.IsDBNull(CompanyName) ? null : new Company
                {
                    CompanyName = rs.Get(CompanyName),
                    City = rs.Get(City)
                }
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
    public async Task LeftOuterJoin()
    {
        var rs = await DB
            .Select(query =>
            {
                var employee = query.From<EmployeesTable>();
                var company = query.LeftOuterJoin<CompaniesTable>().On(x => x.ID == employee.CompanyId);
                return new { employee, company };
            })
            .Map((rs, x) => new EmployeeMapping(rs, x.employee, x.company))
            .FetchArray();

        Assert.Equal(4, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company is { CompanyName: "Google", City: "Mountain View" });
        Assert.Contains(rs, x => x.Name == "David" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Charlie" && x.Company is null);
    }

    [Fact]
    public async Task InnerJoin()
    {
        var rs = await DB
            .Select(query =>
            {
                var employee = query.From<EmployeesTable>();
                var company = query.InnerJoin<CompaniesTable>().On(x => x.ID == employee.CompanyId);
                return new { employee, company };
            })
            .Map((rs, x) => new EmployeeMapping(rs, x.employee, x.company))
            .FetchArray();

        Assert.Equal(3, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company is { CompanyName: "Google", City: "Mountain View" });
        Assert.Contains(rs, x => x.Name == "David" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
    }

    [Fact]
    public async Task RightOuterJoin()
    {
        var rs = await DB
            .Select(query =>
            {
                var company = query.From<CompaniesTable>();
                var employee = query.RightOuterJoin<EmployeesTable>().On(x => x.CompanyId == company.ID);
                return new { employee, company };
            })
            .Map((rs, x) => new EmployeeMapping(rs, x.employee, x.company))
            .FetchArray();

        Assert.Equal(4, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company is { CompanyName: "Google", City: "Mountain View" });
        Assert.Contains(rs, x => x.Name == "David" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Charlie" && x.Company is null);
    }

    [Fact]
    public async Task FromCrossJoin()
    {
        var rs = await DB
            .Select(query =>
            {
                var employee = query.From<EmployeesTable>();
                var company = query.From<CompaniesTable>();
                return new { employee, company };
            })
            .Where(x => x.company.ID == x.employee.CompanyId)
            .Map((rs, x) => new EmployeeMapping(rs, x.employee, x.company))
            .FetchArray();

        Assert.Equal(3, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company is { CompanyName: "Google", City: "Mountain View" });
        Assert.Contains(rs, x => x.Name == "David" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
    }

}