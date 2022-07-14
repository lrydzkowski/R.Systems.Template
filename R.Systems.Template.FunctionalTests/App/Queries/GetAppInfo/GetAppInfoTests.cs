using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.FunctionalTests.Common.Factories;
using R.Systems.Template.WebApi;
using RestSharp;
using System.Net;

namespace R.Systems.Template.FunctionalTests.App.Queries.GetAppInfo;

public class GetAppInfoTests : IClassFixture<WebApiFactory<Program>>
{
    public GetAppInfoTests(WebApiFactory<Program> webApplicationFactory)
    {
        RestClient = new RestClient(webApplicationFactory.CreateClient());
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetAppInfo_CorrectData_ReturnsCorrectVersion()
    {
        string expectedAppName = AppNameService.GetWebApiName();
        string semVerRegex = new SemVerRegex().Get();
        RestRequest request = new("/");

        RestResponse<GetAppInfoResult> response = await RestClient.ExecuteAsync<GetAppInfoResult>(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedAppName, response.Data?.AppName);
        Assert.Matches(semVerRegex, response.Data?.AppVersion ?? "");
    }
}
