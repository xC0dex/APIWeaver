using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APIWeaver;
using APIWeaver.MinimalApi.Demo;
using Microsoft.AspNetCore.Mvc;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<BookStore>();
builder.Services.AddApiWeaver(options =>
{
    options
        .WithSchemaGeneratorOptions(schemaGeneratorOptions =>
        {
            schemaGeneratorOptions
                .WithJsonOptionsSource(JsonOptionsSource.MinimalApiOptions)
                .WithNullableAnnotationForReferenceTypes(true);
        });
});
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.IncludeFields = false;
    options.SerializerOptions.IgnoreReadOnlyFields = false;
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerUi();
}

var bookstoreEndpoint = app.MapGroup("/book-store")
    .WithTags("bookstore")
    .WithOpenApi();

bookstoreEndpoint.MapPost("/user", ([FromBody] User user) => Results.Ok(user)).Produces<User>();
// bookstoreEndpoint.MapGet("/parameters", ([FromQuery] [Range(69, 420, MinimumIsExclusive = true)] int age) => { Results.Ok(); });
//
// bookstoreEndpoint.MapGet("/", (BookStore bookstore) =>
// {
//     var books = bookstore.GetBooks();
//     return Results.Ok(books);
// }).Produces<IEnumerable<Book>>();
//
// bookstoreEndpoint.MapPost("/", (Book book, BookStore bookstore) =>
//     {
//         var addedBook = bookstore.AddBook(book);
//         return addedBook is null ? Results.Conflict() : Results.Ok(addedBook);
//     }).Produces<Book>()
//     .Produces(409);
//
// bookstoreEndpoint.MapPut("/{id:guid?}", (Guid? id, Book book, BookStore bookstore) =>
//     {
//         var updateBook = bookstore.UpdateBook(id!.Value, book);
//         return updateBook is null ? Results.NotFound() : Results.Ok(updateBook);
//     }).Produces<Book>()
//     .Produces(404);
// bookstoreEndpoint.MapPost("/dummy", ([FromBody] [MinLength(4)] User[] friends) => Results.Ok()).Produces<DummyResponse>();
// app.MapPost("/user", (User value, BookStore bookstore) => Results.Ok()).Produces<User>().WithOpenApi();


app.Run();


public class User
{
    [AllowedValues("Peter", "Max")]
    public required string Name { get; set; }

    [AllowedValues(2, 3)]
    public required int Age { get; set; }

    // [MinLength(4)]
    // public required string[] Friends { get; set; }
    [JsonPropertyName("fullName")]
    [JsonInclude]
    public string _fullName = "josh";


    // public required Dictionary<string, Book> Books { get; set; }
}