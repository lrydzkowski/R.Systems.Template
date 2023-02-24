using R.Systems.Template.Tests.Api.Web.Integration.Common.TestsCollections;
using R.Systems.Template.Tests.Api.Web.Integration.Common;
using R.Systems.Template.Tests.Api.Web.Integration.Common.Db;
using R.Systems.Template.Tests.Api.Web.Integration.Common.WebApplication;
using RestSharp;
using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using R.Systems.Template.Api.Web;
using R.Systems.Template.Tests.Api.Web.Integration.Words.Common;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using R.Systems.Template.Core.Words.Queries.GetDefinitions;
using R.Systems.Template.Infrastructure.Wordnik.Common.Models;

namespace R.Systems.Template.Tests.Api.Web.Integration.Words.Queries.GetDefinitions;

[Collection(QueryTestsCollection.CollectionName)]
[Trait(TestConstants.Category, QueryTestsCollection.CollectionName)]
public class GetDefinitionsTests
{
    private readonly string _endpointUrlPath = "/words/{word}/definitions";

    public GetDefinitionsTests(WebApiFactoryWithDb<SampleDataDbInitializer> webApiFactory)
    {
        WebApiFactory = webApiFactory.WithoutAuthentication();
        RestClient = webApiFactory.WithoutAuthentication().CreateRestClient();
        WireMockServer = webApiFactory.WireMockServer;
    }

    private WebApplicationFactory<Program> WebApiFactory { get; }
    private RestClient RestClient { get; }
    private WireMockServer WireMockServer { get; }

    [Fact]
    public async Task GetDefinitions_ShouldReturnEmptyList_WhenDefinitionsNotExist()
    {
        string word = "penalty";
        PrepareWireMock(
            word,
            HttpStatusCode.NotFound,
            new ErrorResponse
            {
                StatusCode = 404,
                Error = "Not Found",
                Message = "Not found"
            }
        );
        RestClient restClient = WebApiFactory.WithWordnikApiBaseUrl(WireMockServer.Url).CreateRestClient();

        RestRequest restRequest = new(BuildUrl(word));
        RestResponse<List<Definition>> response = await restClient.ExecuteAsync<List<Definition>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task GetDefinitions_ShouldReturnDefinitions_WhenDefinitionsExist()
    {
        string word = "penalty";
        List<Definition> expectedResponse = new()
        {
            new Definition
            {
                Word = word,
                Text = "A punishment imposed for a violation of law.",
                ExampleUses = new List<string>()
            },
            new Definition
            {
                Word = word,
                Text = "The disadvantage or painful consequences resulting from an action or condition.",
                ExampleUses = new List<string>
                {
                    "neglected his health and paid the penalty."
                }
            }
        };
        List<DefinitionDto> definitionDto = new()
        {
            new DefinitionDto
            {
                Word = word,
                Text = expectedResponse[0].Text,
                ExampleUses = new List<DefinitionExampleUsesDto>()
            },
            new DefinitionDto
            {
                Word = word,
                Text = expectedResponse[1].Text,
                ExampleUses = new List<DefinitionExampleUsesDto>
                {
                    new()
                    {
                        Text = expectedResponse[1].ExampleUses[0]
                    }
                }
            }
        };
        PrepareWireMock(word, HttpStatusCode.OK, definitionDto);
        RestClient restClient = WebApiFactory.WithWordnikApiBaseUrl(WireMockServer.Url).CreateRestClient();

        RestRequest restRequest = new(BuildUrl(word));
        RestResponse<List<Definition>> response = await restClient.ExecuteAsync<List<Definition>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    [Fact]
    public Task GetDefinitions_ShouldReturnInternalServerError_WhenWordnikApiReturnsError()
    {
        return Task.CompletedTask;
    }

    private void PrepareWireMock<T>(string word, HttpStatusCode expectedStatusCode, T response)
    {
        WireMockServer.Reset();
        WireMockServer.Given(Request.Create().WithPath(BuildUrl(word)).UsingGet())
            .RespondWith(Response.Create().WithStatusCode(expectedStatusCode).WithBodyAsJson(response!));
    }

    private string BuildUrl(string word)
    {
        return _endpointUrlPath.Replace("{word}", word);
    }
}
