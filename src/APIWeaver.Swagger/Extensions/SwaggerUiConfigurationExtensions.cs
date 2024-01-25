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
        authOptions.Invoke(swaggerUiConfiguration.OAuth2Options ?? new OAuth2Options());
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
}