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
    private readonly IGetCompanyRepository _getCompanyRepository;

    public GetCompanyQueryHandler(IGetCompanyRepository getCompanyRepository)
    {
        _getCompanyRepository = getCompanyRepository;
    }

    public async Task<GetCompanyResult> Handle(GetCompanyQuery query, CancellationToken cancellationToken)
    {
        Company? company = await _getCompanyRepository.GetCompanyAsync(query.CompanyId, cancellationToken);
        return new GetCompanyResult
        {
            Company = company
        };
    }
}
