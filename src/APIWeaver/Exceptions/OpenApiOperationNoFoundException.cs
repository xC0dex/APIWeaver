using Microsoft.OpenApi.Models;

namespace APIWeaver.Exceptions;

/// <summary>
/// Exception thrown when an <see cref="OpenApiOperation" /> is not found.
/// </summary>
public sealed class OpenApiOperationNoFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenApiOperationNoFoundException" /> class.
    /// </summary>
    public OpenApiOperationNoFoundException(string route) : base($"OpenApiOperation  for route '{route}' not found.")
    {
    }
}