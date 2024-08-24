namespace APIWeaver;

/// <summary>
/// Options for configuring OpenAPI helper.
/// </summary>
public sealed class OpenApiHelperOptions
{
    /// <summary>
    /// Gets the list of OpenAPI document names.
    /// </summary>
    public IList<string> Documents { get; } = [];
}