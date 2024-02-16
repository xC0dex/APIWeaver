using APIWeaver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddApiWeaver(o =>
{
    o.SchemaGeneratorOptions.WithJsonOptionsSource(JsonOptionsSource.ControllerOptions);
});

var app = builder.Build();
app.UseSwaggerUi();

app.Run();