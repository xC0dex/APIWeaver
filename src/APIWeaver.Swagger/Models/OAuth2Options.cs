namespace APIWeaver;

/// <summary>
/// OAuth configuration for Swagger UI.
/// </summary>
public sealed class OAuth2Options
{
    /// <summary>
    /// The client ID for your application. You must first create an application at the service provider and obtain this value.
    /// </summary>
    public string ClientId { get; set; } = null!;

    /// <summary>
    /// The client secret for your application. You must first create an application at the service provider and obtain this value.
    /// </summary>
    public string ClientSecret { get; set; } = null!;

    /// <summary>
    /// The realm of the client application.
    /// </summary>
    public string Realm { get; set; } = null!;

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
    public Dictionary<string, string> AdditionalQueryStringParams { get; set; } = new();

    /// <summary>
    /// If true, the "Authorize" button will be hidden for operations that do not have any OAuth2 scopes.
    /// </summary>
    public bool UseBasicAuthenticationWithAccessCodeGrant { get; set; }
}