using APIWeaver.Demo.Shared;

namespace APIWeaver.Demo.Minimal;

internal sealed class BookExampleProvider: IExampleProvider<Book>
{
    public static Book GetExample() => new()
    {
        BookId = Guid.NewGuid(),
        Title = "Hola Fresh",
        Description = "A book about nothing",
        BookType = BookType.Newsletter,
        Pages = 188
    };
}