using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Companies.Queries.GetCompany;

public class GetCompanyQuery : IRequest<GetCompanyResult>
{
    public int CompanyId { get; init; }
}

public class GetCompanyResult
{
    public Company? Company { get; set; }
}

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, GetCompanyResult>
{
    public GetCompanyQueryHandler(IGetCompanyRepository getCompanyRepository)
    {
        GetCompanyRepository = getCompanyRepository;
    }

    private IGetCompanyRepository GetCompanyRepository { get; }

    public async Task<GetCompanyResult> Handle(GetCompanyQuery query, CancellationToken cancellationToken)
    {
        Company? company = await GetCompanyRepository.GetCompanyAsync(query.CompanyId);

        return new GetCompanyResult
        {
            Company = company
        };
    }
}
