using APIWeaver.Exceptions;
using APIWeaver.Generators;
using APIWeaver.Models;
using APIWeaver.Providers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace APIWeaver.Tests;

public class OpenApiDocumentProviderTest
{
    private const string DocumentName = "TestDocument";
    private readonly IOpenApiDocumentGenerator _documentGenerator = Substitute.For<IOpenApiDocumentGenerator>();
    private readonly OpenApiDocumentProvider _documentProvider;
    private readonly OpenApiDocument _openApiDocument = new();
    private readonly IOptions<OpenApiOptions> _options = Substitute.For<IOptions<OpenApiOptions>>();

    public OpenApiDocumentProviderTest()
    {
        _documentProvider = new OpenApiDocumentProvider(_documentGenerator, _options);
        var options = new OpenApiOptions
        {
            OpenApiDocuments = new Dictionary<string, OpenApiInfo> {{DocumentName, new OpenApiInfo()}}
        };
        _options.Value.Returns(options);
        _documentGenerator.GenerateDocumentAsync(DocumentName, Arg.Any<OpenApiInfo>(), Arg.Any<CancellationToken>()).Returns(_openApiDocument);
    }

    [Fact]
    public async Task GetOpenApiDocumentAsync_ReturnsDocument_WhenDocumentExists()
    {
        var result = await _documentProvider.GetOpenApiDocumentAsync(DocumentName);
        result.Should().Be(_openApiDocument);
    }

    [Fact]
    public async Task GetOpenApiDocumentAsync_ThrowsException_WhenDocumentDoesNotExist()
    {
        Func<Task> act = async () => await _documentProvider.GetOpenApiDocumentAsync("Another document");
        await act.Should().ThrowAsync<OpenApiDocumentNotFoundException>().WithMessage("OpenAPI document with name 'Another document' was not found.");
    }

    [Fact]
    public async Task GetOpenApiDocumentAsync_ReturnsCachedDocument_WhenDocumentAlreadyGenerated()
    {
        var result1 = await _documentProvider.GetOpenApiDocumentAsync(DocumentName);
        var result2 = await _documentProvider.GetOpenApiDocumentAsync(DocumentName);
        result1.Should().Be(_openApiDocument);
        result2.Should().Be(_openApiDocument);
        await _documentGenerator.Received(1).GenerateDocumentAsync(DocumentName, Arg.Any<OpenApiInfo>(), Arg.Any<CancellationToken>());
    }
}