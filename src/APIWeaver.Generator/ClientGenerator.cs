using APIWeaver.Generators.CSharp;

namespace APIWeaver;

internal sealed class ClientGenerator(
    ILogger logger,
    IOptions<GeneratorConfiguration> options,
    OpenApiDocumentProvider documentProvider,
    CSharpClientProcessor cSharpClientProcessor)
{
    public async Task GenerateAsync()
    {
        var document = await documentProvider.GetDocumentAsync();

        logger.LogInformation("Generating client for {Document}", Path.GetFileName(options.Value.FullDocumentPath));

        var cSharpFilesDefinition = cSharpClientProcessor.PrepareClient(document);

        await Parallel.ForEachAsync(cSharpFilesDefinition, async (fileDefinition, token) =>
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("Generating {Name}", fileDefinition.Name);
            }

            var sourceCode = new FileGenerator().Generate(fileDefinition);
            var fileName = Path.Combine(options.Value.FullOutputPath, $"{fileDefinition.Name}.cs");
            var fullPath = Path.GetFullPath(fileName);

            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("Writing to {FileName}", fullPath);
            }

            await File.WriteAllTextAsync(fullPath, sourceCode, Encoding.UTF8, token);
        });
    }
}