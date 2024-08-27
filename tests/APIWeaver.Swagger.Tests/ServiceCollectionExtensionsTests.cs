using Microsoft.Extensions.Options;

namespace APIWeaver.Swagger.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddOpenApiDocument_ShouldAddDefaultDocument()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddOpenApiDocument();

        // Assert
        using var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<OpenApiHelperOptions>>();
        options.Value.Documents.Should().Contain(Constants.InitialDocumentName);
    }

    [Fact]
    public void AddOpenApiDocument_WithName_ShouldAddDocumentWithName()
    {
        // Arrange
        var services = new ServiceCollection();
        const string documentName = "CustomDocument";

        // Act
        services.AddOpenApiDocument(documentName);

        // Assert
        using var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<OpenApiHelperOptions>>();
        options.Value.Documents.Should().Contain(documentName);
    }


    [Fact]
    public void AddOpenApiDocuments_ShouldAddMultipleDocuments()
    {
        // Arrange
        var services = new ServiceCollection();
        List<string> documentNames = ["Doc1", "Doc2"];

        // Act
        services.AddOpenApiDocuments(documentNames);

        // Assert
        using var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<OpenApiHelperOptions>>();
        options.Value.Documents.Should().Contain(documentNames);
    }
}