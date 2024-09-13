using System.Text.Json;
using APIWeaver;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

if (args.Length != 1)
{
    throw new ArgumentException("Expected a single argument with the path to the configuration file");
}

var configurationPath = args[0];
using var stream = new FileStream(configurationPath, FileMode.Open, FileAccess.Read);
var configuration = JsonSerializer.Deserialize<GeneratorConfiguration>(stream, ConfigurationSerializerContext.Default.GeneratorConfiguration) ?? throw new InvalidOperationException("Failed to read configuration");

var configurationOptions = Options.Create(configuration);

var services = new ServiceCollection();
services.AddSingleton(configurationOptions);
services.AddSingleton<OpenApiDocumentProvider>();
services.AddSingleton<ClientGenerator>();
services.AddConsoleLogging();

using var provider = services.BuildServiceProvider();

provider.GetRequiredService<ClientGenerator>().Generate();