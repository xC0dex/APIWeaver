using System.Text.Json;

namespace APIWeaver;

internal sealed class ConfigurationHelper(ILogger logger)
{
    internal async Task<IOptions<GeneratorConfiguration>?> LoadConfigurationAsync(string[] args)
    {
        if (args.Length == 0)
        {
            logger.LogError("Configuration file path must be provided");
            return null;
        }

        if (!args[0].AsSpan().EndsWith(".json", StringComparison.OrdinalIgnoreCase))
        {
            logger.LogError("Configuration file must have a .json extension");
            return null;
        }

        // Load configuration
        var configurationPath = args[0];
        await using var fileStream = new FileStream(configurationPath, FileMode.Open, FileAccess.Read);
        var configuration = await JsonSerializer.DeserializeAsync<GeneratorConfiguration>(fileStream, ConfigurationSerializerContext.Default.GeneratorConfiguration);

        if (configuration is null)
        {
            logger.LogError("Failed to deserialize configuration");
            return null;
        }

        var executionContext = Path.GetDirectoryName(Path.GetFullPath(configurationPath));

        if (executionContext is null)
        {
            logger.LogError("Failed to determine execution context");
            return null;
        }

        configuration.ExecutionContext = executionContext;

        return Options.Create(configuration);
    }
}