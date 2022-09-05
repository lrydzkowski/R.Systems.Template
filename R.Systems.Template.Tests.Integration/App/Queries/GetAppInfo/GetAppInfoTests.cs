using FluentAssertions;
using R.Systems.Template.Tests.Integration.Common.Builders;
using R.Systems.Template.Tests.Integration.Common.Factories;
using R.Systems.Template.WebApi;
using R.Systems.Template.WebApi.Api;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.App.Queries.GetAppInfo;

public class GetAppInfoTests
{
    [Fact]
    public async Task GetAppInfo_ShouldReturnCorrectVersion_WhenCorrectDataIsPassed()
    {
        string expectedAppName = AppNameService.GetWebApiName();
        string semVerRegex = new SemVerRegex().Get();
        RestClient restClient = new WebApiFactory<Program>().CreateRestClient();
        RestRequest request = new("/");

        RestResponse<GetAppInfoResponse> response = await restClient.ExecuteAsync<GetAppInfoResponse>(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().NotBeNull();
        response.Data?.AppName.Should().Be(expectedAppName);
        response.Data?.AppVersion.Should().MatchRegex(semVerRegex);
    }
}
