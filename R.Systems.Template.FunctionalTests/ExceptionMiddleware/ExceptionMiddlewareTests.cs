using FluentAssertions;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.FunctionalTests.Common.Factories;
using R.Systems.Template.WebApi;
using RestSharp;
using System.Net;

namespace R.Systems.Template.FunctionalTests.ExceptionMiddleware;

public class ExceptionMiddlewareTests : IClassFixture<WebApiFactory<Program>>
{
    public ExceptionMiddlewareTests(WebApiFactory<Program> webApiFactory)
    {
        WebApiFactory = webApiFactory;
    }

    private WebApiFactory<Program> WebApiFactory { get; }

    [Fact]
    public async Task GetAppInfo_ShouldReturn500InternalServerError_WhenUnexpectedExceptionWasThrown()
    {
        RestClient restClient = WebApiFactory.BuildWithCustomGetAppInfoHandler();

        RestRequest request = new("/");

        RestResponse response = await restClient.ExecuteAsync<GetAppInfoResult>(request);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Content.Should().Be("");
    }
}
