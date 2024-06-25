using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployee;

public class GetEmployeeQuery : IContextRequest, IRequest<GetEmployeeResult>
{
    public int? CompanyId { get; init; }
    public int EmployeeId { get; init; }
    public ApplicationContext AppContext { get; set; } = new();
}

public class GetEmployeeResult
{
    public Employee? Employee { get; init; } = new();
}

public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, GetEmployeeResult>
{
    private readonly IVersionedRepositoryFactory<IGetEmployeeRepository> _repositoryFactory;

    public GetEmployeeQueryHandler(IVersionedRepositoryFactory<IGetEmployeeRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task<GetEmployeeResult> Handle(GetEmployeeQuery query, CancellationToken cancellationToken)
    {
        IGetEmployeeRepository repository = _repositoryFactory.GetRepository(query.AppContext);
        Employee? employee = query.CompanyId == null
            ? await repository.GetEmployeeAsync(query.EmployeeId, cancellationToken)
            : await repository.GetEmployeeAsync((int)query.CompanyId, query.EmployeeId, cancellationToken);
        return new GetEmployeeResult
        {
            Employee = employee
        };
    }
}
