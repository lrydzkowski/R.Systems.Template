using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Companies.Queries.GetCompanies;

public class GetCompaniesQuery : IRequest<GetCompaniesResult>
{
}

public class GetCompaniesResult
{
    public List<Company> Companies { get; init; } = new();
}

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, GetCompaniesResult>
{
    public GetCompaniesQueryHandler(IGetCompaniesRepository getCompaniesRepository)
    {
        GetCompaniesRepository = getCompaniesRepository;
    }

    private IGetCompaniesRepository GetCompaniesRepository { get; }

    public async Task<GetCompaniesResult> Handle(GetCompaniesQuery query, CancellationToken cancellationToken)
    {
        List<Company> companies = await GetCompaniesRepository.GetCompaniesAsync();

        return new GetCompaniesResult
        {
            Companies = companies
        };
    }
}
