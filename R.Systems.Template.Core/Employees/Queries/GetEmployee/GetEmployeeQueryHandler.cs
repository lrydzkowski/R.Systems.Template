using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployee;

public class GetEmployeeQuery : IRequest<GetEmployeeResult>
{
    public int? CompanyId { get; init; }
    public int EmployeeId { get; init; }
}

public class GetEmployeeResult
{
    public Employee? Employee { get; init; } = new();
}

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, GetEmployeeResult>
{
    private readonly IGetEmployeeRepository _getEmployeeRepository;

    public GetEmployeeQueryHandler(IGetEmployeeRepository getEmployeeRepository)
    {
        _getEmployeeRepository = getEmployeeRepository;
    }

    public async Task<GetEmployeeResult> Handle(GetEmployeeQuery query, CancellationToken cancellationToken)
    {
        Employee? employee = query.CompanyId == null
            ? await _getEmployeeRepository.GetEmployeeAsync(query.EmployeeId, cancellationToken)
            : await _getEmployeeRepository.GetEmployeeAsync((int)query.CompanyId, query.EmployeeId, cancellationToken);
        return new GetEmployeeResult
        {
            Employee = employee
        };
    }
}
