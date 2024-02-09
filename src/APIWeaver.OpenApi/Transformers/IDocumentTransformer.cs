namespace APIWeaver.OpenApi.Transformers;

/// <summary>
/// Represents a interface that transforms an <see cref="OpenApiDocument" />.
/// </summary>
public interface IDocumentTransformer : ITransformer<DocumentContext>;