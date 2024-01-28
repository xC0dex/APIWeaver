using Microsoft.OpenApi.Models;

namespace APIWeaver.Exceptions;

/// <summary>
/// Exception thrown when an <see cref="OpenApiOperation" /> is not found.
/// </summary>
public sealed class OpenApiOperationNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenApiOperationNotFoundException" /> class.
    /// </summary>
    public OpenApiOperationNotFoundException(string endpoint) : base($"OpenApiOperation  for endpoint '{endpoint}' not found.")
    {
    }
}