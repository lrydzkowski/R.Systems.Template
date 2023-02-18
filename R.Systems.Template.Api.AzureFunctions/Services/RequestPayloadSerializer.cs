using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;

namespace R.Systems.Template.Api.AzureFunctions.Services;

public interface IRequestPayloadSerializer
{
    Task<T?> DeserializeAsync<T>(HttpRequestData request);
}

public class RequestPayloadSerializer
    : IRequestPayloadSerializer
{
    public async Task<T?> DeserializeAsync<T>(HttpRequestData request)
    {
        return await JsonSerializer.DeserializeAsync<T>(
            request.Body,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        );
    }
}
