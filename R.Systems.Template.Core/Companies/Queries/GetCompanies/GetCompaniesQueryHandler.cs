using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Companies.Queries.GetCompanies;

public class GetCompaniesQuery : IGetListQuery, IContextRequest, IRequest<GetCompaniesResult>
{
    public ApplicationContext AppContext { get; set; } = new();
    public ListParametersDto ListParametersDto { get; init; } = new();
}

public class GetCompaniesResult
{
    public ListInfo<Company> Companies { get; init; } = new();
}

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, GetCompaniesResult>
{
    private readonly IReadOnlyList<FieldInfo> _fields =
    [
        new FieldInfo
        {
            FieldName = nameof(Company.CompanyId),
            DefaultSorting = true,
            UseInFiltering = true,
            UseInSorting = true,
            AlwaysPresent = true
        },
        new FieldInfo
        {
            FieldName = nameof(Company.Name),
            UseInFiltering = true,
            UseInSorting = true
        }
    ];

    private readonly IListParametersMapper _listParametersMapper;

    private readonly IVersionedRepositoryFactory<IGetCompaniesRepository> _repositoryFactory;

    public GetCompaniesQueryHandler(
        IVersionedRepositoryFactory<IGetCompaniesRepository> repositoryFactory,
        IListParametersMapper listParametersMapper
    )
    {
        _repositoryFactory = repositoryFactory;
        _listParametersMapper = listParametersMapper;
    }

    public async Task<GetCompaniesResult> Handle(GetCompaniesQuery query, CancellationToken cancellationToken)
    {
        IGetCompaniesRepository repository = _repositoryFactory.GetRepository(query.AppContext);
        ListParameters listParameters = _listParametersMapper.Map(query.ListParametersDto, _fields);
        ListInfo<Company> companies = await repository.GetCompaniesAsync(listParameters, cancellationToken);
        return new GetCompaniesResult
        {
            Companies = companies
        };
    }
}
