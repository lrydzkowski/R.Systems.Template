using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployee;

public class GetEmployeeQuery : IRequest<GetEmployeeResult>
{
    public int EmployeeId { get; init; }
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
        Employee? employee = await GetEmployeeRepository.GetEmployeeAsync(query.EmployeeId);

        return new GetEmployeeResult
        {
            Employee = employee
        };
    }
}
