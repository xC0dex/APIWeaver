namespace APIWeaver.Swagger.Models;

/// <summary>
/// Represents a URL with a name and a route.
/// </summary>
public sealed class Url
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public Url()
    {
    }

    /// <summary>
    /// Constructs a new Url with the given name and route.
    /// </summary>
    /// <param name="name">The name of the URL.</param>
    /// <param name="route">The route of the URL.</param>
    public Url(string name, string route)
    {
        Name = name;
        Route = route;
    }

    /// <summary>
    /// Gets or sets the name of the URL.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the route of the URL.
    /// </summary>
    [JsonPropertyName("url")]
    public string Route { get; set; } = null!;
}