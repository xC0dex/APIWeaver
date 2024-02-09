using APIWeaver.Schema.Models;

namespace APIWeaver.OpenApi.Extensions;

/// <summary>
/// Extension methods for <see cref="OpenApiOptions" />.
/// </summary>
public static class OpenApiOptionsExtensions
{
    /// <summary>
    /// Configures the <see cref="OpenApiSchemaGeneratorOptions" /> for the OpenApiOptions.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="generatorOptions">An action that configures the generator options.</param>
    public static OpenApiOptions WithGeneratorOptions(this OpenApiOptions options, Action<OpenApiGeneratorOptions> generatorOptions)
    {
        generatorOptions.Invoke(options.GeneratorOptions);
        return options;
    }

    /// <summary>
    /// Configures the <see cref="OpenApiSchemaGeneratorOptions" /> for the OpenApiOptions.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="generatorOptions">An action that configures the schema generator options.</param>
    public static OpenApiOptions WithSchemaGeneratorOptions(this OpenApiOptions options, Action<OpenApiSchemaGeneratorOptions> generatorOptions)
    {
        generatorOptions.Invoke(options.SchemaGeneratorOptions);
        return options;
    }


    /// <summary>
    /// Adds an OpenApi document to the OpenApiOptions.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="documentName">The name of the document.</param>
    /// <param name="documentDefinition">An action that defines the document.</param>
    public static OpenApiOptions AddOpenApiDocument(this OpenApiOptions options, string documentName, Action<OpenApiDocumentDefinition> documentDefinition)
    {
        var document = new OpenApiDocumentDefinition();
        documentDefinition.Invoke(document);
        options.OpenApiDocuments.Add(documentName, document);
        return options;
    }

    /// <summary>
    /// Adds an OpenApi document to the OpenApiOption.
    /// </summary>
    /// <param name="options">T<see cref="OpenApiOptions" />.</param>
    /// <param name="documentName">The name of the document.</param>
    /// <param name="documentDefinition">The document definition.</param>
    public static OpenApiOptions AddOpenApiDocument(this OpenApiOptions options, string documentName, OpenApiDocumentDefinition documentDefinition)
    {
        options.OpenApiDocuments.Add(documentName, documentDefinition);
        return options;
    }
}