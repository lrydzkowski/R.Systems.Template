using MongoDB.Driver;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using R.Systems.Template.Infrastructure.MongoDb.Common.Documents;
using R.Systems.Template.Infrastructure.MongoDb.Common.Extensions;

namespace R.Systems.Template.Infrastructure.MongoDb.Employees.Queries;

internal class GetEmployeesRepository : IGetEmployeesRepository
{
    private readonly AppDbContext _appDbContext;

    public GetEmployeesRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public string Version { get; } = Versions.V2;

    public async Task<ListInfo<Employee>> GetEmployeesAsync(
        ListParameters listParameters,
        CancellationToken cancellationToken
    )
    {
        ListInfo<Employee> result = await GetEmployeesAsync(
            listParameters,
            null,
            cancellationToken
        );

        return new ListInfo<Employee>
        {
            Count = result.Count,
            Data = result.Data
        };
    }

    public async Task<ListInfo<Employee>> GetEmployeesAsync(
        ListParameters listParameters,
        Guid companyId,
        CancellationToken cancellationToken
    )
    {
        ListInfo<Employee> result = await GetEmployeesAsync(
            listParameters,
            Builders<Employee>.Filter.Eq(x => x.CompanyId, companyId),
            cancellationToken
        );

        return new ListInfo<Employee>
        {
            Count = result.Count,
            Data = result.Data
        };
    }

    private async Task<ListInfo<Employee>> GetEmployeesAsync(
        ListParameters listParameters,
        FilterDefinition<Employee>? initialFilter = null,
        CancellationToken cancellationToken = default
    )
    {
        ProjectionDefinition<EmployeeDocument, Employee> projection = Builders<EmployeeDocument>.Projection.Expression(
            x => new Employee { EmployeeId = x.Id, FirstName = x.FirstName, LastName = x.LastName }
        );

        ListInfo<Employee> result = await _appDbContext.Employees.GetDataAsync(
            projection,
            listParameters,
            initialFilter,
            cancellationToken
        );

        return result;
    }
}
