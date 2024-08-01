using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployees;

public interface IGetEmployeesRepository : IVersionedRepository
{
    Task<ListInfo<Employee>> GetEmployeesAsync(ListParameters listParameters, CancellationToken cancellationToken);

    Task<ListInfo<Employee>> GetEmployeesAsync(
        ListParameters listParameters,
        Guid companyId,
        CancellationToken cancellationToken
    );
}
