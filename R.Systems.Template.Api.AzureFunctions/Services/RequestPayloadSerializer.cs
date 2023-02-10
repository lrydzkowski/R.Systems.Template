using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;

namespace R.Systems.Template.Api.AzureFunctions.Services;

public class RequestPayloadSerializer
{
    public async Task<T?> DeserializeAsync<T>(HttpRequestData request)
    {
        return await JsonSerializer.DeserializeAsync<T>(request.Body);
    }
}
