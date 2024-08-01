using Newtonsoft.Json;
using RealTimeChat.Application.Commands.CreateChatRoom;
using RealTimeChat.Application.Commands.CreateUser;
using RealTimeChat.Application.Commands.JoinChatRoom;
using RealTimeChat.Application.Commands.UpdateChatRoom;
using RealTimeChat.Core.DTOs;
using RealTimeChat.IntegrationTests.Utils;
using System.Net;
using System.Net.Http.Json;

namespace RealTimeChat.IntegrationTests.Controllers;

public class ChatRoomsControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Startup> _factory;
    private const string _baseUrl = "api/chatrooms";

    public ChatRoomsControllerTests(CustomWebApplicationFactory<Startup> factory)
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

    [Fact]
    public async Task GetAsync_ShouldReturnOk()
    {
        // Arrange
        var userCommand = new CreateUserCommand { Email = "teste000@gmail.com", Password = "123", Username = "teste" };
        var userResponse = await _client.PostAsJsonAsync("api/auth/register", userCommand);
        var userId = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse.Content.ReadAsStringAsync())!.data.id;
        var createResult = await _client.PostAsJsonAsync(_baseUrl, new CreateChatRoomCommand { Name = "Bate papo", UserId = userId });

        // Act
        var response = await _client.GetAsync($"{_baseUrl}?pageSize=10&pageNumber=1");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<PagedResult<ICollection<GetChatRoomsViewModel>>>(await response.Content.ReadAsStringAsync())!;

        Assert.True(content.data.Count > 0);
        Assert.NotNull(content.data);
    }

    [Fact]
    public async Task JoinChatRoom_ShouldReturnOk()
    {
        // Arrange
        var userCommand = new CreateUserCommand { Email = "test131@gmail.com", Password = "123", Username = "test1" };
        var userResponse = await _client.PostAsJsonAsync("api/auth/register", userCommand);
        var userId = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse.Content.ReadAsStringAsync())!.data.id;

        var chatRoomCommand = new CreateChatRoomCommand { Name = "General", UserId = userId };
        var chatRoomResponse = await _client.PostAsJsonAsync(_baseUrl, chatRoomCommand);
        var chatRoomId = JsonConvert.DeserializeObject<Result<DataId>>(await chatRoomResponse.Content.ReadAsStringAsync())!.data.id;

        var userCommand2 = new CreateUserCommand { Email = "test191@gmail.com", Password = "123", Username = "test191" };
        var userResponse2 = await _client.PostAsJsonAsync("api/auth/register", userCommand2);
        var userId2 = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse2.Content.ReadAsStringAsync())!.data.id;

        // Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{chatRoomId}/join/{userId2}", new { });

        // Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Result<object>>(await response.Content.ReadAsStringAsync())!;

        Assert.Equal($"User has successfully entered the chat room", content.message);
    }

    [Fact]
    public async Task JoinChatRoom_WithNonExistingUser_ShouldReturnNotFound()
    {
        // Arrange
        var userCommand = new CreateUserCommand { Email = "test1@gmail.com", Password = "123", Username = "test1" };
        var userResponse = await _client.PostAsJsonAsync("api/auth/register", userCommand);
        var userId = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse.Content.ReadAsStringAsync())!.data.id;

        var chatRoomCommand = new CreateChatRoomCommand { Name = "General", UserId = userId };
        var chatRoomResponse = await _client.PostAsJsonAsync(_baseUrl, chatRoomCommand);
        var chatRoomId = JsonConvert.DeserializeObject<Result<DataId>>(await chatRoomResponse.Content.ReadAsStringAsync())!.data.id;

        // Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{chatRoomId}/join/{Guid.NewGuid()}", new { });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Result<object>>(await response.Content.ReadAsStringAsync())!;
        Assert.Equal("User not found", content.message);
    }

    [Fact]
    public async Task JoinChatRoom_WithNonExistingChatRoom_ShouldReturnNotFound()
    {
        // Arrange
        var userCommand = new CreateUserCommand { Email = "test2@gmail.com", Password = "123", Username = "test2" };
        var userResponse = await _client.PostAsJsonAsync("api/auth/register", userCommand);
        var userId = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse.Content.ReadAsStringAsync())!.data.id;

        // Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{Guid.NewGuid()}/join/{userId}", new { });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Result<object>>(await response.Content.ReadAsStringAsync())!;
        Assert.Equal("Chat room not found", content.message);
    }

    [Fact]
    public async Task JoinChatRoom_WhenAlreadyInChatRoom_ShouldReturnBadRequest()
    {
        // Arrange
        var userCommand = new CreateUserCommand { Email = "test3@gmail.com", Password = "123", Username = "test3" };
        var userResponse = await _client.PostAsJsonAsync("api/auth/register", userCommand);
        var userId = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse.Content.ReadAsStringAsync())!.data.id;

        var chatRoomCommand = new CreateChatRoomCommand { Name = "General", UserId = userId };
        var chatRoomResponse = await _client.PostAsJsonAsync(_baseUrl, chatRoomCommand);
        var chatRoomId = JsonConvert.DeserializeObject<Result<DataId>>(await chatRoomResponse.Content.ReadAsStringAsync())!.data.id;

        await _client.PostAsJsonAsync($"{_baseUrl}/{chatRoomId}/join/{userId}", new { }); // First join

        // Act
        var response = await _client.PostAsJsonAsync($"{_baseUrl}/{chatRoomId}/join/{userId}", new {}); // Second join

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Result<object>>(await response.Content.ReadAsStringAsync())!;
        Assert.Equal("This user is already in this chat room", content.message);
    }

    [Fact]
    public async Task LeaveChatRoom_ShouldReturnOk()
    {
        // Arrange
        var userCommand = new CreateUserCommand { Email = "test11@gmail.com", Password = "123", Username = "test1" };
        var userResponse = await _client.PostAsJsonAsync("api/auth/register", userCommand);
        var userId = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse.Content.ReadAsStringAsync())!.data.id;

        var chatRoomCommand = new CreateChatRoomCommand { Name = "General", UserId = userId };
        var chatRoomResponse = await _client.PostAsJsonAsync(_baseUrl, chatRoomCommand);
        var chatRoomId = JsonConvert.DeserializeObject<Result<DataId>>(await chatRoomResponse.Content.ReadAsStringAsync())!.data.id;

        await _client.PostAsJsonAsync($"{_baseUrl}/{chatRoomId}/join/{userId}", new { });

        // Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{chatRoomId}/leave/{userId}");

        // Assert
        response.EnsureSuccessStatusCode();

        var content = JsonConvert.DeserializeObject<Result<object>>(await response.Content.ReadAsStringAsync())!;

        Assert.Equal("User leave the chat room successfully", content.message);
    }

    [Fact]
    public async Task LeaveChatRoom_WithNonExistingUser_ShouldReturnNotFound()
    {
        // Arrange
        var userCommand = new CreateUserCommand { Email = "test111@gmail.com", Password = "123", Username = "test1" };
        var userResponse = await _client.PostAsJsonAsync("api/auth/register", userCommand);
        var userId = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse.Content.ReadAsStringAsync())!.data.id;

        var chatRoomCommand = new CreateChatRoomCommand { Name = "General", UserId = userId };
        var chatRoomResponse = await _client.PostAsJsonAsync(_baseUrl, chatRoomCommand);
        var chatRoomId = JsonConvert.DeserializeObject<Result<DataId>>(await chatRoomResponse.Content.ReadAsStringAsync())!.data.id;

        // Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{chatRoomId}/leave/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Result<object>>(await response.Content.ReadAsStringAsync())!;
        Assert.Equal("Room participant not found", content.message);
    }

    [Fact]
    public async Task LeaveChatRoom_WithNonExistingChatRoom_ShouldReturnNotFound()
    {
        // Arrange
        var userCommand = new CreateUserCommand { Email = "test26@gmail.com", Password = "123", Username = "test2" };
        var userResponse = await _client.PostAsJsonAsync("api/auth/register", userCommand);
        var userId = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse.Content.ReadAsStringAsync())!.data.id;

        // Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{Guid.NewGuid()}/leave/{userId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Result<object>>(await response.Content.ReadAsStringAsync())!;
        Assert.Equal("Room participant not found", content.message);
    }

    [Fact]
    public async Task LeaveChatRoom_WhenNotInChatRoom_ShouldReturnNotFound()
    {
        // Arrange
        var userCommand = new CreateUserCommand { Email = "test39@gmail.com", Password = "123", Username = "test3" };
        var userResponse = await _client.PostAsJsonAsync("api/auth/register", userCommand);
        var userId = JsonConvert.DeserializeObject<Result<DataRegisterUser>>(await userResponse.Content.ReadAsStringAsync())!.data.id;

        var chatRoomCommand = new CreateChatRoomCommand { Name = "General", UserId = userId };
        var chatRoomResponse = await _client.PostAsJsonAsync(_baseUrl, chatRoomCommand);
        var chatRoomId = JsonConvert.DeserializeObject<Result<DataId>>(await chatRoomResponse.Content.ReadAsStringAsync())!.data.id;

        await _client.DeleteAsync($"{_baseUrl}/{chatRoomId}/leave/{userId}"); //leave
        // Act
        var response = await _client.DeleteAsync($"{_baseUrl}/{chatRoomId}/leave/{userId}"); //leave again

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var content = JsonConvert.DeserializeObject<Result<object>>(await response.Content.ReadAsStringAsync())!;
        Assert.Equal("Room participant not found", content.message);
    }
}
