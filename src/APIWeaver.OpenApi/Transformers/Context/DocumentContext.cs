namespace APIWeaver.OpenApi.Context;

/// <summary>
/// Represents the context of an OpenAPI document.
/// </summary>
/// <param name="OpenApiDocument">An instance of the OpenAPI document.</param>
/// <param name="ServiceProvider">An instance of <see cref="IServiceProvider" />.</param>
/// <param name="CancellationToken">A <see cref="CancellationToken" />.</param>
public sealed record DocumentContext(OpenApiDocument OpenApiDocument, IServiceProvider ServiceProvider, CancellationToken CancellationToken) : TransformContext(ServiceProvider, CancellationToken);