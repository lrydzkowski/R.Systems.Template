using MediatR;
using Microsoft.Extensions.DependencyInjection;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Queries.GetCompany;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Db;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.Function;
using R.Systems.Template.Tests.Api.AzureFunctions.Integration.Common.TestsCollections;
using FluentAssertions;

namespace R.Systems.Template.Tests.Api.AzureFunctions.Integration.Companies.Queries.GetCompany;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetCompanyTests
{
    public GetCompanyTests(FunctionFactory<SampleDataDbInitializer> functionFactory)
    {
        Mediator = functionFactory.Services!.GetRequiredService<ISender>();
    }

    private ISender Mediator { get; }

    [Fact]
    public async Task GetCompany_ShouldReturnCompany_WhenCompanyExists()
    {
        GetCompanyResult expectedResult = new()
        {
            Company = new Company
            {
                CompanyId = 3,
                Name = "Meta",
                Employees = new List<Employee>()
            }
        };
        GetCompanyQuery query = new() { CompanyId = 3 };
        GetCompanyResult result = await Mediator.Send(query);

        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task GetCompany_ShouldReturn404_WhenCompanyNotExist()
    {
        GetCompanyResult expectedResult = new()
        {
            Company = null
        };
        GetCompanyQuery query = new() { CompanyId = 10 };
        GetCompanyResult result = await Mediator.Send(query);

        result.Should().BeEquivalentTo(expectedResult);
    }
}
