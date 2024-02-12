namespace APIWeaver;

/// <summary>
/// Represents the context of an OpenAPI server.
/// </summary>
/// <param name="OpenApiServer">The OpenAPI server for which the context is being created.</param>
/// <param name="ServiceProvider">An instance of <see cref="IServiceProvider" />.</param>
/// <param name="CancellationToken">A <see cref="CancellationToken" />.</param>
public sealed record ServerContext(OpenApiServer OpenApiServer, IServiceProvider ServiceProvider, CancellationToken CancellationToken) : TransformContext(ServiceProvider, CancellationToken);