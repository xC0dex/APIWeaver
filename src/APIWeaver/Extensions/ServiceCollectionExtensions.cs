using Microsoft.Extensions.DependencyInjection;

namespace APIWeaver.Extensions;

/// <summary>
/// Contains extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the APIWeaver services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddApiWeaver(this IServiceCollection services)
    {
        return services;
    }
}