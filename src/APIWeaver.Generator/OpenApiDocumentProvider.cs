using Microsoft.OpenApi.Readers;

namespace APIWeaver;

internal sealed class OpenApiDocumentProvider(IOptions<GeneratorConfiguration> options, ILogger logger)
{
    public async Task<OpenApiDocument> GetDocumentAsync()
    {
        var documentPath = options.Value.FullDocumentPath;
        logger.LogInformation("Reading OpenAPI document from {Path}", documentPath);
        await using var fileStream = new FileStream(documentPath, FileMode.Open, FileAccess.Read);

        var reader = new OpenApiStreamReader();
        var result = await reader.ReadAsync(fileStream);

        var diagnostic = result.OpenApiDiagnostic;
        if (diagnostic.Errors.Count > 0 || diagnostic.Warnings.Count > 0)
        {
            logger.LogWarning("The OpenAPI document contains {ErrorCount} errors and {WarningCount} warnings", diagnostic.Errors.Count, diagnostic.Warnings.Count);
        }

        return result.OpenApiDocument;
    }
}