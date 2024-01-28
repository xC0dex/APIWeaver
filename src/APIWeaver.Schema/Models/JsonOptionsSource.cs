using Microsoft.AspNetCore.Http.Json;

namespace APIWeaver.Schema.Models;

/// <summary>
/// Specifies the source of the JSON options.
/// </summary>
public enum JsonOptionsSource
{
    /// <summary>
    /// The <see cref="JsonOptions" /> used by minimal APIs.
    /// </summary>
    MinimalApiOptions,

    /// <summary>
    /// The <see cref="Microsoft.AspNetCore.Mvc.JsonOptions" /> used by controllers.
    /// </summary>
    ControllerOptions
}