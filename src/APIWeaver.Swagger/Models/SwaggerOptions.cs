using APIWeaver.Core;

namespace APIWeaver;

/// <summary>
/// Represents the configuration for Swagger UI.
/// </summary>
public sealed class SwaggerOptions
{
    private string _endpointPrefix = Constants.DefaultEndpointPrefix;

    /// <summary>
    /// Gets or sets the endpoint prefix for the Swagger UI.
    /// Default value is Default is <see cref="Constants.DefaultEndpointPrefix" />.
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