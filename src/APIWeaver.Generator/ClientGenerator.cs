using System.Text;

namespace APIWeaver;

internal sealed class ClientGenerator(ILogger logger, IOptions<GeneratorConfiguration> options, OpenApiDocumentProvider documentProvider)
{
    public async Task GenerateAsync()
    {
        var document = await documentProvider.GetDocumentAsync();

        var operationsByTag = document.GetOperationsByTag();

        logger.LogInformation("Found {TagCount} tags in the OpenAPI document", operationsByTag.Count);

        await Parallel.ForEachAsync(operationsByTag, async (pair, token) =>
        {
            var tag = pair.Key;
            var operations = pair.Value;
            var configuration = options.Value;
            var clientName = configuration.NamePattern.Replace("{tag}", tag);
            var builder = new StringBuilder();
            builder.AppendLine($"namespace {configuration.Namespace};");
            builder.AppendLine();
            builder.AppendLine($"public class {clientName}");
            builder.Append('{');

            var methods = GetMethodData(operations);
            var code = new MethodSourceCodeBuilder().Build(methods);
            builder.AppendLine(code);

            builder.Append('}');

            var fileName = Path.Combine(configuration.OutputPath, $"{clientName}.cs");
            await File.WriteAllTextAsync(fileName, builder.ToString(), Encoding.UTF8, token);
        });
    }

    private static List<Method> GetMethodData(Dictionary<OperationType, OpenApiOperation> operations)
    {
        var methods = new List<Method>();

        foreach (var operation in operations)
        {
            var method = new Method
            {
                Name = GetMethodName(operation.Value),
                GenericResponseTypes = GetResponseTypes(operation.Value).ToList(),
                HttpMethod = operation.Key
            };
            methods.Add(method);
        }

        return methods;
    }

    private static string GetMethodName(OpenApiOperation operation)
    {
        var operationSpan = operation.OperationId.AsSpan();
        var index = operationSpan.LastIndexOf('_');
        if (index != -1)
        {
            var slice = operationSpan[(index + 1)..];
            return $"{slice}Async";
        }

        return operation.OperationId;
    }

    private static IEnumerable<GenericResponseType> GetResponseTypes(OpenApiOperation operation)
    {
        var responseType = operation.Responses;
        return responseType.Where(x => x.Value.Content.ContainsKey("application/json")).Select(x => new GenericResponseType
        {
            Name = x.Key.ToName()
        });
    }
}

internal class Method
{
    public required List<GenericResponseType> GenericResponseTypes { get; init; }

    public required string Name { get; init; }

    public required OperationType HttpMethod { get; init; }
}

internal class GenericResponseType
{
    public required string Name { get; init; }
}