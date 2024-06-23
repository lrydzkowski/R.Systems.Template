using System.Net;
using FluentAssertions;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;

namespace R.Systems.Template.Tests.Api.Web.Integration.Api;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class ApiTests
{
    private readonly RestClient _restClient;

    public ApiTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        _restClient = webApiFactory.CreateRestClient();
    }

    [Theory]
    [MemberData(nameof(ApiDataBuilder.Build), MemberType = typeof(ApiDataBuilder))]
    public async Task SendRequest_ShouldReturn401_WhenNoAccessToken(string endpointUrlPath, Method httpMethod)
    {
        RestRequest restRequest = new(endpointUrlPath, httpMethod);
        RestResponse response = await _restClient.ExecuteAsync(restRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
