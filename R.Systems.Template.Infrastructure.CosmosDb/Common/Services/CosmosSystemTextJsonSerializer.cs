using Azure.Core.Serialization;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Azure.Cosmos;

namespace R.Systems.Template.Infrastructure.CosmosDb.Common.Services;

internal class CosmosSystemTextJsonSerializer : CosmosLinqSerializer
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private static readonly JsonObjectSerializer SystemTextJsonSerializer = new(JsonSerializerOptions);

    public override T FromStream<T>(Stream stream)
    {
        using (stream)
        {
            if (stream is { CanSeek: true, Length: 0 })
            {
                return default!;
            }

            if (typeof(Stream).IsAssignableFrom(typeof(T)))
            {
                return (T)(object)stream;
            }

            return (T)SystemTextJsonSerializer.Deserialize(stream, typeof(T), default)!;
        }
    }

    public override Stream ToStream<T>(T input)
    {
        MemoryStream streamPayload = new();
        SystemTextJsonSerializer.Serialize(streamPayload, input, input.GetType(), default);
        streamPayload.Position = 0;

        return streamPayload;
    }

    public override string? SerializeMemberName(MemberInfo memberInfo)
    {
        JsonExtensionDataAttribute? jsonExtensionDataAttribute =
            memberInfo.GetCustomAttribute<JsonExtensionDataAttribute>(true);
        if (jsonExtensionDataAttribute != null)
        {
            return null;
        }

        JsonPropertyNameAttribute? jsonPropertyNameAttribute =
            memberInfo.GetCustomAttribute<JsonPropertyNameAttribute>(true);
        if (!string.IsNullOrEmpty(jsonPropertyNameAttribute?.Name))
        {
            return jsonPropertyNameAttribute.Name;
        }

        if (JsonSerializerOptions.PropertyNamingPolicy != null)
        {
            return JsonSerializerOptions.PropertyNamingPolicy.ConvertName(memberInfo.Name);
        }

        return memberInfo.Name;
    }
}
