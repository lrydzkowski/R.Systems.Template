using MongoDB.Driver;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Employees.Queries.GetEmployees;
using R.Systems.Template.Infrastructure.MongoDb.Common.Documents;
using R.Systems.Template.Infrastructure.MongoDb.Common.Extensions;
using R.Systems.Template.Infrastructure.MongoDb.Common.Mappers;

namespace R.Systems.Template.Infrastructure.MongoDb.Employees.Queries;

internal class GetEmployeesRepository : IGetEmployeesRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly IEmployeeMapper _employeeMapper;

    public GetEmployeesRepository(AppDbContext appDbContext, IEmployeeMapper employeeMapper)
    {
        _appDbContext = appDbContext;
        _employeeMapper = employeeMapper;
    }

    public string Version { get; } = Versions.V2;

    public async Task<ListInfo<Employee>> GetEmployeesAsync(
        ListParameters listParameters,
        CancellationToken cancellationToken
    )
    {
        ListInfo<EmployeeDocument> result = await GetEmployeesAsync(
            listParameters,
            null,
            cancellationToken
        );

        return new ListInfo<Employee>
        {
            Count = result.Count,
            Data = _employeeMapper.Map(result.Data)
        };
    }

    public async Task<ListInfo<Employee>> GetEmployeesAsync(
        ListParameters listParameters,
        long companyId,
        CancellationToken cancellationToken
    )
    {
        ListInfo<EmployeeDocument> result = await GetEmployeesAsync(
            listParameters,
            Builders<EmployeeDocument>.Filter.Eq(x => x.CompanyId, companyId),
            cancellationToken
        );

        return new ListInfo<Employee>
        {
            Count = result.Count,
            Data = _employeeMapper.Map(result.Data)
        };
    }

    private async Task<ListInfo<EmployeeDocument>> GetEmployeesAsync(
        ListParameters listParameters,
        FilterDefinition<EmployeeDocument>? initialFilter = null,
        CancellationToken cancellationToken = default
    )
    {
        IReadOnlyList<string> fieldsAvailableToFilter =
            [nameof(EmployeeDocument.FirstName), nameof(EmployeeDocument.LastName)];
        IReadOnlyList<string> fieldsAvailableToSort =
            [nameof(EmployeeDocument.Id), nameof(EmployeeDocument.FirstName), nameof(EmployeeDocument.LastName)];
        string defaultSortingFieldName = nameof(EmployeeDocument.Id);
        ListInfo<EmployeeDocument> result = await _appDbContext.Employees.GetDataAsync(
            listParameters,
            fieldsAvailableToFilter,
            fieldsAvailableToSort,
            defaultSortingFieldName,
            initialFilter,
            cancellationToken
        );

        return result;
    }
}
