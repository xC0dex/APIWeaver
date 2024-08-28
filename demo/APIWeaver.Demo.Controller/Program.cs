using APIWeaver;
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

builder.Services.AddAuthentication().AddJwtBearer();


builder.Services.AddAuthorizationBuilder()
    .AddFallbackPolicy("foo", policy => policy.RequireRole("foo"));

builder.Services.AddOpenApiDocument("v1", options =>
{
    options
        .AddAuthResponse()
        .AddSecurityScheme("Bearer", scheme =>
        {
            scheme.In = ParameterLocation.Header;
            scheme.Type = SecuritySchemeType.OAuth2;
            scheme.Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri("https://example.com/oauth2/authorize"),
                    TokenUrl = new Uri("https://example.com/oauth2/token"),
                }
            };
        })
        .AddOperationTransformer<MethodInfoOperationTransformer>();
});

var app = builder.Build();
app.MapOpenApi().AllowAnonymous();
app.MapSwaggerUi(options =>
{
    options
        .WithTryItOut(false)
        .WithDarkMode(false)
        .WithDeepLinking(false);
    options.OAuth2Options = new OAuth2Options
    {
        ClientId = "client_id",
        ClientSecret = "client_secret"
    };
}).AllowAnonymous();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();