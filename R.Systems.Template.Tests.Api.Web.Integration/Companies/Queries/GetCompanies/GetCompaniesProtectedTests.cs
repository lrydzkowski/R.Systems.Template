using System.Net;
using FluentAssertions;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;

namespace R.Systems.Template.Tests.Api.Web.Integration.Companies.Queries.GetCompanies;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetCompaniesProtectedTests
{
    private readonly string _endpointUrlPath = "/companies-protected";

    public GetCompaniesProtectedTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        WebApiFactory = webApiFactory;
        RestClient = webApiFactory.CreateRestClient();
    }

    private WebApiFactory WebApiFactory { get; }
    private RestClient RestClient { get; }

    [Fact]
    public async Task GetCompanies_ShouldReturn401_WhenAccessTokenIsNotPresent()
    {
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<List<Company>> response = await RestClient.ExecuteAsync<List<Company>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetCompanies_ShouldReturnCompanies_WhenCompaniesExist()
    {
        ListInfo<Company> expectedResponse = new()
        {
            Data = CompaniesSampleData.Companies,
            Count = CompaniesSampleData.Companies.Count
        };
        RestClient restClient = WebApiFactory.WithoutAuthentication().CreateRestClient();
        RestRequest restRequest = new(_endpointUrlPath);

        RestResponse<ListInfo<Company>> response = await restClient.ExecuteAsync<ListInfo<Company>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data.Should().BeEquivalentTo(expectedResponse);
    }
}
