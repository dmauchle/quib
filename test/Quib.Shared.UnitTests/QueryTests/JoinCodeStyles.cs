using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.QueryTests;

[Collection(TestCollections.DATABASE)]
public class JoinCodeStyles(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
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

    private class EmployeesQuery(EmployeesTable employee, CompaniesTable company)
    {
        public EmployeesTable Employee => employee;
        public CompaniesTable Company => company;
    }

    private class Employee
    {
        public required string Name { get; init; }
        public required Company? Company { get; init; }
    }

    private class Company
    {
        public required string CompanyName { get; init; }
        public required string City { get; init; }
    }

    private class EmployeeMapping(ProjectionBuilder query, EmployeesQuery row) : IReaderMapping<Employee>, IReaderMappingFactory<EmployeesQuery, Employee>
    {

        public static IReaderMapping<Employee> Factory(ProjectionBuilder rs, EmployeesQuery query) => new EmployeeMapping(rs, query);

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

    /// <summary>
    /// Do the join through table name
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task InnerJoin_ThroughTableName()
    {
        var rs = await DB
            .Select(x =>
            {
                var employees = x.FromTable("employees");
                var companies = x.InnerJoin("companies").On(c => c.Column<DbText>("companyname") == employees.Column<DbTextNullable>("company"));
                return new
                {
                    employee = employees,
                    company = companies
                };
            })
            .Project((rs, x) => new
            {
                Name = rs.Get(x.employee.Column<DbText>("name")),
                Company = rs.Get(x.company.Column<DbText>("companyname")),
                City = rs.Get(x.company.Column<DbText>("city"))
            })
            .FetchArray((rs, x) => new
            {
                Name = rs.Get(x.Name),
                Company = rs.Get(x.Company),
                City = rs.Get(x.City)
            });

        Assert.Equal(3, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company == "Microsoft" && x.City == "Seattle");
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company == "Google" && x.City == "Mountain View");
        Assert.Contains(rs, x => x.Name == "David" && x.Company == "Microsoft" && x.City == "Seattle");
    }

    /// <summary>
    /// Do the join through <see cref="IDbTableFactory{T}"/>
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task InnerJoin_ThroughIDbTableFactory()
    {
        var rs = await DB
            .Select(query =>
            {
                var employee = query.From<EmployeesTable>();
                var company = query.InnerJoin<CompaniesTable>().On(c => c.CompanyName == employee.CompanyRef);
                return new EmployeesQuery(employee, company);
            })
            .Map<EmployeeMapping, Employee>()
            .FetchArray();

        Assert.Equal(3, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company is { CompanyName: "Google", City: "Mountain View" });
        Assert.Contains(rs, x => x.Name == "David" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
    }

    /// <summary>
    /// Do the join through <see cref="IDbTable"/>
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task InnerJoin_ThroughIDbTable()
    {
        var rs = await DB
            .Select(query =>
            {
                var employee = query.From(x => new EmployeesTable(x));
                var company = query.InnerJoin(x => new CompaniesTable(x)).On(c => c.CompanyName == employee.CompanyRef);
                return new EmployeesQuery(employee, company);
            })
            .Map<EmployeeMapping, Employee>()
            .FetchArray();

        Assert.Equal(3, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company is { CompanyName: "Google", City: "Mountain View" });
        Assert.Contains(rs, x => x.Name == "David" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
    }

    /// <summary>
    /// Do the join through table name
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task LeftOuterJoin_ThroughTableName()
    {
        var rs = await DB
            .Select(x =>
            {
                var employees = x.FromTable("employees");
                var companies = x.LeftOuterJoin("companies").On(c => c.Column<DbText>("companyname") == employees.Column<DbTextNullable>("company"));
                return new
                {
                    employee = employees,
                    company = companies
                };
            })
            .Project((rs, x) => new
            {
                Name = rs.Get(x.employee.Column<DbText>("name")),
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

    /// <summary>
    /// Do the join through <see cref="IDbTableFactory{T}"/>
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task LeftOuterJoin_ThroughIDbTableFactory()
    {
        var rs = await DB
            .Select(query =>
            {
                var employee = query.From<EmployeesTable>();
                var company = query.LeftOuterJoin<CompaniesTable>().On(c => c.CompanyName == employee.CompanyRef);
                return new EmployeesQuery(employee, company);
            })
            .Map<EmployeeMapping, Employee>()
            .FetchArray();

        Assert.Equal(4, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company is { CompanyName: "Google", City: "Mountain View" });
        Assert.Contains(rs, x => x.Name == "David" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Charlie" && x.Company is null);
    }

    /// <summary>
    /// Do the join through <see cref="IDbTable"/>
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task LeftOuterJoin_ThroughIDbTable()
    {
        var rs = await DB
            .Select(query =>
            {
                var employee = query.From(x => new EmployeesTable(x));
                var company = query.LeftOuterJoin(x => new CompaniesTable(x)).On(c => c.CompanyName == employee.CompanyRef);
                return new EmployeesQuery(employee, company);
            })
            .Map<EmployeeMapping, Employee>()
            .FetchArray();

        Assert.Equal(4, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company is { CompanyName: "Google", City: "Mountain View" });
        Assert.Contains(rs, x => x.Name == "David" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Charlie" && x.Company is null);
    }

    /// <summary>
    /// Do the join through table name
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task RightOuterJoin_ThroughTableName()
    {
        var rs = await DB
            .Select(x =>
            {
                var company = x.FromTable("companies");
                var employee = x.RightOuterJoin("employees").On(c => c.Column<DbText>("company") == company.Column<DbTextNullable>("companyname"));
                return new
                {
                    employee,
                    company
                };
            })
            .Project((rs, x) => new
            {
                Name = rs.Get(x.employee.Column<DbText>("name")),
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

    /// <summary>
    /// Do the join through <see cref="IDbTableFactory{T}"/>
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task RightOuterJoin_ThroughIDbTableFactory()
    {
        var rs = await DB
            .Select(query =>
            {
                var company = query.From<CompaniesTable>();
                var employee = query.RightOuterJoin<EmployeesTable>().On(c => c.CompanyRef == company.CompanyName);
                return new EmployeesQuery(employee, company);
            })
            .Map<EmployeeMapping, Employee>()
            .FetchArray();

        Assert.Equal(4, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company is { CompanyName: "Google", City: "Mountain View" });
        Assert.Contains(rs, x => x.Name == "David" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Charlie" && x.Company is null);
    }

    /// <summary>
    /// Do the join through <see cref="IDbTable"/>
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task RightOuterJoin_ThroughIDbTable()
    {
        var rs = await DB
            .Select(query =>
            {
                var company = query.From(x => new CompaniesTable(x));
                var employee = query.RightOuterJoin(x => new EmployeesTable(x)).On(e => e.CompanyRef == company.CompanyName);
                return new EmployeesQuery(employee, company);
            })
            .Map<EmployeeMapping, Employee>()
            .FetchArray();

        Assert.Equal(4, rs.Length);
        Assert.Contains(rs, x => x.Name == "Alice" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Bob" && x.Company is { CompanyName: "Google", City: "Mountain View" });
        Assert.Contains(rs, x => x.Name == "David" && x.Company is { CompanyName: "Microsoft", City: "Seattle" });
        Assert.Contains(rs, x => x.Name == "Charlie" && x.Company is null);
    }

}
