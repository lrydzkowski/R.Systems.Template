using FluentAssertions;
using R.Systems.Template.FunctionalTests.Common.Factories;
using R.Systems.Template.WebApi;
using R.Systems.Template.WebApi.Api;
using RestSharp;
using System.Net;

namespace R.Systems.Template.FunctionalTests.App.Queries.GetAppInfo;

public class GetAppInfoTests : IClassFixture<WebApiFactory<Program>>
{
    public GetAppInfoTests(WebApiFactory<Program> webApiFactory)
    {
        RestClient = new RestClient(webApiFactory.CreateClient());
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetAppInfo_ShouldReturnCorrectVersion_WhenCorrectDataIsPassed()
    {
        string expectedAppName = AppNameService.GetWebApiName();
        string semVerRegex = new SemVerRegex().Get();
        RestRequest request = new("/");

        RestResponse<GetAppInfoResponse> response = await RestClient.ExecuteAsync<GetAppInfoResponse>(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data?.AppName.Should().Be(expectedAppName);
        response.Data?.AppVersion.Should().MatchRegex(semVerRegex);
    }
}
