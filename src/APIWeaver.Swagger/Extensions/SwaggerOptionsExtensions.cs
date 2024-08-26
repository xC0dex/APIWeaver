namespace APIWeaver;

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
    /// Sets the <see cref="SwaggerOptions.Title" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerOptions" />.</param>
    /// <param name="title">The title value to set.</param>
    public static SwaggerOptions WithTitle(this SwaggerOptions options, string title)
    {
        options.Title = title;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerOptions.DarkMode" /> property.
    /// </summary>
    /// <param name="options"><see cref="SwaggerOptions" />.</param>
    /// <param name="darkMode">The DarkMode value to set.</param>
    public static SwaggerOptions WithDarkMode(this SwaggerOptions options, bool darkMode)
    {
        options.DarkMode = darkMode;
        return options;
    }

    /// <summary>
    /// Sets the <see cref="SwaggerOptions.OpenApiRoutePattern" /> property.
    /// </summary>
    /// <param name="options">
    /// <see cref="SwaggerOptions" />
    /// </param>
    /// <param name="routePattern">The OpenAPI route pattern to set.</param>
    public static SwaggerOptions WithOpenApiRoutePattern(this SwaggerOptions options, string routePattern)
    {
        options.OpenApiRoutePattern = routePattern;
        return options;
    }

    /// <summary>
    /// Configures the OpenAPI endpoint for the Swagger UI.
    /// </summary>
    /// <param name="options"><see cref="SwaggerOptions" />.</param>
    /// <param name="name">The name of the OpenAPI document.</param>
    /// <param name="route">The URL where the OpenAPI document can be found.</param>
    public static SwaggerOptions AddOpenApiDocument(this SwaggerOptions options, string name, string route)
    {
        options.Urls.Add(new Url(name, route));
        return options;
    }

    /// <summary>
    /// </summary>
    /// <param name="options"><see cref="SwaggerOptions" />.</param>
    /// <param name="stylesheet">The stylesheet to add.</param>
    public static SwaggerOptions AddStylesheet(this SwaggerOptions options, string stylesheet)
    {
        options.Stylesheets.Add(stylesheet);
        return options;
    }

    /// <summary>
    /// Adds a script to the Swagger UI.
    /// </summary>
    /// <param name="options"><see cref="SwaggerOptions" />.</param>
    /// <param name="script">The script to add.</param>
    public static SwaggerOptions AddScript(this SwaggerOptions options, string script)
    {
        options.Scripts.Add(script);
        return options;
    }
}