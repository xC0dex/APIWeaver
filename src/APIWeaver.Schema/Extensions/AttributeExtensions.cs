namespace APIWeaver.Schema.Extensions;

internal static class AttributeExtensions
{
    public static bool ContainsStringEnumConverterAttribute(this IEnumerable<Attribute> attributes)
    {
        return attributes.OfType<JsonConverterAttribute>().Any(x => x.ConverterType == typeof(JsonStringEnumConverter));
    }
}