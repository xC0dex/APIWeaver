using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIWeaver.OpenApi.TestApi.Controllers;

[ApiController]
[Route("api")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private static readonly WeatherForecast[] Forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

    [HttpGet("default")]
    public IEnumerable<WeatherForecast> GetDefault() => Forecasts;

    [HttpGet("authorize")]
    [Authorize]
    public IEnumerable<WeatherForecast> GetAuthorize() => Forecasts;

    [HttpGet("anonymous")]
    [AllowAnonymous]
    public IEnumerable<WeatherForecast> GetAnonymous() => Forecasts;

    [HttpGet("role")]
    [Authorize(Roles = "role")]
    public IEnumerable<WeatherForecast> GetRoles() => Forecasts;
}