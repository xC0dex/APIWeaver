using APIWeaver.Schema.Repositories;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace APIWeaver.OpenApi.Tests;

public class OpenApiDocumentGeneratorTests
{
    private readonly IApiDescriptionGroupCollectionProvider _apiDescriptionProvider = Substitute.For<IApiDescriptionGroupCollectionProvider>();

    private readonly OpenApiDocumentGenerator _openApiDocumentGenerator;
    private readonly IOpenApiOperationsGenerator _operationsGenerator = Substitute.For<IOpenApiOperationsGenerator>();
    private readonly IOptions<OpenApiOptions> _options = Substitute.For<IOptions<OpenApiOptions>>();
    private readonly ISchemaRepository _schemaRepository = Substitute.For<ISchemaRepository>();
    private readonly IOpenApiServersGenerator _serversGenerator = Substitute.For<IOpenApiServersGenerator>();
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();

    public OpenApiDocumentGeneratorTests()
    {
        _openApiDocumentGenerator = new OpenApiDocumentGenerator(_apiDescriptionProvider, _serviceProvider, _options, _operationsGenerator, _serversGenerator, _schemaRepository);
    }

    [Fact]
    public async Task GenerateDocumentAsync_ShouldCallOtherServices_WhenRequested()
    {
        // Arrange
        var context = Substitute.For<HttpContext>();
        var request = Substitute.For<HttpRequest>();
        request.Scheme.Returns("http");
        request.Host.Returns(new HostString("localhost"));
        request.PathBase.Returns(new PathString("/api"));
        var documentDefinition = new OpenApiDocumentDefinition
        {
            Info = new OpenApiInfo
            {
                Title = "Test",
                Version = "1.2.3"
            }
        };
        context.Request.Returns(request);
        _apiDescriptionProvider.ApiDescriptionGroups.Returns(new ApiDescriptionGroupCollection([], 1));
        _options.Value.Returns(new OpenApiOptions());

        // Act
        await _openApiDocumentGenerator.GenerateDocumentAsync(context, documentDefinition, CancellationToken.None);

        // Assert
        await _serversGenerator.ReceivedWithAnyArgs().GenerateServersAsync(default!, default!);
        _schemaRepository.Received().GetSchemas();
    }

    [Fact]
    public async Task GenerateDocumentAsync_ShouldTransformDocument_WhenTransformerProvided()
    {
        // Arrange
        const string description = "transformed";
        var context = Substitute.For<HttpContext>();
        var request = Substitute.For<HttpRequest>();
        request.Scheme.Returns("http");
        request.Host.Returns(new HostString("localhost"));
        request.PathBase.Returns(new PathString("/api"));
        var documentDefinition = new OpenApiDocumentDefinition
        {
            Info = new OpenApiInfo
            {
                Title = "Test",
                Version = "1.2.3"
            }
        };
        context.Request.Returns(request);
        _apiDescriptionProvider.ApiDescriptionGroups.Returns(new ApiDescriptionGroupCollection([], 1));
        var generatorOptions = new OpenApiGeneratorOptions();
        generatorOptions.DocumentTransformers.Add(async documentContext =>
        {
            documentContext.OpenApiDocument.Info.Description = description;
            await Task.Delay(69, documentContext.CancellationToken);
        });

        var options = new OpenApiOptions
        {
            GeneratorOptions = generatorOptions
        };
        _options.Value.Returns(options);

        // Act
        var result = await _openApiDocumentGenerator.GenerateDocumentAsync(context, documentDefinition, CancellationToken.None);

        // Assert
        result.Info.Description.Should().Be(description);
    }
}