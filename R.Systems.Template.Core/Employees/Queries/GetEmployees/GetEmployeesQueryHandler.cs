using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployees;

public class GetEmployeesQuery : GetElementsQuery, IRequest<Result<List<Employee>>>
{
    public int? CompanyId { get; init; }
}

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, Result<List<Employee>>>
{
    public GetEmployeesQueryHandler(IGetEmployeesRepository getCompaniesRepository)
    {
        GetEmployeesRepository = getCompaniesRepository;
    }

    private IGetEmployeesRepository GetEmployeesRepository { get; }

    public async Task<Result<List<Employee>>> Handle(GetEmployeesQuery query, CancellationToken cancellationToken)
    {
        return query.CompanyId == null
            ? await GetEmployeesRepository.GetEmployeesAsync(query.ListParameters)
            : await GetEmployeesRepository.GetEmployeesAsync(query.ListParameters, (int)query.CompanyId);
    }
}
