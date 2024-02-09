namespace APIWeaver.Core.Transformer;

/// <summary>
/// Represents a generic interface for a transform operation.
/// </summary>
public interface ITransformer<in TContext> where TContext : TransformContext
{
    /// <summary>
    /// Transforms asynchronously.
    /// </summary>
    /// <param name="context">The context for the transformation.</param>
    public Task TransformAsync(TContext context);
}