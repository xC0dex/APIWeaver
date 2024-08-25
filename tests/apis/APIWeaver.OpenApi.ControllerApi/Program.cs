using APIWeaver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();
builder.Services.AddApiWeaver(x => x.SchemaGeneratorOptions.WithJsonOptionsSource(JsonOptionsSource.ControllerOptions));

var app = builder.Build();

app.UseOpenApi();

app.MapControllers();

app.Run();

public partial class Program;

public sealed class ControllerApiProgram : Program;