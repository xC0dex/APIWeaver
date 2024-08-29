using APIWeaver;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAuthentication().AddJwtBearer();


builder.Services.AddOpenApi(options =>
{
    options.AddSecurityScheme("Bearer", scheme =>
    {
        scheme.In = ParameterLocation.Header;
        scheme.Type = SecuritySchemeType.OAuth2;
        scheme.Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://example.com/oauth2/authorize"),
                TokenUrl = new Uri("https://example.com/oauth2/token")
            }
        };
    });
    options.AddAuthResponse();
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
}

app.MapControllers();

app.Run();

public partial class Program;