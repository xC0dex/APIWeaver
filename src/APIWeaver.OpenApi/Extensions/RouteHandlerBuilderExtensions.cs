using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace APIWeaver;

/// <summary>
/// Provides extension methods for the <see cref="RouteHandlerBuilder" /> class.
/// </summary>
public static class RouteHandlerBuilderExtensions
{
    /// <summary>
    /// Adds a response description to <see cref="OpenApiOperation" />.
    /// </summary>
    /// <param name="builder"><see cref="RouteHandlerBuilder" />.</param>
    /// <param name="description">The description of the response.</param>
    /// <param name="statusCode">The HTTP status code of the response. Default is <c>200</c>.</param>
    /// <returns>The <see cref="RouteHandlerBuilder" /> with the added metadata.</returns>
    public static RouteHandlerBuilder ResponseDescription(this RouteHandlerBuilder builder, string description, int statusCode = StatusCodes.Status200OK) =>
        builder.WithMetadata(new ResponseDescriptionAttribute(description, statusCode));
}