using APIWeaver;
using APIWeaver.Demo.Shared;
using Asp.Versioning;
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

builder.Services.AddSingleton<BookStore>();


builder.Services.AddOpenApiDocument("v1", options =>
{
    // options
    //     .AddAuthResponse()
    //     .AddSecurityScheme("Bearer", scheme =>
    //     {
    //         scheme.In = ParameterLocation.Header;
    //         scheme.Type = SecuritySchemeType.OAuth2;
    //         scheme.Flows = new OpenApiOAuthFlows
    //         {
    //             AuthorizationCode = new OpenApiOAuthFlow
    //             {
    //                 AuthorizationUrl = new Uri("https://example.com/oauth2/authorize"),
    //                 TokenUrl = new Uri("https://example.com/oauth2/token")
    //             }
    //         };
    //     })
        options.AddOperationTransformer<MethodInfoOperationTransformer>();
});

var app = builder.Build();

// If the current invocation is for document generation, run the application and exit.
if (IsGenerationContext)
{
    app.Run();
}

app.MapOpenApi().AllowAnonymous();
app.MapSwaggerUi(options =>
{
    options
        .WithTryItOut(false)
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