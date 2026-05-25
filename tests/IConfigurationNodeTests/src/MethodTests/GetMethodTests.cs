using System.Text.Json;
using System.Text.Json.Nodes;
using AwesomeAssertions;
using WB.Configuration;

namespace IConfigurationNodeTests.MethodTests.GetMethodTests;

public sealed class TheGetMethod
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
        string? value = configurationNode.Get<string>("key");

        // Assert
        value.Should().Be("value", because: "the key exists in the configuration node");
    }

    [Test]
    public void ShouldReturnNull_WhenKeyDoesNotExist()
    {
        // Arrange
        IConfigurationNode configurationNode = new ConfigurationNode(new JsonObject
        {
            ["key"] = "value"
        });

        // Act
        string? value = configurationNode.Get<string>("nonexistentKey");

        // Assert
        value.Should().BeNull(because: "the key does not exist in the configuration node");
    }

    [Test]
    public void ShouldReturnValue_WhenIndexExists()
    {
        // Arrange
        JsonArray jsonArray = JsonSerializer.SerializeToNode(new[] { "value" })!.AsArray();
        IConfigurationNode configurationNode = new ConfigurationNode(jsonArray);

        // Act
        string? value = configurationNode.Get<string>(0);

        // Assert
        value.Should().Be("value", because: "the index exists in the configuration node");
    }

    [Test]
    public void ShouldReturnNull_WhenIndexDoesNotExist()
    {
        // Arrange
        JsonArray jsonArray = JsonSerializer.SerializeToNode(new[] { "value" })!.AsArray();
        IConfigurationNode configurationNode = new ConfigurationNode(jsonArray);

        // Act
        string? value = configurationNode.Get<string>(1);

        // Assert
        value.Should().BeNull(because: "the index does not exist in the configuration node");
    }
}
