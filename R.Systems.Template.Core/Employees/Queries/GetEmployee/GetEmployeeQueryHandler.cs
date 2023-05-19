using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployee;

public class GetEmployeeQuery : IRequest<GetEmployeeResult>
{
    public int? CompanyId { get; init; }

    public int? EmployeeId { get; init; }
}

public class GetEmployeeResult
{
    public Employee? Employee { get; init; } = new();
}

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, GetEmployeeResult>
{
    public GetEmployeeQueryHandler(IGetEmployeeRepository getEmployeeRepository)
    {
        GetEmployeeRepository = getEmployeeRepository;
    }

    private IGetEmployeeRepository GetEmployeeRepository { get; }

    public async Task<GetEmployeeResult> Handle(GetEmployeeQuery query, CancellationToken cancellationToken)
    {
        int employeeId = query.EmployeeId ?? 0;
        Employee? employee = query.CompanyId == null
            ? await GetEmployeeRepository.GetEmployeeAsync(employeeId, cancellationToken)
            : await GetEmployeeRepository.GetEmployeeAsync((int)query.CompanyId, employeeId, cancellationToken);

        return new GetEmployeeResult
        {
            Employee = employee
        };
    }
}
