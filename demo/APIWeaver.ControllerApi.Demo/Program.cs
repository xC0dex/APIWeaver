using APIWeaver;
using APIWeaver.Transformers;
using Asp.Versioning;
using Microsoft.OpenApi.Models;

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

builder.Services.AddOpenApiDocument("v1", options =>
{
    options.AddSecurityScheme("Bearer", scheme =>
    {
        scheme.In = ParameterLocation.Header;
        scheme.Type = SecuritySchemeType.OAuth2;
        scheme.Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://example.com/oauth2/authorize")
            }
        };
    });
    
    options.AddOperationTransformer<MethodInfoOperationTransformer>();
    options.AddOperationTransformer<AuthorizeResponseOperationTransformer>();
});

var app = builder.Build();
app.MapOpenApi().AllowAnonymous();
app.MapSwaggerUi(x =>
{
    x.AdditionalUiOptions.Stylesheets.Add("test");
    x.AdditionalUiOptions.Scripts.Add("test");
}).AllowAnonymous();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();