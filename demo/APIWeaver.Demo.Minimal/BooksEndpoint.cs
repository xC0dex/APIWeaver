using APIWeaver.Demo.Shared;
using Microsoft.AspNetCore.Mvc;

namespace APIWeaver.Demo.Minimal;

internal static class BookEndpoints
{
    internal static void MapBookEndpoints(this IEndpointRouteBuilder builder)
    {
        var books = builder.MapGroup("/books")
            .WithTags("bookstore");

        books
            .MapGet("/", ([FromServices] BookStore bookStore) => bookStore.GetAll())
            .Produces<Book[]>()
            .ResponseDescription("An array of books")
            .WithDescription("Get all books");

        books
            .MapGet("/{bookId:guid}", ([FromServices] BookStore bookStore, Guid bookId) =>
            {
                var book = bookStore.GetById(bookId);
                return book is null ? Results.NotFound() : Results.Ok(book);
            })
            .Produces<Book>()
            .Produces(StatusCodes.Status404NotFound)
            .ResponseDescription("A book")
            .ResponseDescription("Book not found", StatusCodes.Status404NotFound)
            .WithDescription("Get a book by its ID");

        books
            .MapPost("/", ([FromServices] BookStore bookStore, Book myCustomBook) =>
            {
                var createdBook = bookStore.Add(myCustomBook);
                return createdBook is null ? Results.Conflict() : Results.Created($"/books/{createdBook.BookId}", createdBook);
            })
            .Produces<Book>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict)
            .WithDescription("Create a new book");

        books
            .MapPut("/{bookId:guid}", ([FromServices] BookStore bookStore, Guid bookId, Book book) =>
            {
                var updatedBook = bookStore.UpdateById(bookId, book);
                return updatedBook is null ? Results.NotFound() : Results.Ok(updatedBook);
            })
            .Produces<Book>()
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Update a book by its ID");

        books.MapDelete("/{bookId:guid}", ([FromServices] BookStore bookStore, Guid bookId) =>
            {
                var deleted = bookStore.DeleteById(bookId);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithDescription("Delete a book by its ID");
    }
}