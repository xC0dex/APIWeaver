using APIWeaver.Generator;

namespace APIWeaver.Tests;

public class OpenApiDocumentGeneratorTest
{
    [Fact]
    public async Task GenerateDocumentAsync_ShouldReturnOpenApiDocument()
    {
        // Arrange
        var generator = new OpenApiDocumentGenerator();

        // Act
        var document = await generator.GenerateDocumentAsync();

        // Assert
        document.Info.Title.Should().Be("APIWeaver");
    }
}