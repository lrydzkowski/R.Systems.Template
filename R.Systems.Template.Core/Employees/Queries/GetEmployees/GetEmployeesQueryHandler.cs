using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployees;

public class GetEmployeesQuery : IGetListQuery, IContextRequest, IRequest<GetEmployeesResult>
{
    public long? CompanyId { get; init; }
    public ApplicationContext AppContext { get; set; } = new();
    public ListParametersDto ListParametersDto { get; init; } = new();
}

public class GetEmployeesResult
{
    public ListInfo<Employee> Employees { get; init; } = new();
}

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, GetEmployeesResult>
{
    private readonly IReadOnlyList<FieldInfo> _fields =
    [
        new FieldInfo
        {
            FieldName = nameof(Employee.EmployeeId),
            DefaultSorting = true,
            UseInFiltering = true,
            UseInSorting = true,
            AlwaysPresent = true
        },
        new FieldInfo
        {
            FieldName = nameof(Employee.FirstName),
            UseInFiltering = true,
            UseInSorting = true
        },
        new FieldInfo
        {
            FieldName = nameof(Employee.LastName),
            UseInFiltering = true,
            UseInSorting = true
        },
        new FieldInfo
        {
            FieldName = nameof(Employee.CompanyId)
        }
    ];

    private readonly IVersionedRepositoryFactory<IGetEmployeesRepository> _getEmployeesRepositoryFactory;
    private readonly IListParametersMapper _listParametersMapper;

    public GetEmployeesQueryHandler(
        IVersionedRepositoryFactory<IGetEmployeesRepository> repositoryFactory,
        IListParametersMapper listParametersMapper
    )
    {
        _getEmployeesRepositoryFactory = repositoryFactory;
        _listParametersMapper = listParametersMapper;
    }

    public async Task<GetEmployeesResult> Handle(GetEmployeesQuery query, CancellationToken cancellationToken)
    {
        IGetEmployeesRepository repository = _getEmployeesRepositoryFactory.GetRepository(query.AppContext);
        ListParameters listParameters = _listParametersMapper.Map(query.ListParametersDto, _fields);
        ListInfo<Employee> employees = query.CompanyId == null
            ? await repository.GetEmployeesAsync(listParameters, cancellationToken)
            : await repository.GetEmployeesAsync(
                listParameters,
                (long)query.CompanyId,
                cancellationToken
            );
        return new GetEmployeesResult
        {
            Employees = employees
        };
    }
}
