using System;
using System.Collections.Generic;

namespace WB.Configuration;

/// <summary>
/// Represents a node in a configuration hierarchy. A configuration node can be queried for values using keys or indices.
/// </summary>
public interface IConfigurationNode
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Indexers                                                             │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Gets a child configuration node associated with the specified key. If the key does not exist, returns an empty configuration node that will return default values for any queries.
    /// </summary>
    /// <param name="key">The key associated with the child configuration node.</param>
    /// <returns>A child configuration node associated with the specified key, or an empty configuration node if the key does not exist.</returns>
    public IConfigurationNode this[string key] { get; }

    /// <summary>
    /// Gets a child configuration node at the specified index. If the index is out of range, returns an empty configuration node that will return default values for any queries.
    /// </summary>
    /// <param name="index">The index of the child configuration node.</param>
    /// <returns>A child configuration node at the specified index, or an empty configuration node if the index is out of range.</returns>
    public IConfigurationNode this[int index] { get; }

    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    public T Get<T>();

    /// <summary>
    /// Tries to get a value of type <typeparamref name="T"/> associated with the specified key. Returns true if the key exists and the value can be converted to type <typeparamref name="T"/>; otherwise, returns false and sets the output parameter to default.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key associated with the value.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found and the value can be converted to type <typeparamref name="T"/>; otherwise, the default value for type <typeparamref name="T"/>.</param>
    /// <returns>true if the key exists and the value can be converted to type <typeparamref name="T"/>; otherwise, false.</returns>
    public bool TryGet<T>(string key, out T? value);

    /// <summary>
    /// Gets a value of type <typeparamref name="T"/> associated with the specified key. Throws a <see cref="KeyNotFoundException"/> if the key does not exist or the value cannot be converted to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key associated with the value.</param>
    /// <returns>The value associated with the specified key.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the key does not exist or the value cannot be converted to type <typeparamref name="T"/>.</exception>
    public T GetRequired<T>(string key)
    {
        if (TryGet(key, out T? value) && value is not null)
        {
            return value;
        }
        else
        {
            throw new KeyNotFoundException($"Key '{key}' not found in configuration.");
        }
    }

    /// <summary>
    /// Gets a value of type <typeparamref name="T"/> associated with the specified key. Returns the default value of type <typeparamref name="T"/> if the key does not exist or the value cannot be converted to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key associated with the value.</param>
    /// <returns>The value associated with the specified key, or the default value of type <typeparamref name="T"/> if the key does not exist or the value cannot be converted to type <typeparamref name="T"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "'Get' is a common method name for retrieving values in configuration APIs, and the context makes it clear that this is not a property accessor.")]
    public T? Get<T>(string key)
    {
        if (TryGet(key, out T? value))
        {
            return value;
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Gets a value of type <typeparamref name="T"/> associated with the specified key. Returns the specified default value if the key does not exist or the value cannot be converted to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key associated with the value.</param>
    /// <param name="defaultValue">The default value to return if the key does not exist or the value cannot be converted to type <typeparamref name="T"/>.</param>
    /// <returns>The value associated with the specified key, or the specified default value if the key does not exist or the value cannot be converted to type <typeparamref name="T"/>.</returns>
    public T GetOrDefault<T>(string key, T defaultValue)
    {
        if (TryGet(key, out T? value) && value is not null)
        {
            return value;
        }
        else
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// Tries to get a value of type <typeparamref name="T"/> associated with the specified index. Returns true if the index exists and the value can be converted to type <typeparamref name="T"/>; otherwise, returns false and sets the output parameter to default.
    /// </summary>
    public bool TryGet<T>(int index, out T? value);

    /// <summary> Gets a value of type <typeparamref name="T"/> associated with the specified index. Throws an
    /// <see cref="ArgumentOutOfRangeException"/> if the index does not exist or the value cannot be converted to type <typeparamref name="T"/>.
    /// </summary>
    public T GetRequired<T>(int index)
    {
        if (TryGet(index, out T? value) && value is not null)
        {
            return value;
        }
        else
        {
            throw new ArgumentOutOfRangeException($"Index '{index}' not found in configuration.");
        }
    }

    /// <summary>
    /// Gets a value of type <typeparamref name="T"/> associated with the specified index. Returns the default value of type <typeparamref name="T"/> if the index does not exist or the value cannot be converted to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="index">The index associated with the value.</param>
    /// <returns>The value associated with the specified index, or the default value of type <typeparamref name="T"/> if the index does not exist or the value cannot be converted to type <typeparamref name="T"/>.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "'Get' is a common method name for retrieving values in configuration APIs, and the context makes it clear that this is not a property accessor.")]
    public T? Get<T>(int index)
    {
        if (TryGet(index, out T? value))
        {
            return value;
        }
        else
        {
            return default;
        }
    }

    /// <summary>
    /// Gets a value of type <typeparamref name="T"/> associated with the specified index. Returns the specified default value if the index does not exist or the value cannot be converted to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="index">The index associated with the value.</param>
    /// <param name="defaultValue">The default value to return if the index does not exist or the value cannot be converted to type <typeparamref name="T"/>.</param>
    /// <returns>The value associated with the specified index, or the specified default value if the index does not exist or the value cannot be converted to type <typeparamref name="T"/>.</returns>
    public T GetOrDefault<T>(int index, T defaultValue)
    {
        if (TryGet(index, out T? value) && value is not null)
        {
            return value;
        }
        else
        {
            return defaultValue;
        }
    }
}
