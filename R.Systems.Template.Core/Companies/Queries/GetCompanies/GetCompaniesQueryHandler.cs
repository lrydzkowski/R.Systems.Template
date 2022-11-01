using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core.Companies.Queries.GetCompanies;

public class GetCompaniesQuery : GetElementsQuery, IRequest<Result<List<Company>>>
{
}

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, Result<List<Company>>>
{
    public GetCompaniesQueryHandler(IGetCompaniesRepository getCompaniesRepository)
    {
        GetCompaniesRepository = getCompaniesRepository;
    }

    private IGetCompaniesRepository GetCompaniesRepository { get; }

    public async Task<Result<List<Company>>> Handle(GetCompaniesQuery query, CancellationToken cancellationToken)
    {
        return await GetCompaniesRepository.GetCompaniesAsync(query.ListParameters);
    }
}
