using System.Net;
using R.Systems.Template.Infrastructure.Wordnik.Common.Api;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace R.Systems.Template.Tests.Api.Web.Integration.Words.Queries.GetDefinitions;

internal static class WireMockBuilder
{
    public static WireMockServer PrepareWireMock<T>(
        this WireMockServer wireMockServer,
        string getDefinitionsUrl,
        HttpStatusCode expectedStatusCode,
        T? response
    )
    {
        wireMockServer.Reset();

        List<ApiResponse<T>> responses = new();
        for (int i = 0; i < WordApi.RetryCount + 1; i++)
        {
            responses.Add(new ApiResponse<T>(expectedStatusCode, response));
        }

        return wireMockServer.PrepareWireMockScenario(getDefinitionsUrl, responses);
    }

    public static WireMockServer PrepareWireMockScenario<T>(
        this WireMockServer wireMockServer,
        string getDefinitionsUrl,
        List<ApiResponse<T>> responses
    )
    {
        wireMockServer.Reset();
        string scenarioName = Guid.NewGuid().ToString();
        for (int i = 0; i < responses.Count; i++)
        {
            ApiResponse<T> response = responses[i];
            IResponseBuilder responseBuilder = Response.Create().WithStatusCode(response.HttpStatusCode);
            if (response.Data != null)
            {
                responseBuilder.WithBodyAsJson(response.Data);
            }

            IRespondWithAProvider provider = wireMockServer
                .Given(Request.Create().WithPath(getDefinitionsUrl).UsingGet())
                .InScenario(scenarioName);
            if (i > 0)
            {
                provider = provider.WhenStateIs($"state{i - 1}");
            }

            provider.WillSetStateTo($"state{i}")
                .RespondWith(responseBuilder);
        }

        return wireMockServer;
    }
}
