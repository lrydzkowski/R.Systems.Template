using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Tests.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Integration.Common.WebApplication;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.Companies.Queries.GetCompanies;

[Collection(QueryWithoutDataTestsCollection.CollectionName)]
public class GetCompaniesWithoutDataTests
{
    private readonly string _endpointUrlPath = "/companies";

    public GetCompaniesWithoutDataTests(WebApiFactory webApiFactory)
    {
        RestClient = webApiFactory.WithoutData().CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetCompanies_ShouldReturnEmptyList_WhenCompaniesNotExist()
    {
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<List<Company>> response = await RestClient.ExecuteAsync<List<Company>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(new List<Company>());
    }
}
