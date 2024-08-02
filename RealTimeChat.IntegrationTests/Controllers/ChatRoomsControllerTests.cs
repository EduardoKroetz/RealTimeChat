using Newtonsoft.Json;
using RealTimeChat.Application.Commands.UpdateChatRoom;
using RealTimeChat.Core.DTOs;
using RealTimeChat.IntegrationTests.Utils;
using System.Net.Http.Json;
using RealTimeChat.Application.Commands.CreateUser;

namespace RealTimeChat.IntegrationTests.Controllers;

public class ChatRoomsControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Startup> _factory;
    private const string _baseUrl = "api/chatrooms";
    private readonly Guid _userId;

    public ChatRoomsControllerTests(CustomWebApplicationFactory<Startup> factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
        var userCommand = new CreateUserCommand { Email = "teste@gmail.com", Password = "123", Username = "teste" };
        _userId = LoginUser.Login(_client, userCommand).Result;
    }

    [Fact]
    public async Task FullChatRoomFlow_ShouldReturnSuccess()
    {
        // Arrange
        var chatRoomName = "Test Chat Room";
        var createResponse = await _client.PostAsJsonAsync($"{_baseUrl}?name={chatRoomName}", new { });
        createResponse.EnsureSuccessStatusCode();
        var chatRoomId = JsonConvert.DeserializeObject<Result<DataId>>(await createResponse.Content.ReadAsStringAsync())!.data.id;

        // Update chat room name
        var updateResponse = await _client.PutAsJsonAsync($"{_baseUrl}/{chatRoomId}?name=updatedChatRoom", new { });
        updateResponse.EnsureSuccessStatusCode();
        var updateContent = JsonConvert.DeserializeObject<Result<DataId>>(await updateResponse.Content.ReadAsStringAsync())!;
        Assert.Equal(chatRoomId, updateContent.data.id);

        // Join the chat room with the created user
        await LoginUser.Login(_client, new CreateUserCommand { Email = "teste1@gmail.com", Password = "1213", Username = "teste1" });
        var joinResponse = await _client.PostAsJsonAsync($"{_baseUrl}/join/{chatRoomId}", new { });
        joinResponse.EnsureSuccessStatusCode();
        var joinContent = JsonConvert.DeserializeObject<Result<object>>(await joinResponse.Content.ReadAsStringAsync())!;
        Assert.Equal("User has successfully entered the chat room", joinContent.message);

        // Leave the chat room
        var leaveResponse = await _client.DeleteAsync($"{_baseUrl}/leave/{chatRoomId}");
        leaveResponse.EnsureSuccessStatusCode();
        var leaveContent = JsonConvert.DeserializeObject<Result<object>>(await leaveResponse.Content.ReadAsStringAsync())!;
        Assert.Equal("User leave the chat room successfully", leaveContent.message);

        // Delete the chat room
        var deleteResponse = await _client.DeleteAsync($"{_baseUrl}/{chatRoomId}");
        deleteResponse.EnsureSuccessStatusCode();

    }


}
