using System;

namespace WB.Configuration;

/// <summary>
/// Represents a disposable object that executes a specified action when disposed. This is used to manage the lifecycle of configuration layers in the <see cref="Configuration"/> class, allowing layers to be automatically removed from the stack when they are no longer needed.
/// </summary>
/// <param name="action">The <see cref="Action"/> to execute when the object is disposed.</param>
internal sealed class ActionDisposable(Action action) : IDisposable
{
    // ┌─────────────────────────────────────────────────────────────────────────────┐
    // │ Public Methods                                                              │
    // └─────────────────────────────────────────────────────────────────────────────┘

    /// <inheritdoc />
    /// <remarks>
    /// When this object is disposed, it will execute the provided action.
    /// </remarks>
    public void Dispose()
        => action();
}
