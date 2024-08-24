namespace APIWeaver.Swagger;

internal struct Constants
{
    /// <summary>
    /// The name of the initial document.
    /// </summary>
    public const string InitialDocumentName = "v1";

    /// <summary>
    /// The default prefix for endpoints.
    /// </summary>
    public const string DefaultSwaggerRoutePrefix = "swagger";

    public const string DefaultOpenApiRoutePattern = "/openapi/{documentName}.json";
}