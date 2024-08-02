using Moq;
using Moq.EntityFrameworkCore;
using RealTimeChat.Core.Entities;
using RealTimeChat.Infrastructure.Persistence.Context;
using RealTimeChat.Infrastructure.Persistence.Repositories;

namespace RealTimeChat.UnitTests.Repositories;

public class ChatRoomRepositoryTests
{
    private readonly Mock<RealTimeChatDbContext> _mockContext;
    private readonly ChatRoomRepository _chatRoomRepository;

    public ChatRoomRepositoryTests()
    {
        _mockContext = new Mock<RealTimeChatDbContext>();

        var chatRooms = new List<ChatRoom>
        {
            new ChatRoom
            {
                Id = Guid.NewGuid(),
                Name = "General",
                CreatedAt = DateTime.Now,
                Messages = new List<Message> { new Message { Id = Guid.NewGuid(), Content = "Hello", ChatRoomId = Guid.NewGuid(), SenderId = Guid.NewGuid(), Timestamp = DateTime.Now } },
                RoomParticipants = new List<RoomParticipant> { new RoomParticipant { Id = Guid.NewGuid(), ChatRoomId = Guid.NewGuid(), UserId = Guid.NewGuid() } }
            }
        }.AsQueryable();

        _mockContext.Setup(c => c.ChatRooms).ReturnsDbSet(chatRooms);
        _mockContext.Setup(c => c.Messages).ReturnsDbSet(new List<Message>().AsQueryable());
        _mockContext.Setup(c => c.RoomParticipants).ReturnsDbSet(new List<RoomParticipant>().AsQueryable());

        _chatRoomRepository = new ChatRoomRepository(_mockContext.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldAddChatRoomToDatabase()
    {
        // Arrange
        var chatRoom = new ChatRoom { Id = Guid.NewGuid(), Name = "New Chat Room", CreatedAt = DateTime.Now };

        var chatRooms = new List<ChatRoom>().AsQueryable();
        _mockContext.Setup(c => c.ChatRooms).ReturnsDbSet(chatRooms);

        // Act
        await _chatRoomRepository.AddAsync(chatRoom);

        // Assert
        _mockContext.Verify(c => c.ChatRooms.AddAsync(chatRoom, default), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveChatRoomFromDatabase()
    {
        // Arrange
        var chatRoom = new ChatRoom { Id = Guid.NewGuid(), Name = "Chat Room to Delete", CreatedAt = DateTime.Now };

        var chatRooms = new List<ChatRoom> { chatRoom }.AsQueryable();
        _mockContext.Setup(c => c.ChatRooms).ReturnsDbSet(chatRooms);

        // Act
        await _chatRoomRepository.DeleteAsync(chatRoom);

        // Assert
        _mockContext.Verify(c => c.ChatRooms.Remove(chatRoom), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnChatRoom()
    {
        // Arrange
        var chatRoomId = _mockContext.Object.ChatRooms.First().Id;

        // Act
        var result = await _chatRoomRepository.GetByIdAsync(chatRoomId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(chatRoomId, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateChatRoomInDatabase()
    {
        // Arrange
        var chatRoom = new ChatRoom { Id = Guid.NewGuid(), Name = "Old Chat Room", CreatedAt = DateTime.Now };

        var chatRooms = new List<ChatRoom> { chatRoom }.AsQueryable();
        _mockContext.Setup(c => c.ChatRooms).ReturnsDbSet(chatRooms);

        chatRoom.Name = "Updated Chat Room";

        // Act
        await _chatRoomRepository.UpdateAsync(chatRoom);

        // Assert
        _mockContext.Verify(c => c.ChatRooms.Update(chatRoom), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetMessagesInRoomAsync_ShouldReturnMessagesInRoom()
    {
        // Arrange
        var chatRoomId = _mockContext.Object.ChatRooms.First().Id;
        var messages = new List<Message>
        {
            new Message { Id = Guid.NewGuid(), Content = "First Message", ChatRoomId = chatRoomId, SenderId = Guid.NewGuid(), Timestamp = DateTime.Now },
            new Message { Id = Guid.NewGuid(), Content = "Second Message", ChatRoomId = chatRoomId, SenderId = Guid.NewGuid(), Timestamp = DateTime.Now.AddMinutes(1) }
        }.AsQueryable();

        _mockContext.Setup(c => c.Messages).ReturnsDbSet(messages);

        // Act
        var result = await _chatRoomRepository.GetMessagesInRoomAsync(0,10,chatRoomId);

        // Assert
        Assert.NotEmpty(result);
        Assert.All(result, m => Assert.Equal(chatRoomId, m.ChatRoomId));
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetUserChatRoomsAsync_ShouldReturnChatRoomsForUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var chatRooms = new List<ChatRoom>
        {
            new ChatRoom { Id = Guid.NewGuid(), Name = "Chat Room 1", CreatedAt = DateTime.Now },
            new ChatRoom { Id = Guid.NewGuid(), Name = "Chat Room 2", CreatedAt = DateTime.Now }
        };

        var roomParticipants = new List<RoomParticipant>
        {
            new RoomParticipant { Id = Guid.NewGuid(), ChatRoomId = chatRooms[0].Id, UserId = userId },
            new RoomParticipant { Id = Guid.NewGuid(), ChatRoomId = chatRooms[1].Id, UserId = userId }
        }.AsQueryable();

        _mockContext.Setup(c => c.RoomParticipants).ReturnsDbSet(roomParticipants);
        _mockContext.Setup(c => c.ChatRooms).ReturnsDbSet(chatRooms.AsQueryable());

        // Act
        var result = await _chatRoomRepository.GetUserChatRoomsAsync(userId);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());
    }
}
