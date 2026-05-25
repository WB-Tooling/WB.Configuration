using AwesomeAssertions;
using WB.Configuration;

namespace ConfigurationTests.MethodTests.PushMethodTests;

public sealed class TryGetStringMethodTests
{
    [Test]
    public void TryGet_ReturnsFalse_WhenConfigurationIsEmpty()
    {
        // Arrange
        Configuration configuration = new();

        // Act
        bool result = configuration.TryGet("key", out string? value);

        // Assert
        result.Should().BeFalse(because: "the configuration is empty");
        value.Should().BeNull(because: "the configuration is empty");
    }

    [Test]
    public void TryGet_ReturnsTrue_WhenKeyExists()
    {
        // Arrange
        Configuration configuration = new();
        using (configuration.Push(new { key = "value" }))
        {
            // Act
            bool result = configuration.TryGet("key", out string? value);

            // Assert
            result.Should().BeTrue(because: "the key exists in the configuration");
            value.Should().Be("value", because: "the key exists in the configuration");
        }
    }

    [Test]
    public void TryGet_ReturnsFalse_WhenKeyDoesNotExist()
    {
        // Arrange
        Configuration configuration = new();
        using (configuration.Push(new { key = "value" }))
        {
            // Act
            bool result = configuration.TryGet("nonexistentKey", out string? value);

            // Assert
            result.Should().BeFalse(because: "the key does not exist in the configuration");
            value.Should().BeNull(because: "the key does not exist in the configuration");
        }
    }
}