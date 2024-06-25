using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Companies.Queries.GetCompanies;

public class GetCompaniesQuery : GetElementsQuery, IContextRequest, IRequest<GetCompaniesResult>
{
    public ApplicationContext AppContext { get; set; } = new();
}

public class GetCompaniesResult
{
    public ListInfo<Company> Companies { get; init; } = new();
}

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, GetCompaniesResult>
{
    private readonly IVersionedRepositoryFactory<IGetCompaniesRepository> _repositoryFactory;

    public GetCompaniesQueryHandler(IVersionedRepositoryFactory<IGetCompaniesRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task<GetCompaniesResult> Handle(GetCompaniesQuery query, CancellationToken cancellationToken)
    {
        IGetCompaniesRepository repository = _repositoryFactory.GetRepository(query.AppContext);
        ListInfo<Company> companies = await repository.GetCompaniesAsync(query.ListParameters, cancellationToken);
        return new GetCompaniesResult
        {
            Companies = companies
        };
    }
}
