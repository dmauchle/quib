# SELECT Query JOINs

Example schema with a foreign key relation, domain model and mapping:

```csharp
public class CompaniesTable(DbTable table) : IDbTable, IDbTable<CompaniesTable>
{
    public static string TableName => "companies";
    public static CompaniesTable CreateFrom(DbTable table) => new CompaniesTable(table);

    public NotNullColumn<DbInt> ID => table.Column<DbInt>("id");
    public NotNullColumn<DbText> CompanyName => table.Column<DbText>("companyname");
    public NotNullColumn<DbText> City => table.Column<DbText>("city");
}

public class Company
{
    public required string CompanyName { get; init; }
    public required string City { get; init; }
}

public class CompanyMapping(IProjectionBuilder rs, CompaniesTable company) : IReaderMapping<CompaniesTable, Company>
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

public class EmployeesTable(DbTable table) : IDbTable, IDbTable<EmployeesTable>
{
    public static string TableName => "employees";
    public static EmployeesTable CreateFrom(DbTable table) => new EmployeesTable(table);

    public NotNullColumn<DbText> Name => table.Column<DbText>("name");
    public NullableColumn<DbInt> CompanyId => table.NullableColumn<DbInt>("company_id");
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

For reusable queries, we can define an `ISelectContext<T>`, which contains the joins and presents the joined tables:

```csharp
public class EmployeesQuery(SelectQueryBuilder query) : ISelectContext<EmployeesQuery>
{
    public static EmployeesQuery CreateFrom(SelectQueryBuilder query) => new EmployeesQuery(query);

    public EmployeesTable Employee => field ??= query.From<EmployeesTable>();
    public CompaniesTable Company => field ??= query.LeftOuterJoin<CompaniesTable>().On(c => c.ID == Employee.CompanyId);
}
```

Note the use of the `field ??=` pattern. This allows the joins to be performed only when they are actually used in the
query.

This example uses `query.LeftOuterJoin<T>()`. Of course, `query.RightOuterJoin<T>()` and `query.InnerJoin<T>()` are also 
available.

The mapping for this query can be defined as follows:

```csharp
public class EmployeeMapping(IProjectionBuilder query, EmployeesQuery row) : IReaderMapping<Employee>, IReaderMapping<EmployeesQuery, Employee>
{
    public static IReaderMapping<Employee> CreateFrom(IProjectionBuilder rs, EmployeesQuery query) => new EmployeeMapping(rs, query);

    public NotNullProjection<DbText> Name { get; } = query.Get(row.Employee.Name);

    public CompanyMapping Company { get; } = new CompanyMapping(query, row.Company);  // Reuse the CompanyMapping's projection

    public IProjectionField[] ObjectIdentity => [];

    public Employee Map(ResultReader rs)
    {
        return new Employee
        {
            Name = rs.Get(Name),
            Company = rs.MapNullable(Company)  // Map via CompanyMapping, may be null due to outer join
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
var rs = await db
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


## NULL with outer-joins

Note that columns of outer-joined tables keep their not-null type (e.g. `NotNullColumn<DbText>`), although they are 
`NULL` if there's no matching row (with `RightOuterJoin`, this applies to the columns of the `From` table). Reading 
such a `NULL` value with `rs.Get(...)` throws an exception. Use `rs.MapNullable(...)` to map the joined table to an 
object (it returns `null` if there's no matching row), or `rs.GetNullable(...)` to read single values:

```csharp
var rs = await db
    .Select<EmployeesQuery>()
    .Project((rs, x) => new { Name = rs.Get(x.Employee.Name), City = rs.Get(x.Company.City) })
    .FetchArray((rs, x) => new { Name = rs.Get(x.Name), City = rs.GetNullable(x.City) });  // City is string?
```