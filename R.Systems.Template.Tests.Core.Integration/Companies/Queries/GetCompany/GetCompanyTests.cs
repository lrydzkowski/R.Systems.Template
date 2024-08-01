using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.Tests.Core.Integration.Common;
using R.Systems.Template.Tests.Core.Integration.Common.Db;
using R.Systems.Template.Tests.Core.Integration.Common.TestsCollections;

namespace R.Systems.Template.Tests.Core.Integration.Companies.Queries.GetCompany;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetCompanyTests
{
    private readonly ISender _mediator;

    public GetCompanyTests(SystemUnderTest<SampleDataDbInitializer> systemUnderTest)
    {
        _mediator = systemUnderTest.BuildServiceProvider().GetRequiredService<ISender>();
    }

    [Fact]
    public async Task GetCompany_ShouldReturnCompany_WhenCompanyExists()
    {
        string companyId = CompaniesSampleData.Companies.First().CompanyId.ToString();
        GetCompanyResult expectedResult = new()
        {
            Company = new Company
            {
                CompanyId = new Guid(companyId),
                Name = "Meta"
            }
        };
        GetCompanyQuery query = new()
        {
            CompanyId = companyId
        };
        GetCompanyResult result = await _mediator.Send(query);
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetCompany_ShouldReturn404_WhenCompanyNotExist()
    {
        GetCompanyResult expectedResult = new()
        {
            Company = null
        };
        GetCompanyQuery query = new()
        {
            CompanyId = "1801444e-fcf7-48c4-b115-3eb495dfc320"
        };
        GetCompanyResult result = await _mediator.Send(query);
        result.Should().BeEquivalentTo(expectedResult);
    }
}
