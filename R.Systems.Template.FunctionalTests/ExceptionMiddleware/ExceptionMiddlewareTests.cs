using Microsoft.AspNetCore.Mvc.Testing;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.WebApi;
using RestSharp;
using System.Net;

namespace R.Systems.Template.FunctionalTests.ExceptionMiddleware;

public class ExceptionMiddlewareTests : IClassFixture<WebApplicationFactory<Program>>
{
    public ExceptionMiddlewareTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        WebApplicationFactory = webApplicationFactory;
    }

    private WebApplicationFactory<Program> WebApplicationFactory { get; }

    [Fact]
    public async Task GetAppInfo_UnexpectedException_Returns500InternalServerError()
    {
        RestClient restClient = WebApplicationFactory.BuildWithCustomGetAppInfoHandler();

        RestRequest request = new("/");

        RestResponse response = await restClient.ExecuteAsync<GetAppInfoResult>(request);

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("", response.Content);
    }
}
