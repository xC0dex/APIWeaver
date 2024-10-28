namespace APIWeaver;

/// <summary>
/// Options for configuring the API Weaver.
/// </summary>
public sealed class ApiWeaverOptions
{
    /// <summary>
    /// Gets or sets the examples dictionary.
    /// </summary>
    internal Dictionary<Type, object?> Examples { get; } = [];

    /// <summary>
    /// Adds an example for a specific type.
    /// </summary>
    /// <typeparam name="T">The type of the example.</typeparam>
    /// <param name="example">The example to add.</param>
    /// <returns>The <see cref="ApiWeaverOptions" /> so that additional calls can be chained.</returns>
    public ApiWeaverOptions AddExample<T>(T example)
    {
        Examples.Add(typeof(T), example);
        return this;
    }
}