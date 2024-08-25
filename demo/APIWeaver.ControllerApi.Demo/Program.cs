using APIWeaver;
using APIWeaver.Transformers;
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

builder.Services.AddAuthentication()
    .AddJwtBearer();
builder.Services.AddAuthorizationBuilder()
    .AddFallbackPolicy("foo", policy => policy.RequireRole("foo"));

builder.Services.AddOpenApiDocument("v1", options: options =>
{
    options.AddOperationTransformer<MethodInfoOperationTransformer>();
    options.AddOperationTransformer<AuthorizeOperationTransformer>();
});

var app = builder.Build();
app.MapOpenApi().AllowAnonymous();
app.MapSwaggerUi().AllowAnonymous();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();