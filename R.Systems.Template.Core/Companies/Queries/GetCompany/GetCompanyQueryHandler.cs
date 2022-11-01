using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core.Companies.Queries.GetCompany;

public class GetCompanyQuery : IRequest<Result<Company?>>
{
    public int CompanyId { get; init; }
}

public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, Result<Company?>>
{
    public GetCompanyQueryHandler(IGetCompanyRepository getCompanyRepository)
    {
        GetCompanyRepository = getCompanyRepository;
    }

    private IGetCompanyRepository GetCompanyRepository { get; }

    public async Task<Result<Company?>> Handle(GetCompanyQuery query, CancellationToken cancellationToken)
    {
        return await GetCompanyRepository.GetCompanyAsync(query.CompanyId);
    }
}
