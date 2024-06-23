using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Companies.Queries.GetCompanies;

public class GetCompaniesQuery : GetElementsQuery, IRequest<GetCompaniesResult>
{
}

public class GetCompaniesResult
{
    public ListInfo<Company> Companies { get; init; } = new();
}

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, GetCompaniesResult>
{
    private readonly IGetCompaniesRepository _getCompaniesRepository;

    public GetCompaniesQueryHandler(IGetCompaniesRepository getCompaniesRepository)
    {
        _getCompaniesRepository = getCompaniesRepository;
    }

    public async Task<GetCompaniesResult> Handle(GetCompaniesQuery query, CancellationToken cancellationToken)
    {
        ListInfo<Company> companies =
            await _getCompaniesRepository.GetCompaniesAsync(query.ListParameters, cancellationToken);
        return new GetCompaniesResult
        {
            Companies = companies
        };
    }
}
