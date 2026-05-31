using System.Text.Json.Nodes;
using AwesomeAssertions;
using WB.Configuration;

namespace ConfigurationNodeTests.IndexerTests.IntIndexTests;

public sealed class IntIndexTests
{
	[Test]
	public void ShouldReturnChildConfigurationNode_WhenIndexExists()
	{
		// Arrange
		ConfigurationNode configurationNode = new(new JsonArray
		{
			new JsonObject
			{
				["key"] = "value"
			}
		});

		// Act
		IConfigurationNode childNode = configurationNode[0];
		string? value = childNode.Get<string>("key");

		// Assert
		value.Should().Be("value", because: "the index exists and should return the child configuration node");
	}

	[Test]
	public void ShouldReturnEmptyConfigurationNode_WhenIndexDoesNotExist()
	{
		// Arrange
		ConfigurationNode configurationNode = new(new JsonArray
		{
			new JsonObject
			{
				["key"] = "value"
			}
		});

		// Act
		IConfigurationNode childNode = configurationNode[1];
		bool result = childNode.TryGet("key", out string? value);

		// Assert
		result.Should().BeFalse(because: "the index does not exist in the current configuration node");
		value.Should().BeNull(because: "an empty configuration node returns default values for any query");
	}

	[Test]
	public void ShouldReturnEmptyConfigurationNode_WhenCurrentNodeIsNotArray()
	{
		// Arrange
		ConfigurationNode configurationNode = new(new JsonObject
		{
			["key"] = "value"
		});

		// Act
		IConfigurationNode childNode = configurationNode[0];
		bool result = childNode.TryGet("key", out string? value);

		// Assert
		result.Should().BeFalse(because: "int index lookup requires the current node to be a JSON array");
		value.Should().BeNull(because: "non-array nodes return an empty configuration node for int index lookups");
	}
}
