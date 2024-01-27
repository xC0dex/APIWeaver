using APIWeaver.Extensions;
using APIWeaver.Swagger.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiWeaver(x =>
{
    x.OpenApiDocuments.Add("One v1", new OpenApiInfo
    {
        Title = "One",
        Version = "1.2.3"
    });
    x.OpenApiDocuments.Add("Two v2", new OpenApiInfo
    {
        Title = "Two",
        Version = "1.2.3"
    });
});

builder.Services.AddApiWeaver();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUi(x =>
    {
        x.WithOpenApiDocument("One v1");
        x.WithOpenApiDocument("Two v2");
    });
}

app.MapGet("/hello-world/{id:guid}", (Guid id) => id)
    .Produces<Guid>()
    .WithDescription("My minimal api endpoint")
    .WithOpenApi();


app.Run();