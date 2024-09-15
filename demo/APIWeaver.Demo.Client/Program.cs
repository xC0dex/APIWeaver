using APIWeaver.Demo.Client;

Console.Write("");


using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("https://localhost:7277");

var client = new UserClientExample(httpClient);

Response<UserDto, UserDto> response = await client.GetUserAotAsync(Guid.NewGuid(), 69, "Max", UserDtoSerializerContext.Default.UserDto, UserDtoSerializerContext.Default.UserDto);
if (response.Ok is not null)
{
    Console.WriteLine($"User: {response.Ok.Name} ({response.Ok.Age})");
}

response = await client.GetUserAsync<UserDto, UserDto>(Guid.NewGuid(), 69, "Max");
if (response.Ok is not null)
{
    Console.WriteLine($"User: {response.Ok.Name} ({response.Ok.Age})");
}
