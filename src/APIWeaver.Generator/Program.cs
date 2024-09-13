using System.Text.Json;
using APIWeaver;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Add global logger
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.SetMinimumLevel(args.Contains("--verbose") ? LogLevel.Debug : LogLevel.Warning);
    builder.AddSimpleConsole();
});

var logger = loggerFactory.CreateLogger("APIWeaver.Generator");
services.AddSingleton(logger);

// Validate arguments
if (args.Length == 0)
{
    logger.LogError("Configuration file path must be provided");
    return;
}

if (!args[0].AsSpan().EndsWith(".json", StringComparison.OrdinalIgnoreCase))
{
    logger.LogError("Configuration file must have a .json extension");
    return;
}

try
{
    // Load configuration
    var configurationPath = args[0];
    await using var fileStream = new FileStream(configurationPath, FileMode.Open, FileAccess.Read);
    var configuration = await JsonSerializer.DeserializeAsync<GeneratorConfiguration>(fileStream, ConfigurationSerializerContext.Default.GeneratorConfiguration);

    if (configuration is null)
    {
        logger.LogError("Failed to deserialize configuration");
        return;
    }

    var configurationOptions = Options.Create(configuration);

    services.AddSingleton(configurationOptions);
    services.AddSingleton<OpenApiDocumentProvider>();
    services.AddSingleton<ClientGenerator>();

    await using var provider = services.BuildServiceProvider();


    await provider.GetRequiredService<ClientGenerator>().GenerateAsync();
}
catch (Exception exception)
{
    logger.LogError(exception, "An error occurred");
}