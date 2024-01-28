namespace APIWeaver.OpenApi.Transformers;

/// <summary>
/// Represents a interface that transforms an <see cref="OpenApiOperation" />.
/// </summary>
public interface IOperationTransformer : ITransformer<OperationContext>;