namespace APIWeaver;

/// <summary>
/// Extensions for <see cref="ApiWeaverOptions" />.
/// </summary>
public static class ApiWeaverOptionsExtensions
{
    /// <summary>
    /// Adds an example for a specific type.
    /// </summary>
    /// <typeparam name="TExample">The type of the example.</typeparam>
    /// <param name="options"><see cref="ApiWeaverOptions" />.</param>
    /// <param name="example">The example to add.</param>
    /// <exception cref="InvalidOperationException">An example for the type has already been added.</exception>
    public static ApiWeaverOptions AddExample<TExample>(this ApiWeaverOptions options, TExample example)
    {
        var success = options.Examples.TryAdd(typeof(TExample), example);
        if (!success)
        {
            throw new InvalidOperationException($"An example for type '{typeof(TExample).Name}' has already been added.");
        }

        return options;
    }

    /// <summary>
    /// Adds an example for a specific type by using the <see cref="IExampleProvider{TExample}" /> interface.
    /// </summary>
    /// <param name="options"><see cref="ApiWeaverOptions" />.</param>
    /// <typeparam name="TExample">The type of the example.</typeparam>
    /// <typeparam name="TExampleProvider">The implementation of <see cref="IExampleProvider{TExample}" />.</typeparam>
    /// <exception cref="InvalidOperationException">An example for the type has already been added.</exception>
    public static ApiWeaverOptions AddExample<TExample, TExampleProvider>(this ApiWeaverOptions options) where TExampleProvider : IExampleProvider<TExample>
    {
        var example = TExampleProvider.GetExample();
        var success = options.Examples.TryAdd(typeof(TExample), example);
        if (!success)
        {
            throw new InvalidOperationException($"An example for type '{typeof(TExample).Name}' has already been added.");
        }

        return options;
    }
}