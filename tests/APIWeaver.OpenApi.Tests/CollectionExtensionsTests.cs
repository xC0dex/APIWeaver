namespace APIWeaver.OpenApi.Tests;

public class CollectionExtensionsTests
{
    private readonly int?[] _array = [1, 2, 3, 4, 5];

    [Fact]
    public void Any_ReturnsTrue_IfAnyElementMatches()
    {
        // Act
        var result = _array.Any(x => x > 3);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Any_ReturnsFalse_IfNoElementMatches()
    {
        // Act
        var result = _array.Any(x => x > 5);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Any_ReturnsTrue_IfArrayHasElements()
    {
        // Act
        var result = _array.Any();

        // Assert
        result.Should().BeTrue();
    }
}