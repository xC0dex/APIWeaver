using APIWeaver.Demo.Shared;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace APIWeaver.ControllerApi.Demo.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("v{version:apiVersion}/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class BookController(BookStore bookStore) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<IEnumerable<Book>>(StatusCodes.Status200OK, "application/json")]
    [ResponseDescription("An array of books")]
    public IActionResult GetBooks()
    {
        return Ok(bookStore.GetAll());
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<Book>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ResponseDescription("A book")]
    [ResponseDescription("Book not found", StatusCodes.Status404NotFound)]
    public IActionResult GetBook(Guid id)
    {
        var book = bookStore.GetById(id);
        if (book is null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPost]
    [ProducesResponseType<Book>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public IActionResult CreateBookAsync(Book myDummyBook)
    {
        var createdBook = bookStore.Add(myDummyBook);
        if (createdBook is null)
        {
            return Conflict("A book with the same ID already exists.");
        }

        return CreatedAtAction(nameof(GetBook), new { id = createdBook.BookId }, createdBook);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType<Book>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateBook(Guid id, Book book)
    {
        var updatedBook = bookStore.UpdateById(id, book);
        if (updatedBook is null)
        {
            return NotFound();
        }

        return Ok(updatedBook);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteBook(Guid id)
    {
        var deleted = bookStore.DeleteById(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}