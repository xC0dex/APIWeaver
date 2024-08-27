namespace APIWeaver;

/// <summary>
/// OAuth configuration for Swagger UI.
/// </summary>
public sealed class OAuth2Options
{
    /// <summary>
    /// The client ID for your application.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// The client secret for your application.
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    /// The realm of the client application.
    /// </summary>
    public string? Realm { get; set; }

    /// <summary>
    /// The application name.
    /// </summary>
    public string? AppName { get; set; }

    /// <summary>
    /// The scope separator. The standard is a space (" "), but some services use a comma (",").
    /// </summary>
    public string ScopeSeparator { get; set; } = " ";

    /// <summary>
    /// String array of initially selected oauth scopes.
    /// </summary>
    public IEnumerable<string> Scopes { get; set; } = [];

    /// <summary>
    /// Additional query parameters added to authorizationUrl and tokenUrl.
    /// </summary>
    public IDictionary<string, string>? AdditionalQueryStringParams { get; set; }

    /// <summary>
    /// If true, the "Authorize" button will be hidden for operations that do not have any OAuth2 scopes.
    /// </summary>
    public bool UseBasicAuthenticationWithAccessCodeGrant { get; set; }
}