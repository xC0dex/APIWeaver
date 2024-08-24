using APIWeaver;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddOpenApiDocument();

var app = builder.Build();

app.MapGet("/", () => "Hello World!").WithOpenApi();

if (app.Environment.IsDevelopment())
{
    app.MapSwaggerUi();
}

app.Run();

public partial class Program;