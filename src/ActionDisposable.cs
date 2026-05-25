using System;

namespace WB.Configuration;

internal sealed class ActionDisposable(Action action) : IDisposable
{
    public void Dispose()
        => action();
}