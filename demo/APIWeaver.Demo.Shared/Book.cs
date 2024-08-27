namespace APIWeaver.Demo.Shared;

public class Book
{
    public required Guid BookId { get; set; }

    public required string Title { get; set; }

    public string? Description { get; set; }

    public required int Pages { get; set; }

    public BookType? BookType { get; set; }
}