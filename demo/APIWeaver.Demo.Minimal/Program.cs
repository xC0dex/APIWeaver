using APIWeaver.Demo.Minimal;
using APIWeaver.Demo.Shared;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDemoServices();

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.MapOpenApi();
app.MapScalarApiReference(o =>
{
    o.AddServer("https://example.com");
    o.WithTheme(ScalarTheme.Mars);
});

app.MapBookEndpoints();

app.Run();