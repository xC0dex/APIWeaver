using APIWeaver.Swagger;

namespace APIWeaver;

/// <summary>
/// Represents the configuration for Swagger UI.
/// </summary>
public sealed class SwaggerOptions
{
    private string _routePrefix = Constants.DefaultSwaggerRoutePrefix;

    /// <summary>
    /// Gets or sets the endpoint prefix for the Swagger UI.
    /// Default value is Default is <see cref="Constants.DefaultSwaggerRoutePrefix" />.
    /// </summary>
    [JsonIgnore]
    public string RoutePrefix
    {
        get => _routePrefix;
        set => _routePrefix = value.Trim('/');
    }

    /// <summary>
    /// Gets or sets the route pattern of the OpenAPI documents.
    /// Default value is <see cref="Constants.DefaultOpenApiRoutePattern" />.
    /// </summary>
    [JsonIgnore]
    public string OpenApiRoutePattern { get; set; } = Constants.DefaultOpenApiRoutePattern;

    /// <summary>
    /// Gets or sets the title for the Swagger UI.
    /// Default value is the application name followed by " | Swagger UI".
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the options for the Swagger UI.
    /// </summary>
    public SwaggerUiOptions UiOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the OAuth2 options for the Swagger UI.
    /// This is optional.
    /// </summary>
    public OAuth2Options? OAuth2Options { get; set; }

    /// <summary>
    /// Gets or sets the additional options for the Swagger UI.
    /// </summary>
    public AdditionalUiOptions AdditionalUiOptions { get; set; } = new();
}

[JsonSerializable(typeof(SwaggerOptions))]
[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
internal sealed partial class SwaggerOptionsSerializerContext : JsonSerializerContext;