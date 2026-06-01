using System.Text.Json.Nodes;
using AwesomeAssertions;

namespace WB.Configuration;

public sealed class GetMethodTests
{
    [Test]
    public void ShouldReturnDefaultValue_WhenCurrentNodeIsNull()
    {
        // Arrange
        ConfigurationNode configurationNode = new(null);

        // Act
        string? value = configurationNode.Get<string>();

        // Assert
        value.Should().BeNull(because: "the current node is null and should return the default value for the requested type");
    }

    [Test]
    public void ShouldReturnDeserializedValue_WhenCurrentNodeIsValidJson()
    {
        // Arrange
        ConfigurationNode configurationNode = new(JsonNode.Parse("\"value\""));

        // Act
        string? value = configurationNode.Get<string>();

        // Assert
        value.Should().Be("value", because: "the current node contains valid JSON that can be deserialized to the requested type");
    }   
}
