namespace APIWeaver;

/// <summary>
/// Optional options for the Swagger UI.
/// </summary>
public sealed class AdditionalUiOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether to use dark mode.
    /// </summary>
    public bool DarkMode { get; set; } = true;

    /// <summary>
    /// Optional stylesheets to include in the Swagger UI.
    /// </summary>
    public IList<string> Stylesheets { get; init; } = [];

    /// <summary>
    /// Optional scripts to include in the Swagger UI.
    /// </summary>
    public IList<string> Scripts { get; init; } = [];
}