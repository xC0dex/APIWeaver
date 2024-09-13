using System.Text;

namespace APIWeaver;

internal sealed class ClientGenerator(ILogger logger, IOptions<GeneratorConfiguration> options, OpenApiDocumentProvider documentProvider)
{
    public async Task GenerateAsync()
    {
        var document = await documentProvider.GetDocumentAsync();

        var operationsByTag = document.GetOperationsByTag();

        logger.LogInformation("Found {TagCount} tags in the OpenAPI document", operationsByTag.Count);

        foreach (var (tag, operations) in operationsByTag)
        {
            var configuration = options.Value;
            var clientName = configuration.NamePattern.Replace("{tag}", tag);
            var clientInterfaceName = $"I{clientName}";
            var builder = new StringBuilder();
            builder.AppendLine($"namespace {configuration.Namespace};");
            builder.AppendLine();
            builder.AppendLine($"public interface {clientInterfaceName};");
            builder.AppendLine();
            builder.AppendLine($"public class {clientName}: {clientInterfaceName}");
            builder.AppendLine("{");

            builder.AppendLine("}");

            var fileName = Path.Combine(configuration.OutputPath, $"{clientName}.cs");
            await File.WriteAllTextAsync(fileName, builder.ToString(), Encoding.UTF8);
        }
    }
    
}