using FluentAssertions;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.Tests.Integration.Common.Factories;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Integration.ExceptionMiddleware;

public class ExceptionMiddlewareTests : IClassFixture<WebApiFactory>
{
    public ExceptionMiddlewareTests(WebApiFactory webApiFactory)
    {
        WebApiFactory = webApiFactory;
    }

    private WebApiFactory WebApiFactory { get; }

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
