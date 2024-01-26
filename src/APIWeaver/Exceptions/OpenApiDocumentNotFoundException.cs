namespace APIWeaver.Exceptions;

/// <summary>
/// Exception thrown when an OpenAPI document was not found.
/// </summary>
public sealed class OpenApiDocumentNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenApiDocumentNotFoundException" /> class.
    /// </summary>
    public OpenApiDocumentNotFoundException(string documentName) : base($"OpenAPI document with name '{documentName}' was not found.")
    {
    }
}