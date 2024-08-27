namespace APIWeaver;

/// <summary>
/// Options for configuring OpenAPI helper.
/// </summary>
internal sealed class OpenApiHelperOptions
{
    /// <summary>
    /// Gets the list of OpenAPI document names.
    /// </summary>
    public IList<string> Documents { get; } = [];
}