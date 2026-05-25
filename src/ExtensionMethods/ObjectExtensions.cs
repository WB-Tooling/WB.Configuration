using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WB.Configuration;

internal static class ObjectExtensions
{
    public static JsonObject ToJsonObject(this object obj, JsonSerializerOptions jsonSerializerOptions)
    {
        JsonNode? jsonNode = JsonSerializer.SerializeToNode(obj, jsonSerializerOptions);

        if (jsonNode is not JsonObject jsonObject)
        {
            throw new ArgumentException("Object must be serializable to an object.", nameof(obj));
        }

        return jsonObject;
    }
}