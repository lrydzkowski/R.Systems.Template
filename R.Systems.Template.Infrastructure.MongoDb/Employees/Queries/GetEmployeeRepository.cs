using MongoDB.Driver;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Infrastructure.MongoDb.Common.Documents;
using R.Systems.Template.Infrastructure.MongoDb.Common.Mappers;

namespace R.Systems.Template.Infrastructure.MongoDb.Employees.Queries;

internal class GetEmployeeRepository : IGetEmployeeRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly IEmployeeMapper _employeeMapper;

    public GetEmployeeRepository(AppDbContext appDbContext, IEmployeeMapper employeeMapper)
    {
        _appDbContext = appDbContext;
        _employeeMapper = employeeMapper;
    }

    public string Version { get; } = Versions.V2;

    public async Task<Employee?> GetEmployeeAsync(Guid employeeId, CancellationToken cancellationToken)
    {
        FilterDefinition<EmployeeDocument> filter = Builders<EmployeeDocument>.Filter.Eq(x => x.Id, employeeId);
        Employee? employee = await GetEmployeeAsync(filter, cancellationToken);

        return employee;
    }

    public async Task<Employee?> GetEmployeeAsync(Guid companyId, Guid employeeId, CancellationToken cancellationToken)
    {
        FilterDefinitionBuilder<EmployeeDocument>? filterDefinitionBuilder = Builders<EmployeeDocument>.Filter;
        FilterDefinition<EmployeeDocument> filter = filterDefinitionBuilder.And(
            filterDefinitionBuilder.Eq(x => x.Id, employeeId),
            filterDefinitionBuilder.Eq(x => x.CompanyId, companyId)
        );
        Employee? employee = await GetEmployeeAsync(filter, cancellationToken);

        return employee;
    }

    private async Task<Employee?> GetEmployeeAsync(
        FilterDefinition<EmployeeDocument> filter,
        CancellationToken cancellationToken
    )
    {
        EmployeeDocument? document = await _appDbContext.Employees.Find(filter).FirstOrDefaultAsync(cancellationToken);
        if (document == null)
        {
            return null;
        }

        Employee employee = _employeeMapper.Map(document);

        return employee;
    }
}
