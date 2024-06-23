using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployees;

public class GetEmployeesQuery : GetElementsQuery, IRequest<GetEmployeesResult>
{
    public int? CompanyId { get; init; }
}

public class GetEmployeesResult
{
    public ListInfo<Employee> Employees { get; init; } = new();
}

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, GetEmployeesResult>
{
    private readonly IGetEmployeesRepository _getEmployeesRepository;

    public GetEmployeesQueryHandler(IGetEmployeesRepository getCompaniesRepository)
    {
        _getEmployeesRepository = getCompaniesRepository;
    }

    public async Task<GetEmployeesResult> Handle(GetEmployeesQuery query, CancellationToken cancellationToken)
    {
        ListInfo<Employee> employees = query.CompanyId == null
            ? await _getEmployeesRepository.GetEmployeesAsync(query.ListParameters, cancellationToken)
            : await _getEmployeesRepository.GetEmployeesAsync(
                query.ListParameters,
                (int)query.CompanyId,
                cancellationToken
            );
        return new GetEmployeesResult
        {
            Employees = employees
        };
    }
}
