namespace APIWeaver.OpenApi.Models;

/// <summary>
/// Options for the OpenApi generator.
/// </summary>
public sealed class OpenApiGeneratorOptions
{
    /// <summary>
    /// A list of <see cref="IOperationTransformer" /> instances.
    /// Each instance can be used to transform an <see cref="OpenApiOperation" />.
    /// </summary>
    public IList<ITransformer<OperationContext>> OperationTransformers { get; } = [];

    /// <summary>
    /// A list of <see cref="IDocumentTransformer" /> instances.
    /// Each instance can be used to transform an <see cref="OpenApiDocument" />.
    /// </summary>
    public IList<ITransformer<DocumentContext>> DocumentTransformers { get; } = [];

    /// <summary>
    /// A list of <see cref="ITransformer{ServerContext}" /> instances or functions.
    /// Each instance can be used to transform an <see cref="OpenApiServer" />.
    /// </summary>
    public IList<ITransformer<ServerContext>> ServerTransformers { get; } = [];
}