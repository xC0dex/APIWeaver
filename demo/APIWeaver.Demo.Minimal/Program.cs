using APIWeaver;
using APIWeaver.Demo.Shared;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<BookStore>();

builder.Services.AddOpenApiDocument(options => options.AddDocumentTransformer((document, _) => document.Info.Title = "Book Store API"));

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.MapSwaggerUi(x => x.OAuth2Options = new OAuth2Options
    {
        Realm = "realm",
        ClientId = "clientId",
        ClientSecret = "clientSecret"
    });
}

var bookstoreGroup = app.MapGroup("/books")
    .WithTags("bookstore");

bookstoreGroup
    .MapGet("/", ([FromServices] BookStore bookStore) => bookStore.GetBooks())
    .Produces<Book[]>();

bookstoreGroup
    .MapPost("/", ([FromServices] BookStore bookStore, Book book) => bookStore.AddBook(book))
    .Produces<Book>();

bookstoreGroup
    .MapPut("/{bookId:guid}", ([FromServices] BookStore bookStore, Guid bookId, Book book) => bookStore.UpdateBook(bookId, book))
    .Produces<Book>();

app.Run();