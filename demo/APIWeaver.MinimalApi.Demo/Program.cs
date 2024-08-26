using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APIWeaver;
using APIWeaver.MinimalApi.Demo;
using Microsoft.AspNetCore.Mvc;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<BookStore>();
builder.Services.Configure<JsonOptions>(options => { options.SerializerOptions.IgnoreReadOnlyProperties = true; });

builder.Services.AddOpenApiDocument(x => { x.AddDocumentTransformer((document, _) => document.Info.Title = "Book Store API"); });

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.MapSwaggerUi(x => x.OAuth2Options = new OAuth2Options
    {
        Realm = "dawd",
        ClientId = "afwf",
        ClientSecret = "awgw"
    });
}

var bookstoreEndpoint = app.MapGroup("/book-store")
    .RequireAuthorization()
    .WithTags("bookstore");

bookstoreEndpoint.MapGet("/user", (Guid id, [FromHeader] [Required] int age, [FromQuery] string? name, [FromBody] string lul) => Results.NotFound());


app.Run();


public class User
{
    private int _number;
    public required string Id { get; set; }

    [JsonPropertyName("theAge")]
    public int Age { get; }

    private string? Name { get; init; }

    [JsonInclude]
    private string FullName { get; init; } = "awd";

    [JsonPropertyOrder(1)]
    public int Number
    {
        set => _number = value;
    }
}

// public class User
// {
//     [AllowedValues("Peter", "Max")]
//     public required string Name { get; set; }
//
//     [AllowedValues(2, 3)]
//     public required int Age { get; set; }
//
//     // [MinLength(4)]
//     // public required string[] Friends { get; set; }
//     [JsonPropertyName("fullName")]
//     [JsonInclude]
//     public string _fullName = "josh";
//
//
//     // public required Dictionary<string, Book> Books { get; set; }
// }