using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace APIWeaver;

/// <summary>
/// A document transformer that adds a bearer security scheme to the operation.
/// </summary>
public sealed class BearerSecuritySchemeOperationTransformer : IOpenApiOperationTransformer
{
    /// <inheritdoc />
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var authorizeAttributes = context.Description.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>();
        var anonymousAttributes = context.Description.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>();
        if (authorizeAttributes.Any() && !anonymousAttributes.Any())
        {
            AddSecurityReference(operation);
            operation.Responses.Add("401", new OpenApiResponse
            {
                Description = "Unauthorized - A valid bearer token is required."
            });
        }

        return Task.CompletedTask;
    }

    private static void AddSecurityReference(OpenApiOperation operation)
    {
        var schema = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        };
        var requirement = new OpenApiSecurityRequirement
        {
            [schema] = []
        };
        operation.Security.Add(requirement);
    }
}

public sealed class BearerSecuritySchemeOperationTransformerTest : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var requirements = new Dictionary<string, OpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("https://example.com/oauth2/authorize")
                    }
                },
                Type = SecuritySchemeType.OAuth2,
                Scheme = "Bearer",
                In = ParameterLocation.Header
            }
        };
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = requirements;
        return Task.CompletedTask;
    }
}