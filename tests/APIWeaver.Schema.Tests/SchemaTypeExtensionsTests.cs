namespace APIWeaver.Schema.Tests;

public class SchemaTypeExtensionsTests
{
    [Theory]
    [InlineData(OpenApiDataType.String, "string")]
    [InlineData(OpenApiDataType.Number, "number")]
    [InlineData(OpenApiDataType.Integer, "integer")]
    [InlineData(OpenApiDataType.Boolean, "boolean")]
    [InlineData(OpenApiDataType.Array, "array")]
    [InlineData(OpenApiDataType.Object, "object")]
    public void ToStringFast_ShouldReturnExpectedString_ForValidTypes(OpenApiDataType type, string expected)
    {
        // Act
        var result = type.ToStringFast();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ToStringFast_ShouldThrowException_ForInvalidType()
    {
        // Arrange
        const OpenApiDataType invalidType = (OpenApiDataType) 999;

        // Act
        var act = () => invalidType.ToStringFast();

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}