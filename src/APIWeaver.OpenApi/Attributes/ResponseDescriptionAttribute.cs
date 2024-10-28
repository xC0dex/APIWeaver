using Microsoft.AspNetCore.Http;

namespace APIWeaver;

/// <summary>
/// Attribute to describe the response of an API endpoint.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class ResponseDescriptionAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResponseDescriptionAttribute"/> class.
    /// </summary>
    /// <param name="description">The description of the response.</param>
    /// <param name="statusCode">The HTTP status code of the response. Default is <c>200</c>.</param>
    public ResponseDescriptionAttribute(string description, int statusCode = StatusCodes.Status200OK)
    {
        StatusCode = statusCode;
        Description = description;
    }
    
    internal int StatusCode { get; }
    
    internal string Description { get; }
}