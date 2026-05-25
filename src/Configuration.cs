using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WB.Configuration;

/// <inheritdoc cref="IConfiguration"/>
internal sealed class Configuration : IConfiguration, IConfigurationNode
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Private Fields                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘
    private static readonly JsonSerializerOptions jsonSerializerOptions = new();

    private readonly List<JsonNode?> layers = [];

    private ConfigurationNode? configurationNode;

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
        //
        if (overrideNode is null)
        {
            return baseNode?.DeepClone();
        }

        // baseNode null → take override node
        if (baseNode is null)
        {
            return overrideNode.DeepClone();
        }

        // both objects → merge recursively
        if (baseNode is JsonObject baseObj && overrideNode is JsonObject overrideObj)
        {
            return MergeObjects(baseObj, overrideObj);
        }

        // Arrays are not merged but replace the base value entirely
        if (overrideNode is JsonArray)
        {
            return overrideNode.DeepClone();
        }

        // Scalars or type conflicts → override wins
        return overrideNode.DeepClone();
    }

    private static JsonObject MergeObjects(JsonObject baseObject, JsonObject overrideObj)
    {
        JsonObject result = [];

        // Erst alle Keys aus base übernehmen
        foreach (var kvp in baseObject)
        {
            result[kvp.Key] = kvp.Value?.DeepClone();
        }

        // Dann override anwenden
        foreach (var kvp in overrideObj)
        {
            string? key = kvp.Key;
            JsonNode? overrideValue = kvp.Value;

            // Null löscht den Key (VS Code Verhalten)
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
