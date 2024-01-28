namespace APIWeaver.Swagger.Models;

/// <summary>
/// Represents a URL with a name and a endpoint.
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
    /// Constructs a new Url with the given name and endpoint.
    /// </summary>
    /// <param name="name">The name of the URL.</param>
    /// <param name="endpoint">The endpoint.</param>
    public Url(string name, string endpoint)
    {
        Name = name;
        Endpoint = endpoint;
    }

    /// <summary>
    /// Gets or sets the name of the URL.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the endpoint of the URL.
    /// </summary>
    [JsonPropertyName("url")]
    public string Endpoint { get; set; } = null!;
}