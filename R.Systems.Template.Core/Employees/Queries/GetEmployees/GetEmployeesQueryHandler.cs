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
    public GetEmployeesQueryHandler(IGetEmployeesRepository getCompaniesRepository)
    {
        GetEmployeesRepository = getCompaniesRepository;
    }

    private IGetEmployeesRepository GetEmployeesRepository { get; }

    public async Task<GetEmployeesResult> Handle(GetEmployeesQuery query, CancellationToken cancellationToken)
    {
        ListInfo<Employee> employees = query.CompanyId == null
            ? await GetEmployeesRepository.GetEmployeesAsync(query.ListParameters, cancellationToken)
            : await GetEmployeesRepository.GetEmployeesAsync(
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
