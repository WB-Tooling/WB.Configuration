using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WB.Configuration;

/// <inheritdoc cref="IConfiguration"/>
public sealed class Configuration : IConfiguration, IConfigurationNode
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘
    private static readonly JsonSerializerOptions jsonSerializerOptions = new();

    private readonly List<JsonNode?> layers = [];

    private ConfigurationNode? configurationNode;

    public IConfigurationNode this[int index]
    {
        get
        {
            if (configurationNode is null)
            {
                return new ConfigurationNode(null);
            }

            return configurationNode[index];
        }
    }

    public IConfigurationNode this[string key]
    {
        get
        {
            if (configurationNode is null)
            {
                return new ConfigurationNode(null);
            }

            return configurationNode[key];
        }
    }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc />
    public bool TryGet<T>(string key, out T? value)
    {
        if (configurationNode is null)
        {
            value = default;

            return false;
        }

        return configurationNode.TryGet(key, out value);
    }

    /// <inheritdoc />
    public bool TryGet<T>(int index, out T? value)
    {
        if (configurationNode is null)
        {
            value = default;

            return false;
        }

        return configurationNode.TryGet(index, out value);
    }


    /// <inheritdoc />
    public IDisposable Push(object configuration)
    {
        JsonNode? jsonNode = JsonSerializer.SerializeToNode(configuration, jsonSerializerOptions);

        lock (layers)
        {
            layers.Add(jsonNode);

            configurationNode = new ConfigurationNode(MergeLayers(layers));
        }

        return new ActionDisposable(() =>
        {
            lock (layers)
            {
                layers.Remove(jsonNode);

                configurationNode = new ConfigurationNode(MergeLayers(layers));
            }
        });
    }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Methods                                                             │
    // └─────────────────────────────────────────────────────────────────────────────┘
    private static JsonNode? MergeLayers(IEnumerable<JsonNode?> layers)
    {
        JsonNode? merged = null;

        foreach (var layer in layers)
        {
            merged = Merge(merged, layer);
        }

        return merged;
    }

    private static JsonNode? Merge(JsonNode? baseNode, JsonNode? overrideNode)
    {
        if (overrideNode is null)
        {
            return baseNode?.DeepClone();
        }

        if (baseNode is null)
        {
            return overrideNode.DeepClone();
        }

        if (baseNode is JsonObject baseObj && overrideNode is JsonObject overrideObj)
        {
            return MergeObjects(baseObj, overrideObj);
        }

        if (overrideNode is JsonArray)
        {
            return overrideNode.DeepClone();
        }

        return overrideNode.DeepClone();
    }

    private static JsonObject MergeObjects(JsonObject baseObject, JsonObject overrideObj)
    {
        JsonObject result = [];

        foreach (KeyValuePair<string, JsonNode?> kvp in baseObject)
        {
            result[kvp.Key] = kvp.Value?.DeepClone();
        }

        foreach (KeyValuePair<string, JsonNode?> kvp in overrideObj)
        {
            string? key = kvp.Key;
            JsonNode? overrideValue = kvp.Value;

            if (overrideValue is null)
            {
                result.Remove(key);

                continue;
            }

            if (!result.ContainsKey(key))
            {
                result[key] = overrideValue.DeepClone();

                continue;
            }

            JsonNode? baseValue = result[key];

            result[key] = Merge(baseValue, overrideValue);
        }

        return result;
    }
}
