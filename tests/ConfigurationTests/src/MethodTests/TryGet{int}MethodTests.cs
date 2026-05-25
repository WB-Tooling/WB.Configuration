using AwesomeAssertions;
using WB.Configuration;

namespace ConfigurationTests.MethodTests.PushMethodTests;

public sealed class TryGetIntMethodTests
{
    [Test]
    public void TryGet_ReturnsFalse_WhenConfigurationIsEmpty()
    {
        // Arrange
        Configuration configuration = new();

        // Act
        bool result = configuration.TryGet(0, out int? value);

        // Assert
        result.Should().BeFalse(because: "the configuration is empty");
        value.Should().BeNull(because: "the configuration is empty");
    }

    [Test]
    public void TryGet_ReturnsTrue_WhenIndexExists()
    {
        // Arrange
        Configuration configuration = new();
        using (configuration.Push(new int[] { 0, 1, 2 }))
        {
            // Act
            bool result = configuration.TryGet(0, out int? value);

            // Assert
            result.Should().BeTrue(because: "the index exists in the configuration");
            value.Should().Be(0, because: "the index exists in the configuration");
        }
    }

    [Test]
    public void TryGet_ReturnsFalse_WhenIndexDoesNotExist()
    {
        // Arrange
        Configuration configuration = new();
        using (configuration.Push(new int[] { 0, 1, 2 }))
        {
            // Act
            bool result = configuration.TryGet(3, out int? value);

            // Assert
            result.Should().BeFalse(because: "the index does not exist in the configuration");
            value.Should().BeNull(because: "the index does not exist in the configuration");
        }
    }
}