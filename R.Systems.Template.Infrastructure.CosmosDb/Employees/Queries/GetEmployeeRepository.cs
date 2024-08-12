using System.Net;
using Microsoft.Azure.Cosmos;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Employees.Queries.GetEmployee;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Items;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Mappers;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Services;

namespace R.Systems.Template.Infrastructure.CosmosDb.Employees.Queries;

internal class GetEmployeeRepository : IGetEmployeeRepository
{
    private readonly AppDbContext _appDbContext;
    private readonly IEmployeeMapper _employeeMapper;

    public GetEmployeeRepository(AppDbContext appDbContext, IEmployeeMapper employeeMapper)
    {
        _appDbContext = appDbContext;
        _employeeMapper = employeeMapper;
    }

    public string Version { get; } = Versions.V4;

    public async Task<Employee?> GetEmployeeAsync(long employeeId, CancellationToken cancellationToken)
    {
        using ResponseMessage responseMessage = await _appDbContext.EmployeesContainers.ReadItemStreamAsync(
            employeeId.ToString(),
            new PartitionKey(employeeId.ToString()),
            cancellationToken: cancellationToken
        );
        if (responseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        responseMessage.EnsureSuccessStatusCode();

        CosmosSystemTextJsonSerializer serializer = new();
        EmployeeItem employeeItem = serializer.FromStream<EmployeeItem>(responseMessage.Content);

        Employee employee = _employeeMapper.Map(employeeItem);

        return employee;
    }

    public Task<Employee?> GetEmployeeAsync(long companyId, long employeeId, CancellationToken cancellationToken)
    {
        IOrderedQueryable<EmployeeItem> queryable =
            _appDbContext.EmployeesContainers.GetItemLinqQueryable<EmployeeItem>();

        EmployeeItem? employeeItem =
            queryable.Where(x => x.CompanyId == companyId.ToString() && x.Id == employeeId.ToString()).FirstOrDefault();
        if (employeeItem is null)
        {
            return Task.FromResult((Employee?)null);
        }

        Employee employee = _employeeMapper.Map(employeeItem);

        return Task.FromResult((Employee?)employee);
    }
}
