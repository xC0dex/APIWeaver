using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace APIWeaver.Transformers.Schema;

internal sealed class ExampleSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;

        if (context.ParameterDescription?.ParameterDescriptor is IParameterInfoParameterDescriptor parameterDescriptor)
        {
            var exampleAttribute = parameterDescriptor.ParameterInfo.GetCustomAttributes<ExampleAttribute>().FirstOrDefault();
            if (exampleAttribute is not null)
            {
                var exampleValue = exampleAttribute.Value;
                if (type == typeof(Guid))
                {
                    exampleValue = Guid.Parse(exampleValue.ToString() ?? throw new InvalidOperationException($"The example value for type {nameof(Guid)} must be a string."));
                }

                ApplyExample(type, exampleValue, schema, context);
                return Task.CompletedTask;
            }
        }

        var apiWeaverOptions = context.ApplicationServices.GetRequiredService<IOptionsMonitor<ApiWeaverOptions>>().Get(context.DocumentName);
        if (apiWeaverOptions.Examples.TryGetValue(type, out var example))
        {
            ApplyExample(type, example, schema, context);
        }

        return Task.CompletedTask;
    }

    private static void ApplyExample(Type type, object? example, OpenApiSchema schema, OpenApiSchemaTransformerContext context)
    {
        var schemaExampleCache = context.ApplicationServices.GetRequiredKeyedService<SchemaExampleCache>(context.DocumentName);
        var openApiExample = schemaExampleCache.Get(type);
        if (openApiExample is not null)
        {
            schema.Example = openApiExample;
        }

        var json = JsonSerializer.Serialize(example, context.JsonTypeInfo);
        using var document = JsonDocument.Parse(json);
        var element = document.RootElement;
        openApiExample = MapToOpenApiType(element);
        if (openApiExample is not null)
        {
            schemaExampleCache.Add(type, openApiExample);
            schema.Example = openApiExample;
        }
    }

    private static IOpenApiAny? MapToOpenApiType(JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                var openApiObject = new OpenApiObject();

                foreach (var child in element.EnumerateObject())
                {
                    var openApiType = MapToOpenApiType(child.Value);
                    if (openApiType is not null)
                    {
                        openApiObject[child.Name] = openApiType;
                    }
                }

                return openApiObject;
            case JsonValueKind.String:
                var value = element.GetString();
                return new OpenApiString(value);
            case JsonValueKind.Number:
                var number = element.GetDouble();
                return new OpenApiDouble(number);
            case JsonValueKind.True:
            case JsonValueKind.False:
                var boolean = element.GetBoolean();
                return new OpenApiBoolean(boolean);
            case JsonValueKind.Array:
                var openApiArray = new OpenApiArray();
                foreach (var item in element.EnumerateArray())
                {
                    var arrayItem = MapToOpenApiType(item);
                    if (arrayItem is not null)
                    {
                        openApiArray.Add(arrayItem);
                    }
                }

                return openApiArray;
            case JsonValueKind.Null:
                return new OpenApiNull();
            case JsonValueKind.Undefined:
            default:
                return null;
        }
    }
}