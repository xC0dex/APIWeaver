namespace APIWeaver.Core.Transformer;

/// <summary>
/// Represents a context that can be transformed.
/// </summary>
/// <param name="ServiceProvider">An instance of <see cref="IServiceProvider" />.</param>
/// <param name="CancellationToken">A <see cref="CancellationToken" />.</param>
public abstract record TransformContext(IServiceProvider ServiceProvider, CancellationToken CancellationToken);