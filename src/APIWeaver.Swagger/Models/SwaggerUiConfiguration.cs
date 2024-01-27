namespace APIWeaver.Swagger.Models;

/// <summary>
/// Represents the configuration for Swagger UI.
/// </summary>
public sealed class SwaggerUiConfiguration
{
    private string _endpointPrefix = "swagger";

    /// <summary>
    /// Gets or sets the route prefix for the Swagger UI.
    /// Default value is "swagger".
    /// </summary>
    [JsonIgnore]
    public string EndpointPrefix
    {
        get => _endpointPrefix;
        set => _endpointPrefix = value.Trim('/');
    }

    /// <summary>
    /// Gets or sets the title for the Swagger UI.
    /// Default value is "Swagger UI".
    /// </summary>
    public string Title { get; set; } = "Swagger UI";

    /// <summary>
    /// Gets or sets the Swagger options for the Swagger UI.
    /// </summary>
    public SwaggerOptions SwaggerOptions { get; set; } = new();

    /// <summary>
    /// Gets or sets the OAuth2 options for the Swagger UI.
    /// This is optional.
    /// </summary>
    public OAuth2Options? OAuth2Options { get; set; }
}