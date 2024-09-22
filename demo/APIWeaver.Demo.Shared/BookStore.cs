using Bogus;

namespace APIWeaver.Demo.Shared;

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

    public IEnumerable<Book> GetAll() => _books;

    public Book? GetById(Guid bookId) => _books.FirstOrDefault(x => x.BookId == bookId);

    public Book? Add(Book book)
    {
        if (_books.Any(x => x.BookId == book.BookId))
        {
            return null;
        }

        _books.Add(book);
        return book;
    }

    public Book? UpdateById(Guid bookId, Book book)
    {
        var index = _books.FindIndex(x => x.BookId == bookId);
        if (index != -1)
        {
            _books[index] = book;
            return book;
        }

        return null;
    }

    public bool DeleteById(Guid bookId)
    {
        var book = _books.FirstOrDefault(x => x.BookId == bookId);
        if (book is not null)
        {
            _books.Remove(book);
            return true;
        }

        return false;
    }
}