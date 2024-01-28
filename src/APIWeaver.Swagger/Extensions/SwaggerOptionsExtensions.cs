namespace APIWeaver.Swagger.Extensions;

/// <summary>
/// Extension methods for <see cref="SwaggerOptions" />.
/// </summary>
public static class SwaggerOptionsExtensions
{
    /// <summary>
    /// Configures OAuth2 options for the Swagger UI.
    /// </summary>
    /// <param name="swaggerOptions"><see cref="SwaggerOptions" />.</param>
    /// <param name="authOptions">An action to configure the OAuth2 options.</param>
    public static SwaggerOptions WithOAuth2Options(this SwaggerOptions swaggerOptions, Action<OAuth2Options> authOptions)
    {
        swaggerOptions.OAuth2Options ??= new OAuth2Options();
        authOptions.Invoke(swaggerOptions.OAuth2Options);
        return swaggerOptions;
    }

    /// <summary>
    /// Configures Swagger options for the Swagger UI.
    /// </summary>
    /// <param name="swaggerOptions"><see cref="SwaggerOptions" />.</param>
    /// <param name="swaggerUiOptions">An action to configure the Swagger options.</param>
    public static SwaggerOptions WithUiOptions(this SwaggerOptions swaggerOptions, Action<SwaggerUiOptions> swaggerUiOptions)
    {
        swaggerUiOptions.Invoke(swaggerOptions.UiOptions);
        return swaggerOptions;
    }

    /// <summary>
    /// Configures the OpenAPI endpoint for the Swagger UI.
    /// </summary>
    /// <param name="configuration"><see cref="SwaggerOptions" />.</param>
    /// <param name="name">The name of the OpenAPI document.</param>
    /// <param name="endpoint">The URL where the OpenAPI document can be found.</param>
    public static SwaggerOptions WithOpenApiEndpoint(this SwaggerOptions configuration, string name, string endpoint)
    {
        configuration.UiOptions.Urls.Add(new Url(name, endpoint));
        return configuration;
    }

    /// <summary>
    /// Configures the OpenAPI endpoint for the Swagger UI.
    /// </summary>
    /// <param name="configuration"><see cref="SwaggerOptions" />.</param>
    /// <param name="documentName">The name of the OpenAPI document.</param>
    /// <param name="isJson">Whether the OpenAPI document is in JSON format. False if the format is YAML.</param>
    public static SwaggerOptions WithOpenApiDocument(this SwaggerOptions configuration, string documentName, bool isJson = true)
    {
        var endpoint = $"/{configuration.EndpointPrefix}/{documentName}-openapi.{(isJson ? "json" : "yaml")}";
        configuration.UiOptions.Urls.Add(new Url(documentName, endpoint));
        return configuration;
    }
}