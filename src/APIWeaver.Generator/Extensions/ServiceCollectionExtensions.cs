using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace APIWeaver;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddConsoleLogging(this IServiceCollection services)
    {
        services.AddLogging(x =>
        {
            x.SetMinimumLevel(LogLevel.Information);
            x.AddSimpleConsole();
        });
        return services;
    }
}