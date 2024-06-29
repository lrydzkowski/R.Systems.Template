using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployees;

public class GetEmployeesQuery : GetElementsQuery, IContextRequest, IRequest<GetEmployeesResult>
{
    public long? CompanyId { get; init; }
    public ApplicationContext AppContext { get; set; } = new();
}

public class GetEmployeesResult
{
    public ListInfo<Employee> Employees { get; init; } = new();
}

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, GetEmployeesResult>
{
    private readonly IVersionedRepositoryFactory<IGetEmployeesRepository> _getEmployeesRepositoryFactory;

    public GetEmployeesQueryHandler(IVersionedRepositoryFactory<IGetEmployeesRepository> repositoryFactory)
    {
        _getEmployeesRepositoryFactory = repositoryFactory;
    }

    public async Task<GetEmployeesResult> Handle(GetEmployeesQuery query, CancellationToken cancellationToken)
    {
        IGetEmployeesRepository repository = _getEmployeesRepositoryFactory.GetRepository(query.AppContext);
        ListInfo<Employee> employees = query.CompanyId == null
            ? await repository.GetEmployeesAsync(query.ListParameters, cancellationToken)
            : await repository.GetEmployeesAsync(
                query.ListParameters,
                (long)query.CompanyId,
                cancellationToken
            );
        return new GetEmployeesResult
        {
            Employees = employees
        };
    }
}
