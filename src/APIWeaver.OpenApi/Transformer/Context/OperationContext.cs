namespace APIWeaver;

/// <summary>
/// Represents the context of an OpenAPI operation.
/// </summary>
/// <param name="OpenApiOperation">The OpenAPI operation for which the context is being created.</param>
/// <param name="ServiceProvider">An instance of <see cref="IServiceProvider" />.</param>
/// <param name="CancellationToken">A <see cref="CancellationToken" />.</param>
public sealed record OperationContext(OpenApiOperation OpenApiOperation, IServiceProvider ServiceProvider, CancellationToken CancellationToken) : TransformContext(ServiceProvider, CancellationToken);