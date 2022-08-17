using MediatR;
using R.Systems.Template.Core.Common.DataTransferObjects;

namespace R.Systems.Template.Core.Companies.Queries.GetCompany;

public class GetCompanyRequest : IRequest<GetCompanyResult>
{
    public int CompanyId { get; init; }
}

public class GetCompanyResult
{
    public CompanyDto? Company { get; set; }
}

public class GetCompanyHandler : IRequestHandler<GetCompanyRequest, GetCompanyResult>
{
    public GetCompanyHandler(IGetCompanyRepository getCompanyRepository)
    {
        GetCompanyRepository = getCompanyRepository;
    }

    private IGetCompanyRepository GetCompanyRepository { get; }

    public async Task<GetCompanyResult> Handle(GetCompanyRequest request, CancellationToken cancellationToken)
    {
        CompanyDto? companyDto = await GetCompanyRepository.GetCompanyAsync(request.CompanyId);

        return new GetCompanyResult
        {
            Company = companyDto
        };
    }
}
