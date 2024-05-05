using System.Text.Json;

namespace R.Systems.Template.Api.AzureFunctions.Services;

public class CustomJsonSerializer
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public string Serialize<T>(T data)
    {
        return JsonSerializer.Serialize(data, _options);
    }
}
