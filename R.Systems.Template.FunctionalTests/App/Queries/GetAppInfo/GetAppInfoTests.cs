using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.WebApi;
using RestSharp;

namespace R.Systems.Template.FunctionalTests.App.Queries.GetAppInfo;

public class GetAppInfoTests : IClassFixture<WebApiFactory<Program>>
{
    public GetAppInfoTests(WebApiFactory<Program> webApplicationFactory)
    {
        RestClient = new RestClient(webApplicationFactory.CreateClient());
    }

    public RestClient RestClient { get; }

    [Fact]
    public async Task GetAppInfo_CorrectData_ReturnsCorrectVersion()
    {
        string expectedAppName = AppNameService.GetWebApiName();
        string semVerRegex = new SemVerRegex().Get();
        RestRequest request = new("/");

        AppInfo? appInfo = await RestClient.GetAsync<AppInfo>(request);

        Assert.Equal(expectedAppName, appInfo?.AppName);
        Assert.Matches(semVerRegex, appInfo?.AppVersion ?? "");
    }
}
