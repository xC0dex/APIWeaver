using APIWeaver.Demo.Shared;
using Asp.Versioning;
using Scalar.AspNetCore;
using static APIWeaver.BuildHelper;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddDemoServices();

var app = builder.Build();

// If the current invocation is for document generation, run the application and exit.
if (IsGenerationContext)
{
    app.Run();
}

app.MapOpenApi();
app.MapScalarApiReference();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();