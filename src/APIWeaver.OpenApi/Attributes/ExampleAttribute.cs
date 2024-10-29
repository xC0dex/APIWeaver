namespace APIWeaver;

/// <summary>
/// An attribute to provide an example value for a parameter.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public sealed class ExampleAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleAttribute"/> class with the specified example value.
    /// </summary>
    /// <param name="value">The example value for the parameter.</param>
    public ExampleAttribute(object value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the example value for the parameter.
    /// </summary>
    internal object Value { get; }
}