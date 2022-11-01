using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployee;

public class GetEmployeeQuery : IRequest<Result<Employee?>>
{
    public int? CompanyId { get; init; }

    public int EmployeeId { get; init; }
}

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, Result<Employee?>>
{
    public GetEmployeeQueryHandler(IGetEmployeeRepository getEmployeeRepository)
    {
        GetEmployeeRepository = getEmployeeRepository;
    }

    private IGetEmployeeRepository GetEmployeeRepository { get; }

    public async Task<Result<Employee?>> Handle(GetEmployeeQuery query, CancellationToken cancellationToken)
    {
        return query.CompanyId == null
            ? await GetEmployeeRepository.GetEmployeeAsync(query.EmployeeId)
            : await GetEmployeeRepository.GetEmployeeAsync((int)query.CompanyId, query.EmployeeId);
    }
}
