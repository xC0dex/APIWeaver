using APIWeaver.Extensions;
using APIWeaver.Swagger.Extensions;
using APIWeaver.Swagger.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiWeaver();

if (builder.Environment.IsDevelopment())
{
    builder.Services.Configure<SwaggerUiConfiguration>(x => x.Title = "Hola fresh");
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi();
}

app.MapGet("/hello-world/{id:guid}", (Guid id) => id)
    .Produces(200);


app.Run();