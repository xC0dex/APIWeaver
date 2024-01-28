namespace APIWeaver.Swagger.Models;

/// <summary>
/// Options for the Swagger UI.
/// <see href="https://github.com">GitHub</see>
/// The options are based on <a href="https://github.com/swagger-api/swagger-ui/blob/master/docs/usage/configuration.md">Swagger UI Configuration</a>.
/// </summary>
public sealed class SwaggerUiOptions
{
    /// <summary>
    /// Represents a collection of URLs for the OpenAPI documentation.
    /// </summary>
    public IList<Url> Urls { get; set; } = [];

    /// <summary>
    /// If set to true, enables deep linking for tags and operations.
    /// </summary>
    public bool DeepLinking { get; set; }

    /// <summary>
    /// Controls the display of operationId in operations list.
    /// </summary>
    public bool DisplayOperationId { get; set; }

    /// <summary>
    /// The default expansion depth for models (set to -1 completely hide the models).
    /// </summary>
    public int DefaultModelsExpandDepth { get; set; } = 1;

    /// <summary>
    /// The default expansion depth for the model on the model-example section.
    /// </summary>
    public int DefaultModelExpandDepth { get; set; } = 1;

    /// <summary>
    /// Controls the display of the request duration (in milliseconds) for "Try it out" requests.
    /// </summary>
    public bool DisplayRequestDuration { get; set; }


    /// <summary>
    /// If set, limits the number of tagged operations displayed to at most this many.
    /// </summary>
    public int? MaxDisplayedTags { get; set; }

    /// <summary>
    /// Controls the display of vendor extension (x-) fields and values for Operations, Parameters, Responses, and Schema.
    /// </summary>
    public bool ShowExtensions { get; set; }

    /// <summary>
    /// Controls the display of extensions (pattern, maxLength, minLength, maximum, minimum) fields and values for Parameters.
    /// </summary>
    public bool ShowCommonExtensions { get; set; }

    /// <summary>
    /// Controls whether the "Try it out" section should be enabled by default.
    /// </summary>
    public bool TryItOutEnabled { get; set; }

    /// <summary>
    /// Enables the request snippet section. When disabled, the legacy curl snippet will be used.
    /// </summary>
    public bool RequestSnippetsEnabled { get; set; }

    /// <summary>
    /// OAuth redirect URL.
    /// </summary>
    [JsonPropertyName("oauth2RedirectUrl")]
    public string OAuth2RedirectUrl { get; set; } = "oauth2-redirect.html";

    /// <summary>
    /// Set a different validator URL.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string? ValidatorUrl { get; set; }
}