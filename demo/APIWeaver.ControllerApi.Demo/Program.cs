using APIWeaver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddOpenApiDocument();

var app = builder.Build();
app.MapOpenApi();
app.MapSwaggerUi();


app.MapControllers();
app.Run();