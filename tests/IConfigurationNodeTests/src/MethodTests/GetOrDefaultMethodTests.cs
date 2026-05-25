using System.Text.Json;
using System.Text.Json.Nodes;
using AwesomeAssertions;
using WB.Configuration;

namespace IConfigurationNodeTests.MethodTests.GetOrDefaultMethodTests;

public sealed class TheGetOrDefaultMethod
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
        string value = configurationNode.GetOrDefault("key", "defaultValue");

        // Assert
        value.Should().Be("value", because: "the key exists in the configuration node");
    }

    [Test]
    public void ShouldReturnDefaultValue_WhenKeyDoesNotExist()
    {
        // Arrange
        IConfigurationNode configurationNode = new ConfigurationNode(new JsonObject
        {
            ["key"] = "value"
        });

        // Act
        string value = configurationNode.GetOrDefault("nonexistentKey", "defaultValue");

        // Assert
        value.Should().Be("defaultValue", because: "the key does not exist in the configuration node");
    }

    [Test]
    public void ShouldReturnValue_WhenIndexExists()
    {
        // Arrange
        JsonArray jsonArray = JsonSerializer.SerializeToNode(new[] { "value" })!.AsArray();
        IConfigurationNode configurationNode = new ConfigurationNode(jsonArray);

        // Act
        string value = configurationNode.GetOrDefault(0, "defaultValue");

        // Assert
        value.Should().Be("value", because: "the index exists in the configuration node");
    }

    [Test]
    public void ShouldReturnDefaultValue_WhenIndexDoesNotExist()
    {
        // Arrange
        JsonArray jsonArray = JsonSerializer.SerializeToNode(new[] { "value" })!.AsArray();
        IConfigurationNode configurationNode = new ConfigurationNode(jsonArray);

        // Act
        string value = configurationNode.GetOrDefault(3, "defaultValue");

        // Assert
        value.Should().Be("defaultValue", because: "the index does not exist in the configuration node");
    }
}
