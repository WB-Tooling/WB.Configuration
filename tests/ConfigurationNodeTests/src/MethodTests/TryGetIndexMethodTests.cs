using System.Text.Json;
using System.Text.Json.Nodes;
using AwesomeAssertions;
using WB.Configuration;

namespace ConfigurationNodeTests.MethodTests.TryGetIndexMethodTests;

public sealed class TheTryGetMethod
{
    [Test]
    public void ShouldReturnTrueAndValue_WhenKeyExists()
    {
        // Arrange
        JsonArray jsonArray = JsonSerializer.SerializeToNode(new[] { 1, 2, 3 })!.AsArray();
        ConfigurationNode configurationNode = new(jsonArray);
        // Act
        bool result = configurationNode.TryGet(0, out int? value);

        // Assert
        result.Should().BeTrue(because: "the key exists in the configuration node");
        value.Should().Be(1, because: "the key exists in the configuration node");
    }

    [Test]
    public void ShouldReturnFalseAndNull_WhenKeyDoesNotExist()
    {
        // Arrange
        JsonArray jsonArray = JsonSerializer.SerializeToNode(new[] { 1, 2, 3 })!.AsArray();
        ConfigurationNode configurationNode = new(jsonArray);

        // Act
        bool result = configurationNode.TryGet(3, out int? value);

        // Assert
        result.Should().BeFalse(because: "the key does not exist in the configuration node");
        value.Should().BeNull(because: "the key does not exist in the configuration node");
    }
}
