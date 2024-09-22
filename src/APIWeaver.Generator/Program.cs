using System.Diagnostics;
using APIWeaver;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Add global logger
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.SetMinimumLevel(args.Contains("--verbose") ? LogLevel.Debug : LogLevel.Information);
    builder.AddSimpleConsole();
});

var logger = loggerFactory.CreateLogger("APIWeaver.Generator");
services.AddSingleton(logger);

try
{
    var configurationHelper = new ConfigurationHelper(logger);
    var configurationOptions = await configurationHelper.LoadConfigurationAsync(args);
    if (configurationOptions is null)
    {
        return;
    }

    services.AddSingleton(configurationOptions);
    services.AddGenerator();

    await using var provider = services.BuildServiceProvider();

    // Generate clients
    var timestamp = Stopwatch.GetTimestamp();
    await provider.GetRequiredService<ClientGenerator>().GenerateAsync();
    var elapsedTime = Stopwatch.GetElapsedTime(timestamp);
    logger.LogDebug("Generation completed in {Elapsed}ms", (int) elapsedTime.TotalMilliseconds);
}
catch (Exception exception)
{
    logger.LogError(exception, "Failed to generate client");
}