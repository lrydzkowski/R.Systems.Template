using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Companies.Queries.GetCompany;

public class GetCompanyQuery : IContextRequest, IRequest<GetCompanyResult>
{
    public string CompanyId { get; init; } = "";
    public ApplicationContext AppContext { get; set; } = new();
}

public class GetCompanyResult
{
    public Company? Company { get; set; }
}

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, GetCompanyResult>
{
    private readonly IVersionedRepositoryFactory<IGetCompanyRepository> _repositoryFactory;

    public GetCompanyQueryHandler(IVersionedRepositoryFactory<IGetCompanyRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task<GetCompanyResult> Handle(GetCompanyQuery query, CancellationToken cancellationToken)
    {
        IGetCompanyRepository repository = _repositoryFactory.GetRepository(query.AppContext);
        bool parsingResult = Guid.TryParse(query.CompanyId, out Guid parsedCompanyId);
        if (!parsingResult)
        {
            return new GetCompanyResult();
        }

        Company? company = await repository.GetCompanyAsync(parsedCompanyId, cancellationToken);
        return new GetCompanyResult
        {
            Company = company
        };
    }
}
