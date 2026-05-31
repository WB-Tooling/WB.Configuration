using System.Text.Json.Nodes;
using AwesomeAssertions;
using WB.Configuration;

namespace ConfigurationNodeTests.IndexerTests.StringIndexTests;

public sealed class StringIndexTests
{
	[Test]
	public void ShouldReturnChildConfigurationNode_WhenKeyExists()
	{
		// Arrange
		ConfigurationNode configurationNode = new(new JsonObject
		{
			["section"] = new JsonObject
			{
				["key"] = "value"
			}
		});

		// Act
		IConfigurationNode childNode = configurationNode["section"];
		string? value = childNode.Get<string>("key");

		// Assert
		value.Should().Be("value", because: "the key exists and should return the child configuration node");
	}

	[Test]
	public void ShouldReturnEmptyConfigurationNode_WhenKeyDoesNotExist()
	{
		// Arrange
		ConfigurationNode configurationNode = new(new JsonObject
		{
			["section"] = new JsonObject
			{
				["key"] = "value"
			}
		});

		// Act
		IConfigurationNode childNode = configurationNode["nonexistentSection"];
		bool result = childNode.TryGet("key", out string? value);

		// Assert
		result.Should().BeFalse(because: "the key does not exist in the current configuration node");
		value.Should().BeNull(because: "an empty configuration node returns default values for any query");
	}

	[Test]
	public void ShouldReturnEmptyConfigurationNode_WhenCurrentNodeIsNotObject()
	{
		// Arrange
		ConfigurationNode configurationNode = new(new JsonArray("value"));

		// Act
		IConfigurationNode childNode = configurationNode["section"];
		bool result = childNode.TryGet("key", out string? value);

		// Assert
		result.Should().BeFalse(because: "string index lookup requires the current node to be a JSON object");
		value.Should().BeNull(because: "non-object nodes return an empty configuration node for string index lookups");
	}
}
