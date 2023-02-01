using System.Net;
using Microsoft.Azure.Functions.Worker.Http;

namespace R.Systems.Template.Api.AzureFunctions.Services;

public class HttpResponseBuilder
{
    public HttpResponseBuilder(CustomJsonSerializer customJsonSerializer)
    {
        CustomJsonSerializer = customJsonSerializer;
    }

    private CustomJsonSerializer CustomJsonSerializer { get; }

    public async Task<HttpResponseData> BuildAsync<T>(HttpRequestData request, T data)
    {
        HttpResponseData httpResponse = request.CreateResponse(HttpStatusCode.OK);
        httpResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");

        await httpResponse.WriteStringAsync(CustomJsonSerializer.Serialize(data));

        return httpResponse;
    }
}
