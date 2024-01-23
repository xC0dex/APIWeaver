using APIWeaver.Swagger.Extensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi();
}

app.Run();

public partial class Program;