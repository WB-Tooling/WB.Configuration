using System.Text.Json.Nodes;
using AwesomeAssertions;
using WB.Configuration;

namespace ConfigurationNodeTests.MethodTests.TryGetKeyMethodTests;

public sealed class TheTryGetMethod
{
    [Test]
    public void ShouldReturnTrueAndValue_WhenKeyExists()
    {
        // Arrange
        ConfigurationNode configurationNode = new(new JsonObject
        {
            ["key"] = "value"
        });

        // Act
        bool result = configurationNode.TryGet("key", out string? value);

        // Assert
        result.Should().BeTrue(because: "the key exists in the configuration node");
        value.Should().Be("value", because: "the key exists in the configuration node");
    }

    [Test]
    public void ShouldReturnFalseAndNull_WhenKeyDoesNotExist()
    {
        // Arrange
        ConfigurationNode configurationNode = new(new JsonObject
        {
            ["key"] = "value"
        });

        // Act
        bool result = configurationNode.TryGet("nonexistentKey", out JsonNode? value);

        // Assert
        result.Should().BeFalse(because: "the key does not exist in the configuration node");
        value.Should().BeNull(because: "the key does not exist in the configuration node");
    }
}
