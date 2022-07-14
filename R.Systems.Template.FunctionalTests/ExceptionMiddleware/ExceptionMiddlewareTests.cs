using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.WebApi;
using RestSharp;
using System.Net;

namespace R.Systems.Template.FunctionalTests.ExceptionMiddleware;

public class ExceptionMiddlewareTests : IClassFixture<WebApiWithUnexpectedErrorFactory<Program>>
{
    public ExceptionMiddlewareTests(WebApiWithUnexpectedErrorFactory<Program> webApplicationFactory)
    {
        RestClient = new RestClient(webApplicationFactory.CreateClient());
    }

    private RestClient RestClient { get; }

    [Fact]
    public async Task GetAppInfo_UnexpectedException_Returns500InternalServerError()
    {
        RestRequest request = new("/");

        RestResponse response = await RestClient.ExecuteAsync<GetAppInfoResult>(request);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("", response.Content);
    }
}
