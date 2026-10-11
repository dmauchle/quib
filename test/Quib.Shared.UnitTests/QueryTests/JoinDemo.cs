using Quib.UnitTests.Infrastructure;
using Xunit.Abstractions;

namespace Quib.UnitTests.QueryTests;

[Collection(TestCollections.DATABASE)]
public class JoinDemo(DatabaseFixture env, ITestOutputHelper log) : DbTestBase(env, log)
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

    private class CompanyMapping(IProjectionBuilder rs, CompaniesTable company) : IReaderMapping<CompaniesTable, Company>
    {
        public static IReaderMapping<Company> CreateFrom(IProjectionBuilder rs, CompaniesTable company) => new CompanyMapping(rs, company);

        private readonly NotNullProjection<DbText> _companyName = rs.Get(company.CompanyName);
        private readonly NotNullProjection<DbText> _city = rs.Get(company.City);

        IProjectionField[] IReaderMapping<Company>.ObjectIdentity { get; } = [rs.Get(company.ID)];

        Company IReaderMapping<Company>.Map(ResultReader rs)
        {
            return new Company
            {
                CompanyName = rs.Get(_companyName),
                City = rs.Get(_city)
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


    private class EmployeeMapping(IProjectionBuilder query, EmployeesQuery row) : IReaderMapping<EmployeesQuery, Employee>
    {
        public static IReaderMapping<Employee> CreateFrom(IProjectionBuilder rs, EmployeesQuery query) => new EmployeeMapping(rs, query);

        public NotNullProjection<DbText> Name { get; } = query.Get(row.Employee.Name);

        public CompanyMapping Company { get; } = new CompanyMapping(query, row.Company);

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
    public async Task ISelectQueryBuilder_Style()
    {
        var rs = await DB
            .Select<EmployeesQuery>()
            .Where(x => x.Company.City == "Seattle")
            .OrderBy(x => [x.Employee.Name])
            .Map<EmployeeMapping, Employee>()
            .FetchArray();

        Assert.Equal(2, rs.Length);

    }

    [Fact]
    public async Task QueryBuilder_Style()
    {
        var rs = await DB
            .SelectFrom<EmployeesTable>()
            .Select((query, employee) =>
            {
                var company = query.LeftOuterJoin<CompaniesTable>().On(c => c.ID == employee.CompanyId);
                return new { employee, company };
            })
            .Where(x => x.company.City == "Seattle")
            .Project(x => x.employee.Name)
            .FetchArray();

        Assert.Equal(2, rs.Length);
    }

}
