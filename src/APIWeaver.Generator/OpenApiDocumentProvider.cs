using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace APIWeaver;

internal sealed class OpenApiDocumentProvider(IOptions<GeneratorConfiguration> options, ILogger<OpenApiDocumentProvider> logger)
{
    public OpenApiDocument GetDocument()
    {
        try
        {
            logger.LogInformation("Reading OpenAPI document from {Path}", options.Value.OpenApiDocumentPath);
            using var fileStream = new FileStream(options.Value.OpenApiDocumentPath, FileMode.Open, FileAccess.Read);

            var reader = new OpenApiStreamReader();
            var result = reader.Read(fileStream, out var diagnostic);

            if (diagnostic.Errors.Count > 0 || diagnostic.Warnings.Count > 0)
            {
                logger.LogWarning("The OpenAPI document contains {ErrorCount} errors and {WarningCount} warnings", diagnostic.Errors.Count, diagnostic.Warnings.Count);
            }

            return result;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to read OpenAPI document");
            throw;
        }
    }
}