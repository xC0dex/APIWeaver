using System.Collections;

namespace APIWeaver.Schema.Extensions;

internal static class TypeExtensions
{
    public static bool IsEnumerable(this Type type) => typeof(IEnumerable).IsAssignableFrom(type) || typeof(IAsyncEnumerable<>).IsAssignableFrom(type);

    public static bool IsSet(this Type type) => typeof(ISet<>).IsAssignableFrom(type) || Array.Exists(type.GetInterfaces(), IsGenericSetType);

    public static bool IsDictionary(this Type type) => typeof(IDictionary).IsAssignableFrom(type) || typeof(IDictionary<,>).IsAssignableFrom(type) || typeof(IReadOnlyDictionary<,>).IsAssignableFrom(type);

    public static bool HasStringEnumConverterAttribute(this Type type) => type.GetCustomAttribute<JsonConverterAttribute>()?.ConverterType == typeof(JsonStringEnumConverter);

    public static bool IsUndefined(this Type type) => type == typeof(JsonElement) || type == typeof(JsonDocument) || type == typeof(object);

    private static bool IsGenericSetType(Type type) => type.IsGenericType && typeof(ISet<>) == type.GetGenericTypeDefinition();
}