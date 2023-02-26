using System.Net;
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
        IResponseBuilder responseBuilder = Response.Create().WithStatusCode(expectedStatusCode);
        if (response != null)
        {
            responseBuilder.WithBodyAsJson(response);
        }

        wireMockServer.Given(Request.Create().WithPath(getDefinitionsUrl).UsingGet())
            .RespondWith(responseBuilder);

        return wireMockServer;
    }

    public static WireMockServer PrepareWireMockScenario<T>(
        this WireMockServer wireMockServer,
        string getDefinitionsUrl,
        string scenarioName,
        List<ApiResponse<T>> responses
    )
    {
        wireMockServer.Reset();
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
