using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployees;

public class GetEmployeesQuery : IRequest<GetEmployeesResult>
{
}

public class GetEmployeesResult
{
    public List<Employee> Employees { get; init; } = new();
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
        List<Employee> employees = await GetEmployeesRepository.GetEmployeesAsync();

        return new GetEmployeesResult
        {
            Employees = employees
        };
    }
}
