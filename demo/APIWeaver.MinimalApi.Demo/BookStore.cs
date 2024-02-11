using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Bogus;

namespace APIWeaver.MinimalApi.Demo;

public sealed class BookStore
{
    private readonly List<Book> _books;

    public BookStore()
    {
        var faker = new Faker<Book>()
            .UseSeed(420)
            .RuleFor(b => b.BookId, f => f.Random.Guid())
            .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
            .RuleFor(b => b.Description, f => f.Lorem.Paragraph(1))
            .RuleFor(b => b.BookType, f => f.Random.Enum<BookType>())
            .RuleFor(b => b.Pages, f => f.Random.Int(69, 420));
        _books = faker.Generate(10);
    }

    public IEnumerable<Book> GetBooks() => _books;

    public Book? UpdateBook(Guid bookId, Book book)
    {
        var index = _books.FindIndex(x => x.BookId == bookId);
        if (index != -1)
        {
            _books[index] = book;
            return book;
        }

        return null;
    }

    public Book? AddBook(Book book)
    {
        if (_books.Find(x => x.BookId == book.BookId) is not null)
        {
            return null;
        }

        _books.Add(book);
        return book;
    }
}

public class Book
{
    public required Guid BookId { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    [Range(69, 420)]
    public required int Pages { get; set; }

    public BookType? BookType { get; set; }

    // [Obsolete]
    public Author? Author { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BookType
{
    Newsletter,
    Documentation
}

public class Author
{
    public required string Name { get; set; }
}

public class DummyResponse
{
    [Required]
    public Author Author { get; set; } = null!;
}