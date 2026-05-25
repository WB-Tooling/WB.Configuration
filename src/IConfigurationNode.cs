using System;
using System.Collections.Generic;

namespace WB.Configuration;

public interface IConfigurationNode
{
    public bool TryGet<T>(string key, out T? value);

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

    public bool TryGet<T>(int index, out T? value);

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
