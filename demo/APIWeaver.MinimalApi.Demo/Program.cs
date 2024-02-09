using APIWeaver.MinimalApi.Demo;
using APIWeaver.OpenApi.Extensions;
using APIWeaver.Schema.Models;
using APIWeaver.Swagger.Extensions;
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
    options.SerializerOptions.IncludeFields = true;
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

bookstoreEndpoint.MapGet("/", (BookStore bookstore) =>
{
    var books = bookstore.GetBooks();
    return Results.Ok(books);
}).Produces<IEnumerable<Book>>();

bookstoreEndpoint.MapPost("/", (Book book, BookStore bookstore) =>
    {
        var addedBook = bookstore.AddBook(book);
        return addedBook is null ? Results.Conflict() : Results.Ok(addedBook);
    }).Produces<Book>()
    .Produces(409);

bookstoreEndpoint.MapPut("/{id:guid?}", (Guid? id, Book book, BookStore bookstore) =>
    {
        var updateBook = bookstore.UpdateBook(id!.Value, book);
        return updateBook is null ? Results.NotFound() : Results.Ok(updateBook);
    }).Produces<Book>()
    .Produces(404);

bookstoreEndpoint.MapPost("/dummy", (DummyResponse value, BookStore bookstore) => Results.Ok((object?) value)).Produces<DummyResponse>();
app.MapPost("/user", (User value, BookStore bookstore) => Results.Ok()).Produces<User>().WithOpenApi();


app.Run();


public class User
{
    public required string? Name { get; set; }

    // [Obsolete]
    public required User[] Friends { get; set; }

    public required Dictionary<string, Book> Books { get; set; }
}