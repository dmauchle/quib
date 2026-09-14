# SELECT Query JOIN's

Example schema with a foreign key relation, domain model and mapping

```csharp
public class CompaniesTable(DbTable table) : IDbTable, IDbTableFactory<CompaniesTable>
{
    public static string TableName => "companies";
    public static CompaniesTable Factory(DbTable table) => new CompaniesTable(table);

    public DbColumn<DbInt> ID => table.Column<DbInt>("id");
    public DbColumn<DbText> CompanyName => table.Column<DbText>("companyname");
    public DbColumn<DbText> City => table.Column<DbText>("city");
}

public class Company
{
    public required string CompanyName { get; init; }
    public required string City { get; init; }
}

public class CompanyMapping(ProjectionBuilder rs, CompaniesTable company) : IReaderMappingFactory<CompaniesTable, Company>
{
    public static IReaderMapping<Company> Factory(ProjectionBuilder rs, CompaniesTable company) => new CompanyMapping(rs, company);

    private readonly ProjectedValue<DbText> _companyName = rs.Get(company.CompanyName);
    private readonly ProjectedValue<DbText> _city = rs.Get(company.City);

    IProjectedValue[] IReaderMapping<Company>.ObjectIdentity { get; } = [rs.Get(company.ID)];

    Company IReaderMapping<Company>.Map(ResultReader rs)
    {
        return new Company
        {
            CompanyName = rs.Get(_companyName),
            City = rs.Get(_city)
        };
    }
}

public class EmployeesTable(DbTable table) : IDbTable, IDbTableFactory<EmployeesTable>
{
    public static string TableName => "employees";
    public static EmployeesTable Factory(DbTable table) => new EmployeesTable(table);

    public DbColumn<DbText> Name => table.Column<DbText>("name");
    public DbColumn<DbIntNullable> CompanyId => table.Column<DbIntNullable>("company_id");
}

public class Employee
{
    public required string Name { get; init; }
    public required Company? Company { get; init; }
}

```

## Reusable Join Queries

Let's assume we want to map into this domain model: 

```csharp
public class Employee
{
    public required string Name { get; init; }
    public required Company? Company { get; init; }
}
```

For reusable queries, we can define an `ISelectQueryFactory<T>`, which contains the joins and presents the joined tables:

```csharp
public class EmployeesQuery(SelectQueryBuilder query) : ISelectQueryFactory<EmployeesQuery>
{
    public static EmployeesQuery Factory(SelectQueryBuilder query) => new EmployeesQuery(query);

    public EmployeesTable Employee => field ??= query.From<EmployeesTable>();
    public CompaniesTable Company => field ??= query.LeftOuterJoin<CompaniesTable>().On(c => c.ID == Employee.CompanyId);
}
```

Note the use of the `field ??=` pattern. This allows the joins only to be performed, when they're actually used in the
query.

This example uses a `query.LeftOuterJoin<T>`. Of course, `query.RightOuterJoin<T>()` and `query.InnerJoin<T>()` are also 
available.

This mapping for this query can be defined as follows:

```csharp
public class EmployeeMapping(ProjectionBuilder query, EmployeesQuery row) : IReaderMapping<Employee>, IReaderMappingFactory<EmployeesQuery, Employee>
{
    public static IReaderMapping<Employee> Factory(ProjectionBuilder rs, EmployeesQuery query) => new EmployeeMapping(rs, query);

    public ProjectedValue<DbText> Name { get; } = query.Get(row.Employee.Name);

    public CompanyMapping Company { get; } = new CompanyMapping(query, row.Company);  // Reuse the CompanyMapping's projection

    public IProjectedValue[] ObjectIdentity => [];

    public Employee Map(ResultReader rs)
    {
        return new Employee
        {
            Name = rs.Get(Name),
            Company = rs.MapNullable(Company)  // Map via CompanyMapping
        };
    }
}
```

Query example: 

```csharp
var rs = await db
    .Select<EmployeesQuery>()
    .Where(x => x.Company.City == "Seattle")
    .OrderBy(x => [x.Employee.Name])
    .Map<EmployeeMapping, Employee>()
    .FetchArray();
```


## Inline Joins

If you just want a quick join, you can do it like this and use it e.g. to filter the results by a column that is not
even in the result set:

```csharp
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
```