using APIWeaver.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiWeaver();

var app = builder.Build();

app.MapGet("/hello-world/{id:guid}", (Guid id) => id)
    .Produces(200);


app.Run();