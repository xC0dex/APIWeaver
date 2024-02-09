using APIWeaver.Schema.Models;

namespace APIWeaver.OpenApi.Tests;

public class OpenApiSchemaGeneratorOptionsExtensionsTests
{
    [Fact]
    public void WithJsonOptionsSource_SetsJsonOptionsSourceProperty()
    {
        // Arrange
        var options = new OpenApiSchemaGeneratorOptions();
        const JsonOptionsSource expectedJsonOptionsSource = JsonOptionsSource.MinimalApiOptions;

        // Act
        options.WithJsonOptionsSource(expectedJsonOptionsSource);

        // Assert
        options.JsonOptionsSource.Should().Be(expectedJsonOptionsSource);
    }

    [Fact]
    public void WithNullableAnnotationForReferenceTypes_SetsNullableAnnotationForReferenceTypesProperty()
    {
        // Arrange
        var options = new OpenApiSchemaGeneratorOptions();
        const bool expectedNullableAnnotationForReferenceTypes = true;

        // Act
        options.WithNullableAnnotationForReferenceTypes(expectedNullableAnnotationForReferenceTypes);

        // Assert
        options.NullableAnnotationForReferenceTypes.Should().Be(expectedNullableAnnotationForReferenceTypes);
    }
}