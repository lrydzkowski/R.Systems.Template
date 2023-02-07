using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using FluentAssertions;
using System.Net;

namespace R.Systems.Template.Tests.Api.Web.Integration.Api;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class ApiTests
{
    public ApiTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        RestClient = webApiFactory.CreateRestClient();
    }

    private RestClient RestClient { get; }

    [Theory]
    [MemberData(nameof(ApiDataBuilder.Build), MemberType = typeof(ApiDataBuilder))]
    public async Task SendRequest_ShouldReturn401_WhenNoAccessToken(string endpointUrlPath, Method httpMethod)
    {
        RestRequest restRequest = new(endpointUrlPath, httpMethod);

        RestResponse response = await RestClient.ExecuteAsync(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
