using Microsoft.AspNetCore.Http;

namespace APIWeaver;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class ResponseDescriptionAttribute : Attribute
{
    public ResponseDescriptionAttribute(string description, int statusCode = StatusCodes.Status200OK)
    {
        StatusCode = statusCode;
        Description = description;
    }

    public int StatusCode { get; }

    public string Description { get; }
}