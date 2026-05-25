using System;
using AwesomeAssertions;
using WB.Configuration;

namespace ConfigurationTests.MethodTests.PushMethodTests;

public sealed class ThePushMethod
{
    [Test]
    public void ShouldMergeLayers()
    {
        // Arrange
        IConfiguration configuration = new Configuration();

        // Act
        using (configuration.Push(new { Key1 = "Value1" }))
        using (configuration.Push(new { Key1 = "Value2" }))
        {
            string? value1 = configuration.Get<string>("Key1");

            // Assert
            value1.Should().Be("Value2");
        }
    }

    [Test]
    public void ShouldRestorePreviousLayerWhenTopLayerIsDisposed()
    {
        // Arrange
        IConfiguration configuration = new Configuration();

        using (configuration.Push(new { Key1 = "Value1" }))
        {
            // Act
            using (configuration.Push(new { Key1 = "Value2" }))
            {
                configuration.Get<string>("Key1").Should().Be("Value2");
            }

            // Assert
            configuration.Get<string>("Key1").Should().Be("Value1");
        }
    }

    [Test]
    public void ShouldRemoveKeyWhenOverrideSetsItToNull()
    {
        // Arrange
        IConfiguration configuration = new Configuration();

        // Act
        using (configuration.Push(new { Key1 = "Value1", Key2 = "Value2" }))
        using (configuration.Push(new { Key1 = (string?)null }))
        {
            // Assert
            configuration.Get<string>("Key1").Should().BeNull();
            configuration.Get<string>("Key2").Should().Be("Value2");
        }
    }

    [Test]
    public void ShouldReplaceArraysInsteadOfMergingThem()
    {
        // Arrange
        IConfiguration configuration = new Configuration();

        // Act
        using (configuration.Push(new { Values = new[] { 1, 2, 3 } }))
        using (configuration.Push(new { Values = new[] { 9 } }))
        {
            int[]? values = configuration.Get<int[]>("Values");

            // Assert
            values.Should().BeEquivalentTo([9], options => options.WithStrictOrdering());
        }
    }

    [Test]
    public void ShouldBeEmptyAfterLastLayerIsDisposed()
    {
        // Arrange
        IConfiguration configuration = new Configuration();

        // Act
        using (configuration.Push(new { Key1 = "Value1" }))
        {
            configuration.Get<string>("Key1").Should().Be("Value1");
        }

        // Assert
        configuration.Get<string>("Key1").Should().BeNull();
    }
}