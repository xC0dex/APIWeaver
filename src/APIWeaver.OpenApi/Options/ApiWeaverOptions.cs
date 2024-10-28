namespace APIWeaver;

/// <summary>
/// Options for configuring the API Weaver.
/// </summary>
public sealed class ApiWeaverOptions
{
    /// <summary>
    /// Gets or sets the examples dictionary.
    /// </summary>
    internal Dictionary<Type, object?> Examples { get; } = [];
}