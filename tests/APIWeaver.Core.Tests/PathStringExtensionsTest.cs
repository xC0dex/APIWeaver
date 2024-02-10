using APIWeaver.Core.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace APIWeaver.Core.Tests;

public class PathStringExtensionsTest
{
    [Fact]
    public void EndsWith_ShouldReturnTrue_WhenPathEndsWithSegment()
    {
        // Arrange
        var path = new PathString("/swagger/document.json");

        // Act
        var result = path.EndsWith("document.json");

        result.Should().BeTrue();
    }

    [Fact]
    public void EndsWith_ShouldReturnFalse_WhenPathEndsNotWithSegment()
    {
        // Arrange
        var path = new PathString("/swagger/document.yml");

        // Act
        var result = path.EndsWith("document.json");

        result.Should().BeFalse();
    }

    [Fact]
    public void EndsWith_ShouldReturnFalse_WhenPathHasNoValue()
    {
        // Arrange
        var path = new PathString(null);

        // Act
        var result = path.EndsWith("document.json");

        result.Should().BeFalse();
    }
}