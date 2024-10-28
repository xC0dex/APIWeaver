using APIWeaver;
using APIWeaver.Demo.Shared;
using Microsoft.AspNetCore.Mvc;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<BookStore>();

builder.Services.AddApiWeaver("v1", options =>
{
    options
        .AddExample(new Book
        {
            BookId = Guid.NewGuid(),
            Title = "Hola",
            Description = "A book about nothing",
            BookType = BookType.Newsletter,
            Pages = 187
        });
});
builder.Services.AddOpenApi(options =>
{
    options.AddOperationTransformer<AdditionalDescriptionTransformer>();
    options.AddDocumentTransformer((document, _) => document.Info.Title = "Book Store API");
});

var app = builder.Build();

app.UseDeveloperExceptionPage();
app.MapOpenApi();
app.MapScalarApiReference(o =>
{
    o.WithTheme(ScalarTheme.Mars);
});

var bookstoreGroup = app.MapGroup("/books")
    .WithTags("bookstore");

bookstoreGroup
    .MapGet("/", ([FromServices] BookStore bookStore) => bookStore.GetAll())
    .Produces<Book[]>();

bookstoreGroup
    .MapPost("/", ([FromServices] BookStore bookStore, Book myCustomBook) => bookStore.Add(myCustomBook))
    .Produces<Book>();

bookstoreGroup
    .MapPut("/{bookId:guid}", ([FromServices] BookStore bookStore, Guid bookId, Book book) => bookStore.UpdateById(bookId, book))
    .Produces<Book>();

app.Run();