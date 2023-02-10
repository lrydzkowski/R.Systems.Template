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
        return await BuildAsync(request, data, HttpStatusCode.OK);
    }

    public async Task<HttpResponseData> BuildNotFoundAsync<T>(HttpRequestData request, T data)
    {
        return await BuildAsync(request, data, HttpStatusCode.NotFound);
    }

    private async Task<HttpResponseData> BuildAsync<T>(HttpRequestData request, T data, HttpStatusCode statusCode)
    {
        HttpResponseData httpResponse = request.CreateResponse(statusCode);
        httpResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");

        await httpResponse.WriteStringAsync(CustomJsonSerializer.Serialize(data));

        return httpResponse;
    }
}
