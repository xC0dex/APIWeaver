namespace APIWeaver.Swagger.Extensions;

/// <summary>
/// Extension methods for <see cref="SwaggerUiConfiguration" />.
/// </summary>
public static class SwaggerUiConfigurationExtensions
{
    /// <summary>
    /// Configures OAuth2 options for the Swagger UI.
    /// </summary>
    /// <param name="swaggerUiConfiguration"><see cref="SwaggerUiConfiguration" />.</param>
    /// <param name="authOptions">An action to configure the OAuth2 options.</param>
    public static SwaggerUiConfiguration WithOAuth2Options(this SwaggerUiConfiguration swaggerUiConfiguration, Action<OAuth2Options> authOptions)
    {
        swaggerUiConfiguration.OAuth2Options ??= new OAuth2Options();
        authOptions.Invoke(swaggerUiConfiguration.OAuth2Options);
        return swaggerUiConfiguration;
    }

    /// <summary>
    /// Configures Swagger options for the Swagger UI.
    /// </summary>
    /// <param name="swaggerUiConfiguration"><see cref="SwaggerUiConfiguration" />.</param>
    /// <param name="swaggerOptions">An action to configure the Swagger options.</param>
    public static SwaggerUiConfiguration WithSwaggerOptions(this SwaggerUiConfiguration swaggerUiConfiguration, Action<SwaggerOptions> swaggerOptions)
    {
        swaggerOptions.Invoke(swaggerUiConfiguration.SwaggerOptions);
        return swaggerUiConfiguration;
    }

    /// <summary>
    /// Configures the OpenAPI endpoint for the Swagger UI.
    /// </summary>
    /// <param name="configuration"><see cref="SwaggerUiConfiguration" />.</param>
    /// <param name="name">The name of the OpenAPI document.</param>
    /// <param name="url">The URL where the OpenAPI document can be found.</param>
    public static SwaggerUiConfiguration WithOpenApiEndpoint(this SwaggerUiConfiguration configuration, string name, string url)
    {
        configuration.SwaggerOptions.Urls.Add(new Url(name, url));
        return configuration;
    }

    /// <summary>
    /// Configures the OpenAPI endpoint for the Swagger UI.
    /// </summary>
    /// <param name="configuration"><see cref="SwaggerUiConfiguration" />.</param>
    /// <param name="documentName">The name of the OpenAPI document.</param>
    /// <param name="isJson">Whether the OpenAPI document is in JSON format. False if the format is YAML.</param>
    public static SwaggerUiConfiguration WithOpenApiDocument(this SwaggerUiConfiguration configuration, string documentName, bool isJson = true)
    {
        var route = $"/{configuration.EndpointPrefix}/{documentName}-openapi.{(isJson ? "json" : "yaml")}";
        configuration.SwaggerOptions.Urls.Add(new Url(documentName, route));
        return configuration;
    }
}