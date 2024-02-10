using APIWeaver.OpenApi.Extensions;
using APIWeaver.OpenApi.MinimalApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiWeaver();

var app = builder.Build();

app.UseOpenApi();

var userGroup = app.MapGroup("/user").WithOpenApi();

userGroup.MapGet("/", () =>
{
    var result = Enumerable.Range(0, 9).Select(x => new User
    {
        Id = Guid.NewGuid(),
        Name = "John Doe",
        Age = 69
    });
    return Results.Ok(result);
}).Produces<IEnumerable<User>>();

userGroup.MapGet("/{id:guid}", (Guid id) =>
{
    var user = new User
    {
        Id = id,
        Name = "John Doe",
        Age = 69
    };
    return Results.Ok(user);
}).Produces<User>();

userGroup.MapPost("/", (User user) => Results.Ok(user)).Produces<User>();

app.Run();


public partial class Program;