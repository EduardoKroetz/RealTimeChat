using Newtonsoft.Json;
using RealTimeChat.Application.Commands.CreateChatRoom;
using RealTimeChat.Application.Commands.CreateUser;
using RealTimeChat.Application.Commands.UpdateChatRoom;
using RealTimeChat.IntegrationTests.Utils;
using System.Net;
using System.Net.Http.Json;

namespace RealTimeChat.IntegrationTests.Controllers;

public class ChatRoomControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Startup> _factory;
    private const string _baseUrl = "api/chatroom";

    public ChatRoomControllerTests(CustomWebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    [Fact]
    public async Task AddAsync_ShouldReturnOk()
    {
        // Arrange
        var userCommand = new CreateUserCommand { Email = "teste@gmail.com", Password = "123", Username = "teste" };
        var userResponse = await _client.PostAsJsonAsync("api/auth/register", userCommand);
        var userId = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse.Content.ReadAsStringAsync())!.data.id;

        var command = new CreateChatRoomCommand { Name = "General", UserId = userId };

        // Act
        var response = await _client.PostAsJsonAsync(_baseUrl, command);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Result<DataId>>(await response.Content.ReadAsStringAsync())!;

        Assert.NotEqual(Guid.Empty, content.data.id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnOk()
    {
        // Arrange
        var userCommand = new CreateUserCommand { Email = "teste3@gmail.com", Password = "123", Username = "teste" };
        var userResponse = await _client.PostAsJsonAsync("api/auth/register", userCommand);
        var userId = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse.Content.ReadAsStringAsync())!.data.id;

        var createCommand = new CreateChatRoomCommand { Name = "General", UserId = userId };
        var createResponse = await _client.PostAsJsonAsync(_baseUrl, createCommand);
        createResponse.EnsureSuccessStatusCode();
        var createContent = JsonConvert.DeserializeObject<Result<DataId>>(await createResponse.Content.ReadAsStringAsync())!;
        var chatRoomId = createContent.data.id;

        // Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{chatRoomId}");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingChatRoom_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistingChatRoomId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{nonExistingChatRoomId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Result<object>>(await response.Content.ReadAsStringAsync())!;
        Assert.Equal("Chat room not found", content.message);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnOk()
    {
        // Arrange
        var userCommand = new CreateUserCommand { Email = "teste1000@gmail.com", Password = "123", Username = "teste" };
        var userResponse = await _client.PostAsJsonAsync("api/auth/register", userCommand);
        var userId = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse.Content.ReadAsStringAsync())!.data.id;
        var createResult = await _client.PostAsJsonAsync(_baseUrl, new CreateChatRoomCommand { Name = "Bate papo", UserId = userId });

        var chatRoomId = JsonConvert.DeserializeObject<Result<DataId>>(await createResult.Content.ReadAsStringAsync())!.data.id;

        var command = new UpdateChatRoomCommand { Id = chatRoomId, Name = "Bate papo aaa" };
        // Act
        var response = await _client.PutAsJsonAsync(_baseUrl, command);

        // Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Result<DataId>>(await response.Content.ReadAsStringAsync())!;

        Assert.NotEqual(Guid.Empty, content.data.id);
    }
}
