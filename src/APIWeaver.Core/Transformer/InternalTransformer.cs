namespace APIWeaver.Core.Transformer;

internal sealed class InternalTransformer<TContext>(Func<TContext, Task> func) : ITransformer<TContext> where TContext : TransformContext
{
    public Task TransformAsync(TContext context) => func.Invoke(context);
}