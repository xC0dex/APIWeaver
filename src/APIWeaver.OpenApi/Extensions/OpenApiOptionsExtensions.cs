using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace APIWeaver;

/// <summary>
/// Useful extension methods for <see cref="OpenApiOptions" />.
/// </summary>
public static class OpenApiOptionsExtensions
{
    /// <summary>
    /// Registers a given delegate as a document transformer on the current <see cref="OpenApiOptions" /> instance.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="transformer">The synchronous delegate representing the document transformer.</param>
    /// <returns>The <see cref="OpenApiOptions" /> instance for further customization.</returns>
    public static OpenApiOptions AddDocumentTransformer(this OpenApiOptions options, Action<OpenApiDocument, OpenApiDocumentTransformerContext> transformer)
    {
        options.AddDocumentTransformer((document, context, _) =>
        {
            transformer(document, context);
            return Task.CompletedTask;
        });
        return options;
    }

    /// <summary>
    /// Registers a given delegate as an operation transformer on the current <see cref="OpenApiOptions" /> instance.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="transformer">The synchronous delegate representing the operation transformer.</param>
    /// <returns>The <see cref="OpenApiOptions" /> instance for further customization.</returns>
    public static OpenApiOptions AddOperationTransformer(this OpenApiOptions options, Action<OpenApiOperation, OpenApiOperationTransformerContext> transformer)
    {
        options.AddOperationTransformer((operation, context, _) =>
        {
            transformer(operation, context);
            return Task.CompletedTask;
        });
        return options;
    }
}