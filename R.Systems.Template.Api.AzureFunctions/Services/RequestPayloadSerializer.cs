using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;

namespace R.Systems.Template.Api.AzureFunctions.Services;

public interface IRequestPayloadSerializer
{
    Task<T?> DeserializeAsync<T>(HttpRequestData request);
}

public class RequestPayloadSerializer : IRequestPayloadSerializer
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<T?> DeserializeAsync<T>(HttpRequestData request)
    {
        return await JsonSerializer.DeserializeAsync<T>(request.Body, _options);
    }
}
