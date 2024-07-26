using Newtonsoft.Json;
using RealTimeChat.Application.Commands.CreateUser;
using RealTimeChat.Application.ViewModels;
using RealTimeChat.IntegrationTests.Utils;
using System.Net;
using System.Net.Http.Json;

namespace RealTimeChat.IntegrationTests.Controllers;

public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Startup> _factory;
    private const string _baseUrl = "api/users";

    public UsersControllerTests(CustomWebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task GetAsync_ShouldReturnPagedResult()
    {
        // Arrange
        var userCommand1 = new CreateUserCommand { Email = "user1@gmail.com", Password = "123", Username = "user1" };
        var userCommand2 = new CreateUserCommand { Email = "user2@gmail.com", Password = "123", Username = "user2" };
        await _client.PostAsJsonAsync("api/auth/register", userCommand1);
        await _client.PostAsJsonAsync("api/auth/register", userCommand2);

        var pageSize = 1;
        var pageNumber = 1;

        // Act
        var response = await _client.GetAsync($"{_baseUrl}?pageSize={pageSize}&pageNumber={pageNumber}");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<PagedResult<List<GetUsersViewModel>>>(await response.Content.ReadAsStringAsync())!;

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(content.data);
        Assert.Equal(pageSize, content.data.Count);
        Assert.Equal(pageNumber, content.pageNumber);
        Assert.Equal(pageSize, content.pageSize);
    }
}