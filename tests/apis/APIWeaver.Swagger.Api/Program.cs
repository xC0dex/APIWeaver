
using APIWeaver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApiWeaver();

var app = builder.Build();

app.MapGet("/", () => "Hello World!").WithOpenApi();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi();
}

app.Run();

public partial class Program;