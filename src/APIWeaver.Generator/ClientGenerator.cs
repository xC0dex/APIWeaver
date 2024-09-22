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

        var cSharpFilesDefinition = cSharpClientProcessor.PrepareClient(document);

        await Parallel.ForEachAsync(cSharpFilesDefinition, async (fileDefinition, token) =>
        {
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug("Generating client for {Name}", fileDefinition.Name);
            }
            var sourceCode = new CSharpFileBuilder().Build(fileDefinition);
            var fileName = Path.Combine(options.Value.FullOutputPath, $"{fileDefinition.Name}.cs");
            await File.WriteAllTextAsync(fileName, sourceCode, Encoding.UTF8, token);
        });
    }
}