namespace APIWeaver;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class ExampleAttribute(object value): Attribute
{
    internal object Value { get; } = value;
}