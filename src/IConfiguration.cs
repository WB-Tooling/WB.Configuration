using System;

namespace WB.Configuration;

/// <summary>
/// Represents a configuration object that can be queried for values using keys or indices. The configuration supports layering, allowing multiple configuration objects to be merged together, with the most recently pushed configuration taking precedence over earlier configurations. The <see cref="Push"/> method is used to add a new configuration layer, and the returned <see cref="IDisposable"/> should be disposed to remove the layer from the stack.
/// </summary>
public interface IConfiguration : IConfigurationNode
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <summary>
    /// Pushes a new configuration layer onto the stack. The configuration will be merged with the existing layers, and the new configuration will take precedence over the existing layers. The returned <see cref="IDisposable"/> should be disposed to pop the configuration layer off the stack.
    /// </summary>
    /// <param name="configuration">The configuration object to push onto the stack.</param>
    /// <returns>An <see cref="IDisposable"/> that, when disposed, will pop the configuration layer off the stack.</returns>
    public IDisposable Push(object configuration);
}
