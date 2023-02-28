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
        string url = BuildUrl(word);
        HttpStatusCode httpStatusCode = HttpStatusCode.NotFound;
        ErrorResponse apiResponse = new()
        {
            StatusCode = 404,
            Error = "Not Found",
            Message = "Not found"
        };
        WireMockServer.PrepareWireMock(url, httpStatusCode, apiResponse);
        RestClient restClient = WebApiFactory.WithWordnikApiBaseUrl(WireMockServer.Url).CreateRestClient();

        RestRequest restRequest = new(url);
        RestResponse<List<Definition>> response = await restClient.ExecuteAsync<List<Definition>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task GetDefinitions_ShouldReturnDefinitions_WhenDefinitionsExist()
    {
        string word = "penalty";
        string url = BuildUrl(word);
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
        WireMockServer.PrepareWireMock(url, HttpStatusCode.OK, definitionDto);
        RestClient restClient = WebApiFactory.WithWordnikApiBaseUrl(WireMockServer.Url).CreateRestClient();

        RestRequest restRequest = new(url);
        RestResponse<List<Definition>> response = await restClient.ExecuteAsync<List<Definition>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task GetDefinitions_ShouldReturnInternalServerError_WhenWordnikApiReturnsError()
    {
        string word = "penalty";
        string url = BuildUrl(word);
        WireMockServer.PrepareWireMock<object?>(url, HttpStatusCode.InternalServerError, null);
        RestClient restClient = WebApiFactory.WithWordnikApiBaseUrl(WireMockServer.Url).CreateRestClient();

        RestRequest restRequest = new(url);
        RestResponse<List<Definition>> response = await restClient.ExecuteAsync<List<Definition>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        response.Data.Should().BeNull();
    }

    [Fact]
    public async Task GetDefinitions_ShouldReturnDefinitions_WhenRetryPolicySuccess()
    {
        string word = "oblivious";
        string url = BuildUrl(word);
        List<Definition> expectedResponse = new()
        {
            new Definition
            {
                Word = word,
                Text = "A punishment imposed for a violation of law.",
                ExampleUses = new List<string>()
            }
        };
        List<DefinitionDto> definitionDto = new()
        {
            new DefinitionDto
            {
                Word = word,
                Text = expectedResponse[0].Text,
                ExampleUses = new List<DefinitionExampleUsesDto>()
            }
        };
        WireMockServer.PrepareWireMockScenario(
            url,
            new List<ApiResponse<List<DefinitionDto>>>
            {
                new(HttpStatusCode.InternalServerError, null),
                new(HttpStatusCode.RequestTimeout, null),
                new(HttpStatusCode.RequestTimeout, null),
                new(HttpStatusCode.OK, definitionDto)
            }
        );
        RestClient restClient = WebApiFactory.WithWordnikApiBaseUrl(WireMockServer.Url).CreateRestClient();

        RestRequest restRequest = new(url);
        RestResponse<List<Definition>> response = await restClient.ExecuteAsync<List<Definition>>(restRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Data.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    [Fact]
    public async Task GetDefinitions_ShouldReturnDefinitionsFromCache_WhenGetDefinitionsForTheSecondTime()
    {
        string word = "penalty";
        string url = BuildUrl(word);
        List<Definition> expectedResponse = new()
        {
            new Definition
            {
                Word = word,
                Text = "A punishment imposed for a violation of law.",
                ExampleUses = new List<string>()
            }
        };
        List<DefinitionDto> definitionDto = new()
        {
            new DefinitionDto
            {
                Word = word,
                Text = expectedResponse[0].Text,
                ExampleUses = new List<DefinitionExampleUsesDto>()
            }
        };
        WireMockServer.PrepareWireMockScenario(
            url,
            new List<ApiResponse<List<DefinitionDto>>>
            {
                new(HttpStatusCode.OK, definitionDto),
                new(HttpStatusCode.RequestTimeout, null)
            }
        );
        RestClient restClient = WebApiFactory.WithWordnikApiBaseUrl(WireMockServer.Url).CreateRestClient();

        RestRequest restRequest = new(url);
        RestResponse<List<Definition>> firstResponse = await restClient.ExecuteAsync<List<Definition>>(restRequest);

        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        firstResponse.Data.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());

        RestResponse<List<Definition>> secondResponse = await restClient.ExecuteAsync<List<Definition>>(restRequest);

        secondResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        secondResponse.Data.Should().BeEquivalentTo(expectedResponse, options => options.WithStrictOrdering());
    }

    private string BuildUrl(string word)
    {
        return _endpointUrlPath.Replace("{word}", word);
    }
}

internal record ApiResponse<T>(HttpStatusCode HttpStatusCode, T? Data);
