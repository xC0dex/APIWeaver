namespace APIWeaver;

/// <summary>
/// Provides an example of a specific type.
/// </summary>
/// <typeparam name="TExample">The type of the example.</typeparam>
public interface IExampleProvider<out TExample>
{
    /// <summary>
    /// Gets an example of the specified type.
    /// </summary>
    /// <returns>An example of type <typeparamref name="TExample" />.</returns>
    static abstract TExample GetExample();
}