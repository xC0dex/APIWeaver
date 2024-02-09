using APIWeaver.Schema.Models;

namespace APIWeaver.OpenApi.Tests;

public class OpenApiOptionsExtensionsTests
{
    private readonly OpenApiOptions _options = new();
    
    [Fact]
    public void WithGeneratorOptions_ShouldSetGeneratorOptions()
    {

        // Act
        _options.WithGeneratorOptions(x =>
        {
            x.DocumentTransformers.Add(_ => { });
            x.OperationTransformers.Add(_ => { });
            x.ServerTransformers.Add(_ => { });
        });

        // Assert
        _options.GeneratorOptions.DocumentTransformers.Should().HaveCount(1);
        _options.GeneratorOptions.OperationTransformers.Should().HaveCount(1);
        _options.GeneratorOptions.ServerTransformers.Should().HaveCount(1);
    }
    
    [Fact]
    public void WithSchemaGeneratorOptions_ShouldSetSchemaGeneratorOptions()
    {

        // Act
        _options.WithSchemaGeneratorOptions(x =>
        {
            x.WithJsonOptionsSource(JsonOptionsSource.ControllerOptions);
            x.WithNullableAnnotationForReferenceTypes(false);
            x.SchemaTransformers.Add(_ => {});
        });

        // Assert
        _options.SchemaGeneratorOptions.JsonOptionsSource.Should().Be(JsonOptionsSource.ControllerOptions);
        _options.SchemaGeneratorOptions.NullableAnnotationForReferenceTypes.Should().BeFalse();
        _options.SchemaGeneratorOptions.SchemaTransformers.Should().HaveCount(1);
    }
    
    [Fact]
    public void AddOpenApiDocument_ShouldAddDocument()
    {

        // Act
        _options.AddOpenApiDocument("document", x =>
        {
            x.Info = new OpenApiInfo
            {
                Title = "My-Title",
                Version = "1.2.3"
            };
        });
      

        // Assert
        _options.OpenApiDocuments.Should().HaveCount(1);
        _options.OpenApiDocuments["document"].Info!.Title.Should().Be("My-Title");
        _options.OpenApiDocuments["document"].Info!.Version.Should().Be("1.2.3");
    }
    
    [Fact]
    public void AddOpenApiDocument_ShouldAddDocument1()
    {

        // Act
        _options.AddOpenApiDocument("document", new OpenApiDocumentDefinition
        {
            Info = new OpenApiInfo
            {
                Title = "My-Title",
                Version = "1.2.3"
            }
        });
      

        // Assert
        _options.OpenApiDocuments.Should().HaveCount(1);
        _options.OpenApiDocuments["document"].Info!.Title.Should().Be("My-Title");
        _options.OpenApiDocuments["document"].Info!.Version.Should().Be("1.2.3");
    }

}