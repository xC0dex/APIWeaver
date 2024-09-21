using System.Text;

namespace APIWeaver;

internal sealed class ClientGenerator(ILogger logger, IOptions<GeneratorConfiguration> options, OpenApiDocumentProvider documentProvider, ResponseCache responseCache)
{
    public async Task GenerateAsync()
    {
        var document = await documentProvider.GetDocumentAsync();

        var operationsByTag = document.GetOperationsByTag();

        logger.LogInformation("Found {TagCount} tags in the OpenAPI document", operationsByTag.Count);

        var configuration = options.Value;
        await Parallel.ForEachAsync(operationsByTag, async (pair, token) =>
        {
            var tag = pair.Key;
            var operations = pair.Value;
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

            var fileName = Path.Combine(configuration.FullOutputPath, $"{clientName}.cs");
            await File.WriteAllTextAsync(fileName, builder.ToString(), Encoding.UTF8, token);
        });

        await GenerateResponseTypeAsync(configuration);
    }

    private async Task GenerateResponseTypeAsync(GeneratorConfiguration configuration)
    {
        var responseTypes = responseCache.GetUniqueCombinations();

        var builder = new StringBuilder();
        builder.AppendLine("using System.Net;");
        builder.AppendLine();
        builder.AppendLine($"namespace {configuration.Namespace};");

        foreach (var responseType in responseTypes)
        {
            builder.AppendLine();
            builder.Append("public readonly struct Response");
            builder.Append('<');
            for (var i = 0; i < responseType.Length; i++)
            {
                builder.Append($"T{responseType[i]}");
                if (i < responseType.Length - 1)
                {
                    builder.Append(", ");
                }
            }

            builder.Append('>');
            builder.AppendLine();
            builder.Append('{');

            builder.AppendLine();
            const int indent = 1;

            builder.AppendIndentLine("public bool IsSuccess => (int) StatusCode is >= 200 and < 300;", indent);
            builder.AppendIndentLine(indent);
            builder.AppendIndentLine("public required HttpStatusCode StatusCode { get; init; }", indent);
            builder.AppendIndentLine(indent);
            for (var i = 0; i < responseType.Length; i++)
            {
                builder.AppendIndent("public ", indent);
                builder.Append($"T{responseType[i]}?");
                builder.Append($" {responseType[i]}");
                builder.Append(" { get; init; }");
                builder.AppendIndentLine(indent);
                builder.AppendIndentLine(indent);
            }

            builder.AppendIndentLine("public Stream? BodyStream { get; init; }", 1);

            builder.Append('}');
            builder.AppendLine();
        }

        var fileName = Path.Combine(configuration.FullOutputPath, "Response.cs");
        await File.WriteAllTextAsync(fileName, builder.ToString(), Encoding.UTF8);
    }

    private List<Method> GetMethodData(Dictionary<OperationType, OpenApiOperation> operations)
    {
        var methods = new List<Method>();

        foreach (var operation in operations)
        {
            var responseTypes = GetResponseTypes(operation.Value).ToArray();
            responseCache.Add(responseTypes);
            var method = new Method
            {
                Name = GetMethodName(operation.Value),
                ResponseTypes = responseTypes,
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

    private static IEnumerable<string> GetResponseTypes(OpenApiOperation operation)
    {
        var responseType = operation.Responses;
        return responseType.Where(x => x.Value.Content.ContainsKey("application/json")).Select(x => x.Key.ToName());
    }
}

internal class Method
{
    public required string[] ResponseTypes { get; init; }

    public required string Name { get; init; }

    public required OperationType HttpMethod { get; init; }
}