using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

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
    /// Registers a given delegate as a schema transformer on the current <see cref="OpenApiOptions" /> instance.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="transformer">The synchronous delegate representing the schema transformer.</param>
    /// <returns>The <see cref="OpenApiOptions" /> instance for further customization.</returns>
    public static OpenApiOptions AddSchemaTransformer(this OpenApiOptions options, Action<OpenApiSchema, OpenApiSchemaTransformerContext> transformer)
    {
        options.AddSchemaTransformer((operation, context, _) =>
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
                var referenceScheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Id = schemeName, Type = ReferenceType.SecurityScheme } };
                operation.Security ??= [];
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [referenceScheme] = []
                });
            }
        });
        return options;
    }

    /// <summary>
    /// Adds <see cref="StatusCodes.Status401Unauthorized" /> and <see cref="StatusCodes.Status403Forbidden" /> responses to the operation if the operation requires authentication or authorization.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <remarks> This method adds the <see cref="AuthResponseOperationTransformer" /> to the transformers.</remarks>
    public static OpenApiOptions AddAuthResponse(this OpenApiOptions options)
    {
        options.AddOperationTransformer<AuthResponseOperationTransformer>();
        return options;
    }

    /// <summary>
    /// Adds required transformers to support the <see cref="ResponseDescriptionAttribute" />.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    public static OpenApiOptions AddResponseDescriptions(this OpenApiOptions options) => options.AddOperationTransformer<ResponseDescriptionTransformer>();

    /// <summary>
    /// Adds the name of the request body parameter to the <see cref="OpenApiOperation"/> by adding the <c>x-name</c> key to the operation.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    public static OpenApiOptions AddRequestBodyParameterName(this OpenApiOptions options) => options.AddOperationTransformer<RequestBodyParameterNameTransformer>();

    /// <summary>
    /// Adds a server to the OpenAPI document.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="urls">The list of server URLs to add.</param>
    /// <remarks>Existing servers are replaced.</remarks>
    public static OpenApiOptions AddServer(this OpenApiOptions options, [StringSyntax(StringSyntaxAttribute.Uri)] params IEnumerable<string> urls)
    {
        var servers = urls.Select(url => new OpenApiServer
        {
            Url = url
        });
        return options.AddServer(servers);
    }

    /// <summary>
    /// Adds a server to the OpenAPI document.
    /// </summary>
    /// <param name="options"><see cref="OpenApiOptions" />.</param>
    /// <param name="servers">The list of <see cref="OpenApiServer"/> instances to add.</param>
    /// <remarks>Existing servers are replaced.</remarks>
    public static OpenApiOptions AddServer(this OpenApiOptions options, params IEnumerable<OpenApiServer> servers)
    {
        options.AddDocumentTransformer((document, _) =>
        {
            document.Servers ??= [];

            document.Servers = servers.ToList();
        });
        return options;
    }
}