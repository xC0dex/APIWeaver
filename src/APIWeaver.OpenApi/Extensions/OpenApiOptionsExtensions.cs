using APIWeaver.Transformers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace APIWeaver;

/// <summary>
/// Useful extension methods for <see cref="OpenApiOptions" />.
/// </summary>
public static class OpenApiOptionsExtensions
{
    /// <summary>
    /// Registers a given delegate as a document transformer on the current <see cref="OpenApiOptions" /> instance.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="transformer">The synchronous delegate representing the document transformer.</param>
    /// <returns>The <see cref="OpenApiOptions" /> instance for further customization.</returns>
    public static OpenApiOptions AddDocumentTransformer(this OpenApiOptions options, Action<OpenApiDocument, OpenApiDocumentTransformerContext> transformer)
    {
        options.AddDocumentTransformer((document, context, _) =>
        {
            transformer(document, context);
            return Task.CompletedTask;
        });
        return options;
    }

    /// <summary>
    /// Registers a given delegate as an operation transformer on the current <see cref="OpenApiOptions" /> instance.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="transformer">The synchronous delegate representing the operation transformer.</param>
    /// <returns>The <see cref="OpenApiOptions" /> instance for further customization.</returns>
    public static OpenApiOptions AddOperationTransformer(this OpenApiOptions options, Action<OpenApiOperation, OpenApiOperationTransformerContext> transformer)
    {
        options.AddOperationTransformer((operation, context, _) =>
        {
            transformer(operation, context);
            return Task.CompletedTask;
        });
        return options;
    }

    /// <summary>
    /// Adds the given security scheme to the current <see cref="OpenApiOptions" /> instance.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="schemeName">The name of the scheme.</param>
    /// <param name="scheme">The <see cref="OpenApiSecurityScheme" />.</param>
    public static OpenApiOptions AddSecurityScheme(this OpenApiOptions options, string schemeName, OpenApiSecurityScheme scheme)
    {
        options.AddSecurityScheme(schemeName, (_, _, _) => Task.FromResult(scheme));
        return options;
    }

    /// <summary>
    /// Adds the given security scheme to the current <see cref="OpenApiOptions" /> instance.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="schemeName">The name of the scheme.</param>
    /// <param name="scheme">An action to configure the <see cref="OpenApiSecurityScheme" />.</param>
    public static OpenApiOptions AddSecurityScheme(this OpenApiOptions options, string schemeName, Action<OpenApiSecurityScheme> scheme)
    {
        return options.AddSecurityScheme(schemeName, (s, provider, _) =>
        {
            scheme(s);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// Adds the given security scheme to the current <see cref="OpenApiOptions" /> instance.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="schemeName">The name of the scheme.</param>
    /// <param name="factory">A factory to provide the <see cref="OpenApiSecurityScheme" />.</param>
    public static OpenApiOptions AddSecurityScheme(this OpenApiOptions options, string schemeName, Action<OpenApiSecurityScheme, IServiceProvider> factory)
    {
        return options.AddSecurityScheme(schemeName, (scheme, provider, _) =>
        {
            factory(scheme, provider);
            return Task.CompletedTask;
        });
    }

    /// <summary>
    /// Adds the given security scheme to the current <see cref="OpenApiOptions" /> instance.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="schemeName">The name of the scheme.</param>
    /// <param name="asyncFactory">An async factory to provide the <see cref="OpenApiSecurityScheme" />.</param>
    public static OpenApiOptions AddSecurityScheme(this OpenApiOptions options, string schemeName, Func<OpenApiSecurityScheme, IServiceProvider, CancellationToken, Task> asyncFactory)
    {
        options.AddDocumentTransformer(async (document, context, cancellationToken) =>
        {
            var securityScheme = new OpenApiSecurityScheme();
            await asyncFactory(securityScheme, context.ApplicationServices, cancellationToken);
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes.Add(schemeName, securityScheme);
        });

        options.AddOperationTransformer(async (operation, context, _) =>
        {
            var hasAuthorization = await context.HasAuthorizationAsync();
            if (hasAuthorization)
            {
                var referenceScheme = new OpenApiSecurityScheme {Reference = new OpenApiReference {Id = schemeName, Type = ReferenceType.SecurityScheme}};
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [referenceScheme] = []
                });
            }
        });
        return options;
    }
    
    /// <summary>
    /// Adds <see cref="StatusCodes.Status401Unauthorized"/> and <see cref="StatusCodes.Status403Forbidden"/> responses to the operation if the operation requires authentication or authorization.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <remarks> This method adds the <see cref="AuthResponseOperationTransformer"/> to the transformers.</remarks>
    public static OpenApiOptions AddAuthResponse(this OpenApiOptions options)
    {
        options.AddOperationTransformer<AuthResponseOperationTransformer>();
        return options;
    }
}