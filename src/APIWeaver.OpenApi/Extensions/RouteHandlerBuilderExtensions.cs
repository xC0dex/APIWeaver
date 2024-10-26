using Microsoft.AspNetCore.Builder;

namespace APIWeaver;

internal static class RouteHandlerBuilderExtensions
{
    public static IEndpointConventionBuilder WithExample(this IEndpointConventionBuilder builder)
    {
        builder.WithMetadata(ExampleMetadata.Default);
        return builder;
    }

    public static IEndpointConventionBuilder WithExample<T>(this IEndpointConventionBuilder builder, Action<T> configureExample) where T : new()
    {
        var x = new T();
        configureExample.Invoke(x);
        return builder;
    }
}

internal sealed class ExampleMetadata
{
    public static readonly ExampleMetadata Default = new();

    private ExampleMetadata()
    {
    }
}