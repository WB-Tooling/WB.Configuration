namespace WB.Configuration;

using System;
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
    // │ Public Indexers                                                             │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc />
    public T Get<T>()
    {
        if (jsonNode is null)
        {
            return default!;
        }

        T? result = jsonNode.Deserialize<T>() ?? throw new InvalidOperationException($"Failed to deserialize JSON node to type {typeof(T).FullName}.");
        
        return result;
    }

    /// <inheritdoc />
    public IConfigurationNode this[string key]
    {
        get
        {
            if (jsonNode is JsonObject jsonObject && jsonObject.TryGetPropertyValue(key, out JsonNode? childNode))
            {
                return new ConfigurationNode(childNode);
            }
            else
            {
                return new ConfigurationNode(null);
            }
        }
    }

    public IConfigurationNode this[int index]
    {
        get
        {
            if (jsonNode is JsonArray jsonArray && index >= 0 && index < jsonArray.Count)
            {
                return new ConfigurationNode(jsonArray[index]);
            }
            else
            {
                return new ConfigurationNode(null);
            }
        }
    }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc />
    public bool TryGet<T>(string key, out T? value)
    {
        if (jsonNode is JsonObject jsonObject && jsonObject.TryGetPropertyValue(key, out JsonNode? jsonValue))
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
        if (jsonNode is JsonArray jsonArray && index >= 0 && index < jsonArray.Count)
        {
            value = jsonArray[index].Deserialize<T>();

            return true;
        }
        else
        {
            value = default;

            return false;
        }
    }
}
