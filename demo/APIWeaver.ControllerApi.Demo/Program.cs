using APIWeaver;
using Asp.Versioning;

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


builder.Services.AddOpenApiDocuments(["v1", "v2"]);

var app = builder.Build();
app.MapOpenApi();
app.MapSwaggerUi();


app.MapControllers();
app.Run();