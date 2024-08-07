using System.Drawing.Printing;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Core.Companies.Queries.GetCompanies;
using R.Systems.Template.Tests.Core.Integration.Common;
using R.Systems.Template.Tests.Core.Integration.Common.Db;
using R.Systems.Template.Tests.Core.Integration.Common.TestsCollections;

namespace R.Systems.Template.Tests.Core.Integration.Companies.Queries.GetCompanies;

[Collection(QueryWithoutDataTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryWithoutDataTestsCollection.CollectionName)]
public class GetCompaniesWithoutDataTests
{
    private readonly ISender _mediator;

    public GetCompaniesWithoutDataTests(SystemUnderTest<NoDataDbInitializer> systemUnderTest)
    {
        _mediator = systemUnderTest.BuildServiceProvider().GetRequiredService<ISender>();
    }

    [Fact]
    public async Task GetCompanies_ShouldReturnEmptyList_WhenCompaniesNotExist()
    {
        GetCompaniesResult expectedResult = new()
        {
            Companies = new ListInfo<Company>()
        };
        GetCompaniesQuery query = new()
        {
            ListParametersDto = new ListParametersDto
            {
                Page = 1,
                PageSize = 100,
                SortingFieldName = null,
                SortingOrder = "asc",
                SearchQuery = null
            }
        };
        GetCompaniesResult result = await _mediator.Send(query);
        result.Should().BeEquivalentTo(expectedResult);
    }
}
