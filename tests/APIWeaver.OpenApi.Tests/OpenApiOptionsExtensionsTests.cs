using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using NSubstitute;

namespace APIWeaver.OpenApi.Tests;

public class OpenApiOptionsExtensionsTests
{
    [Fact]
    public async Task AddDocumentTransformer_ShouldAddTransformer()
    {
        // Arrange
        var options = new OpenApiOptions();
        var openApiDocument = new OpenApiDocument();
        var transformerContext = new OpenApiDocumentTransformerContext
        {
            ApplicationServices = Substitute.For<IServiceProvider>(),
            DescriptionGroups = [],
            DocumentName = "v1"
        };

        // Act
        options.AddDocumentTransformer((document, _) =>
        {
            document.Info = new OpenApiInfo
            {
                Title = "v1"
            };
        });

        // Assert
        var documentTransformers = OpenApiOptionsAccessor.GetDocumentTransformers(options);
        documentTransformers.Should().HaveCount(1);
        await documentTransformers[0].TransformAsync(openApiDocument, transformerContext, CancellationToken.None);
        openApiDocument.Info.Title.Should().Be("v1");
    }

    [Fact]
    public async Task AddOperationTransformer_ShouldAddTransformer()
    {
        // Arrange
        var options = new OpenApiOptions();
        var openApiOperation = new OpenApiOperation();
        var transformerContext = new OpenApiOperationTransformerContext
        {
            ApplicationServices = Substitute.For<IServiceProvider>(),
            DocumentName = "v1",
            Description = new ApiDescription()
        };

        // Act
        options.AddOperationTransformer((operation, _) => { operation.OperationId = "test"; });

        // Assert
        var operationTransformers = OpenApiOptionsAccessor.GetOperationTransformers(options);
        operationTransformers.Should().HaveCount(1);
        await operationTransformers[0].TransformAsync(openApiOperation, transformerContext, CancellationToken.None);
        openApiOperation.OperationId.Should().Be("test");
    }

    [Fact]
    public void AddSecurityScheme_ShouldAddRequiredTransformers()
    {
        // Arrange
        var options = new OpenApiOptions();

        // Act
        options.AddSecurityScheme("Bearer", new OpenApiSecurityScheme());

        // Assert
        var operationTransformers = OpenApiOptionsAccessor.GetOperationTransformers(options);
        operationTransformers.Should().HaveCount(1);
        var documentTransformers = OpenApiOptionsAccessor.GetDocumentTransformers(options);
        documentTransformers.Should().HaveCount(1);
    }

    [Fact]
    public async Task AddSecurityScheme_ShouldAddRequiredTransformersd()
    {
        // Arrange
        var options = new OpenApiOptions();
        var openApiDocument = new OpenApiDocument();
        var transformerContext = new OpenApiDocumentTransformerContext
        {
            ApplicationServices = Substitute.For<IServiceProvider>(),
            DescriptionGroups = [],
            DocumentName = "v1"
        };

        // Act
        options.AddSecurityScheme("Bearer", (scheme, provider) => { scheme.Name = "Test"; });

        // Assert
        var documentTransformers = OpenApiOptionsAccessor.GetDocumentTransformers(options);
        documentTransformers.Should().HaveCount(1);
        await documentTransformers[0].TransformAsync(openApiDocument, transformerContext, CancellationToken.None);
        openApiDocument.Components.SecuritySchemes.Should().HaveCount(1).And.Contain(x => x.Value.Name == "Test");
    }
}