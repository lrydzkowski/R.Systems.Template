﻿using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Companies.Queries.GetCompanies;

public class GetCompaniesQuery : GetElementsQuery, IRequest<GetCompaniesResult>
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
        List<Company> companies = await GetCompaniesRepository.GetCompaniesAsync(query.ListParameters);

        return new GetCompaniesResult
        {
            Companies = companies
        };
    }
}
