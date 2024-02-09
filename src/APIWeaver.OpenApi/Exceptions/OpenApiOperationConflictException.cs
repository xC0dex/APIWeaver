namespace APIWeaver.OpenApi.Exceptions;

/// <summary>
/// Exception for conflicts in <see cref="OpenApiOperation" />.
/// </summary>
public sealed class OpenApiOperationConflictException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenApiOperationConflictException" /> class.
    /// </summary>
    /// <param name="httpMethod">The HTTP method that caused the conflict.</param>
    public OpenApiOperationConflictException(OperationType httpMethod) : base($"http method '{httpMethod}' already exists.")
    {
    }
}