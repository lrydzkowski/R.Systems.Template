using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployee;

public class GetEmployeeQuery : IContextRequest, IRequest<GetEmployeeResult>
{
    public string? CompanyId { get; init; }
    public string EmployeeId { get; init; } = "";
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

        bool parsingEmployeeId = Guid.TryParse(query.EmployeeId, out Guid employeeId);
        if (!parsingEmployeeId)
        {
            return new GetEmployeeResult();
        }

        if (query.CompanyId is null)
        {
            Employee? employee = await repository.GetEmployeeAsync(employeeId, cancellationToken);

            return new GetEmployeeResult
            {
                Employee = employee
            };
        }

        bool parsingCompanyId = Guid.TryParse(query.CompanyId, out Guid companyId);
        if (!parsingCompanyId)
        {
            return new GetEmployeeResult();
        }

        Employee? employeeInCompany = await repository.GetEmployeeAsync(companyId, employeeId, cancellationToken);

        return new GetEmployeeResult
        {
            Employee = employeeInCompany
        };
    }
}
