using System.Text.Json;

namespace R.Systems.Template.AzureFunctionsApi.Services;

public class CustomJsonSerializer
{
    public string Serialize<T>(T data)
    {
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        return JsonSerializer.Serialize(data, jsonSerializerOptions);
    }
}
