using APIWeaver.OpenApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiWeaver();

var app = builder.Build();

app.UseOpenApi();

app.MapGet("/", () => "Hello World!").WithOpenApi();

app.Run();


public partial class Program;