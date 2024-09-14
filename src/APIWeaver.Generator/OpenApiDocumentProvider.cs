using Microsoft.OpenApi.Readers;

namespace APIWeaver;

internal sealed class OpenApiDocumentProvider(IOptions<GeneratorConfiguration> options, ILogger logger)
{
    public async Task<OpenApiDocument> GetDocumentAsync()
    {
        logger.LogInformation("Reading OpenAPI document from {Path}", options.Value.OpenApiDocumentPath);
        await using var fileStream = new FileStream(options.Value.OpenApiDocumentPath, FileMode.Open, FileAccess.Read);

        var reader = new OpenApiStreamReader();
        var result = await reader.ReadAsync(fileStream).ConfigureAwait(false);

        var diagnostic = result.OpenApiDiagnostic;
        if (diagnostic.Errors.Count > 0 || diagnostic.Warnings.Count > 0)
        {
            logger.LogWarning("The OpenAPI document contains {ErrorCount} errors and {WarningCount} warnings", diagnostic.Errors.Count, diagnostic.Warnings.Count);
        }

        return result.OpenApiDocument;
    }
}