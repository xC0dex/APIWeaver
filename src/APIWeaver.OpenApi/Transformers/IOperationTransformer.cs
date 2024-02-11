namespace APIWeaver;

/// <summary>
/// Represents a interface that transforms an <see cref="OpenApiOperation" />.
/// </summary>
public interface IOperationTransformer : ITransformer<OperationContext>;