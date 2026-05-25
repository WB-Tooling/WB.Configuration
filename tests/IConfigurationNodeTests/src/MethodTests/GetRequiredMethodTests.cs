using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using AwesomeAssertions;
using WB.Configuration;

namespace IConfigurationNodeTests.MethodTests.GetRequiredMethodTests;

public sealed class TheGetRequiredMethod
{
    [Test]
    public void ShouldReturnValue_WhenKeyExists()
    {
        // Arrange
        IConfigurationNode configurationNode = new ConfigurationNode(new JsonObject
        {
            ["key"] = "value"
        });

        // Act
        string value = configurationNode.GetRequired<string>("key");

        // Assert
        value.Should().Be("value", because: "the key exists in the configuration node");
    }

    [Test]
    public void ShouldThrowKeyNotFoundException_WhenKeyDoesNotExist()
    {
        // Arrange
        IConfigurationNode configurationNode = new ConfigurationNode(new JsonObject
        {
            ["key"] = "value"
        });

        // Act
        Action act = () => configurationNode.GetRequired<string>("nonexistentKey");

        // Assert
        act.Should().Throw<KeyNotFoundException>(because: "the key does not exist in the configuration node");
    }

    [Test]
    public void ShouldReturnValue_WhenIndexExists()
    {
        // Arrange
        JsonArray jsonArray = JsonSerializer.SerializeToNode(new[] { "value" })!.AsArray();
        IConfigurationNode configurationNode = new ConfigurationNode(jsonArray);

        // Act
        string value = configurationNode.GetRequired<string>(0);

        // Assert
        value.Should().Be("value", because: "the index exists in the configuration node");
    }

    [Test]
    public void ShouldThrowArgumentOutOfRangeException_WhenIndexDoesNotExist()
    {
        // Arrange
        JsonArray jsonArray = JsonSerializer.SerializeToNode(new[] { "value" })!.AsArray();
        IConfigurationNode configurationNode = new ConfigurationNode(jsonArray);

        // Act
        Action act = () => configurationNode.GetRequired<string>(1);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>(because: "the index does not exist in the configuration node");
    }
}
