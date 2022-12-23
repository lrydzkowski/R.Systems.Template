using FluentAssertions;
using R.Systems.Template.Core.App.Queries.GetAppInfo;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Net;

namespace R.Systems.Template.Tests.Api.Web.Integration.ExceptionMiddleware;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class ExceptionMiddlewareTests
{
    public ExceptionMiddlewareTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
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
