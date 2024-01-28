namespace APIWeaver.OpenApi.Extensions;

/// <summary>
/// Provides extension methods for lists of transformers.
/// </summary>
public static class TransformerListExtensions
{
    /// <summary>
    /// Adds a new transformer to the list.
    /// </summary>
    /// <typeparam name="TContext">The type of the context to be transformed.</typeparam>
    /// <param name="list">The list to which the transformer will be added.</param>
    /// <param name="function">A async function that represents the transformer.</param>
    public static void Add<TContext>(this IList<ITransformer<TContext>> list, Func<TContext, Task> function) where TContext : TransformContext
    {
        list.Add(new InternalTransformer<TContext>(function));
    }

    /// <summary>
    /// Adds a new transformer to the list.
    /// </summary>
    /// <typeparam name="TContext">The type of the context to be transformed.</typeparam>
    /// <param name="list">The list to which the transformer will be added.</param>
    /// <param name="action">An action that represents the transformer.</param>
    public static void Add<TContext>(this IList<ITransformer<TContext>> list, Action<TContext> action) where TContext : TransformContext
    {
        list.Add(context =>
        {
            action.Invoke(context);
            return Task.CompletedTask;
        });
    }
}