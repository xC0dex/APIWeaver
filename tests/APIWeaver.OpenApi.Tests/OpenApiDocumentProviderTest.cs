namespace APIWeaver.OpenApi.Tests;

public sealed class OpenApiDocumentProviderTest
{
    private const string DocumentName = "TestDocument";
    private readonly HttpContext _context = Substitute.For<HttpContext>();
    private readonly IOpenApiDocumentGenerator _documentGenerator = Substitute.For<IOpenApiDocumentGenerator>();
    private readonly OpenApiDocumentProvider _documentProvider;
    private readonly OpenApiDocument _openApiDocument = new();
    private readonly IOptions<OpenApiOptions> _options = Substitute.For<IOptions<OpenApiOptions>>();

    public OpenApiDocumentProviderTest()
    {
        _documentProvider = new OpenApiDocumentProvider(_documentGenerator, _options);
        var options = new OpenApiOptions
        {
            OpenApiDocuments = new Dictionary<string, OpenApiDocumentDefinition> {{DocumentName, new OpenApiDocumentDefinition()}}
        };
        _options.Value.Returns(options);
        _documentGenerator.GenerateDocumentAsync(Arg.Any<HttpContext>(), Arg.Any<OpenApiDocumentDefinition>(), Arg.Any<CancellationToken>()).Returns(_openApiDocument);
    }

    [Fact]
    public async Task GetOpenApiDocumentAsync_ReturnsDocument_WhenDocumentExists()
    {
        var result = await _documentProvider.GetOpenApiDocumentAsync(_context, DocumentName);
        result.Should().Be(_openApiDocument);
    }

    [Fact]
    public async Task GetOpenApiDocumentAsync_ThrowsException_WhenDocumentDoesNotExist()
    {
        Func<Task> act = async () => await _documentProvider.GetOpenApiDocumentAsync(_context, "Another document");
        await act.Should().ThrowAsync<OpenApiDocumentNotFoundException>().WithMessage("OpenAPI document with name 'Another document' was not found.");
    }
}