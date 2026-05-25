using System;

namespace WB.Configuration;

public interface IConfiguration : IConfigurationNode
{
    public IDisposable Push(object configuration);
}