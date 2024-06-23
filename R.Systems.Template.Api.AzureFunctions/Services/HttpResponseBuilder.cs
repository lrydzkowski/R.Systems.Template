using System.Net;
using Microsoft.Azure.Functions.Worker.Http;

namespace R.Systems.Template.Api.AzureFunctions.Services;

public interface IHttpResponseBuilder
{
    Task<HttpResponseData> BuildAsync<T>(HttpRequestData request, T data);
    Task<HttpResponseData> BuildNotFoundAsync<T>(HttpRequestData request, T data);
    HttpResponseData BuildNoContent(HttpRequestData request);
}

public class HttpResponseBuilder : IHttpResponseBuilder
{
    private readonly CustomJsonSerializer _customJsonSerializer;

    public HttpResponseBuilder(CustomJsonSerializer customJsonSerializer)
    {
        _customJsonSerializer = customJsonSerializer;
    }

    public async Task<HttpResponseData> BuildAsync<T>(HttpRequestData request, T data)
    {
        return await BuildAsync(request, data, HttpStatusCode.OK);
    }

    public async Task<HttpResponseData> BuildNotFoundAsync<T>(HttpRequestData request, T data)
    {
        return await BuildAsync(request, data, HttpStatusCode.NotFound);
    }

    public HttpResponseData BuildNoContent(HttpRequestData request)
    {
        HttpResponseData httpResponse = request.CreateResponse(HttpStatusCode.NoContent);
        return httpResponse;
    }

    private async Task<HttpResponseData> BuildAsync<T>(HttpRequestData request, T data, HttpStatusCode statusCode)
    {
        HttpResponseData httpResponse = request.CreateResponse(statusCode);
        httpResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await httpResponse.WriteStringAsync(_customJsonSerializer.Serialize(data));
        return httpResponse;
    }
}
