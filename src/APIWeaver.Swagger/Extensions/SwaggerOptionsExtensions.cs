namespace APIWeaver.Swagger.Extensions;

/// <summary>
/// Extension methods for <see cref="SwaggerOptions" />.
/// </summary>
public static class SwaggerOptionsExtensions
{
    /// <summary>
    /// Configures the OpenAPI endpoint for the Swagger UI.
    /// </summary>
    /// <param name="options"><see cref="SwaggerOptions" />.</param>
    /// <param name="name">The name of the OpenAPI document.</param>
    /// <param name="url">The URL where the OpenAPI document can be found.</param>
    public static SwaggerOptions WithOpenApiEndpoint(this SwaggerOptions options, string name, string url)
    {
        options.Urls.Add(new Url(name, url));
        return options;
    }
}