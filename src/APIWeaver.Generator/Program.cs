using System.Text.Json;
using APIWeaver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var services = new ServiceCollection();

// Add global logger
using var loggerFactory = LoggerFactory.Create(x =>
{
    x.SetMinimumLevel(args.Contains("--verbose") ? LogLevel.Debug : LogLevel.Warning);
    x.AddSimpleConsole();
});

var logger = loggerFactory.CreateLogger("APIWeaver.Generator");
services.AddSingleton(logger);

// Validate arguments
if (args.Length == 0)
{
    logger.LogError("Configuration file path must be provided");
    return;
}

if (!args[0].EndsWith(".json", StringComparison.OrdinalIgnoreCase))
{
    logger.LogError("Configuration file must have a .json extension");
    return;
}

// Load configuration
var configurationPath = args[0];
using var fileStream = new FileStream(configurationPath, FileMode.Open, FileAccess.Read);
var configuration = JsonSerializer.Deserialize<GeneratorConfiguration>(fileStream, ConfigurationSerializerContext.Default.GeneratorConfiguration);

if (configuration is null)
{
    logger.LogError("Failed to deserialize configuration");
    return;
}

var configurationOptions = Options.Create(configuration);

services.AddSingleton(configurationOptions);
services.AddSingleton<OpenApiDocumentProvider>();
services.AddSingleton<ClientGenerator>();

using var provider = services.BuildServiceProvider();

provider.GetRequiredService<ClientGenerator>().Generate();