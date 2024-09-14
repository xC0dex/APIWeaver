namespace APIWeaver.Demo.Client;

internal class UserDto
{
    public required Guid UserId { get; set; }

    public string? Name { get; set; }

    public int Age { get; set; }
}
