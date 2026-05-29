namespace WB.Configuration;

using System.Text.Json;
using System.Text.Json.Nodes;

/// <inheritdoc cref="IConfigurationNode"/>
internal sealed class ConfigurationNode(JsonNode? jsonNode) : IConfigurationNode
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘
    private readonly JsonNode? jsonNode = jsonNode;

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc />
    public bool TryGet<T>(string key, out T? value)
    {
        if (jsonNode is JsonObject obj && obj.TryGetPropertyValue(key, out JsonNode? jsonValue))
        {
            value = jsonValue.Deserialize<T>();

            return true;
        }
        else
        {
            value = default;

            return false;
        }
    }

    /// <inheritdoc />
    public bool TryGet<T>(int index, out T? value)
    {
        if (jsonNode is JsonArray arr && index >= 0 && index < arr.Count)
        {
            value = arr[index].Deserialize<T>();

            return true;
        }
        else
        {
            value = default;

            return false;
        }
    }
}
