namespace APIWeaver.Swagger.Exceptions;

/// <summary>
/// Represents an exception that is thrown when there is a mismatch in the OpenAPI document.
/// </summary>
public sealed class OpenApiDocumentMismatchException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenApiDocumentMismatchException" />.
    /// </summary>
    public OpenApiDocumentMismatchException(int openApiDocumentCount, int swaggerUiDocumentCount) : base(
        $"There are {openApiDocumentCount} OpenAPI documents and {swaggerUiDocumentCount} Swagger UI documents registered. Either remove all Swagger UI documents or add the missing ones to the {nameof(SwaggerOptions.UiOptions.Urls)}.")
    {
    }
}