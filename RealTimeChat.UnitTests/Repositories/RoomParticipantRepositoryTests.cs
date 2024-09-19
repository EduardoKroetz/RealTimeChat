using Moq;
using Moq.EntityFrameworkCore;
using RealTimeChat.Core.Entities;
using RealTimeChat.Infrastructure.Persistence.Context;
using RealTimeChat.Infrastructure.Persistence.Repositories;

namespace RealTimeChat.UnitTests.Repositories;

public class RoomParticipantRepositoryTests
{
    private readonly Mock<RealTimeChatDbContext> _mockContext;
    private readonly RoomParticipantRepository _roomParticipantRepository;

    public RoomParticipantRepositoryTests()
    {
        _mockContext = new Mock<RealTimeChatDbContext>();

        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Username = "JohnDoe", CreatedAt = DateTime.UtcNow, Email = "johndoe@gmail.com", PasswordHash = "" }
        }.AsQueryable();

        var chatRooms = new List<ChatRoom>
        {
            new ChatRoom { Id = Guid.NewGuid(), CreatedAt = DateTime.UtcNow, CreatedBy = users.First().Id, Name = "ChatRoom" }
        }.AsQueryable();

        var roomParticipants = new List<RoomParticipant>
        {
            new RoomParticipant { Id = Guid.NewGuid(), ChatRoomId = chatRooms.First().Id, UserId = users.First().Id }
        }.AsQueryable();

        _mockContext.Setup(c => c.Users).ReturnsDbSet(users);
        _mockContext.Setup(c => c.ChatRooms).ReturnsDbSet(chatRooms);
        _mockContext.Setup(c => c.RoomParticipants).ReturnsDbSet(roomParticipants);

        _roomParticipantRepository = new RoomParticipantRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRoomParticipant()
    {
        // Arrange
        var roomParticipantId = _mockContext.Object.RoomParticipants.First().Id;

        // Act
        var result = await _roomParticipantRepository.GetByIdAsync(roomParticipantId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(roomParticipantId, result.Id);
    }

    [Fact]
    public async Task AddAsync_ShouldAddRoomParticipantToDatabase()
    {
        // Arrange
        var roomParticipant = new RoomParticipant { Id = Guid.NewGuid(), ChatRoomId = Guid.NewGuid(), UserId = Guid.NewGuid() };

        var roomParticipants = new List<RoomParticipant>().AsQueryable();
        _mockContext.Setup(c => c.RoomParticipants).ReturnsDbSet(roomParticipants);

        // Act
        await _roomParticipantRepository.AddAsync(roomParticipant);

        // Assert
        _mockContext.Verify(c => c.RoomParticipants.AddAsync(roomParticipant, default), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveRoomParticipantFromDatabase()
    {
        // Arrange
        var roomParticipant = new RoomParticipant { Id = Guid.NewGuid(), ChatRoomId = Guid.NewGuid(), UserId = Guid.NewGuid() };

        var roomParticipants = new List<RoomParticipant>() { roomParticipant }.AsQueryable();
        _mockContext.Setup(c => c.RoomParticipants).ReturnsDbSet(roomParticipants);

        // Act
        await _roomParticipantRepository.DeleteAsync(roomParticipant);

        // Assert
        _mockContext.Verify(c => c.RoomParticipants.Remove(roomParticipant), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateRoomParticipantInDatabase()
    {
        // Arrange
        var roomParticipant = new RoomParticipant { Id = Guid.NewGuid(), ChatRoomId = Guid.NewGuid(), UserId = Guid.NewGuid() };

        var roomParticipants = new List<RoomParticipant>() { roomParticipant }.AsQueryable();
        _mockContext.Setup(c => c.RoomParticipants).ReturnsDbSet(roomParticipants);

        roomParticipant.ChatRoomId = Guid.NewGuid();

        // Act
        await _roomParticipantRepository.UpdateAsync(roomParticipant);

        // Assert
        _mockContext.Verify(c => c.RoomParticipants.Update(roomParticipant), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetParticipantsInRoomAsync_ShouldReturnParticipants()
    {
        // Arrange
        var chatRoomId = _mockContext.Object.ChatRooms.First().Id;
        var user = _mockContext.Object.Users.First();
        var roomParticipant = new RoomParticipant { Id = Guid.NewGuid(), ChatRoomId = chatRoomId, UserId = user.Id, User = user };

        _mockContext.Setup(c => c.RoomParticipants).ReturnsDbSet(new List<RoomParticipant>() { roomParticipant }.AsQueryable());

        // Act
        var participants = await _roomParticipantRepository.GetParticipantsInRoomAsync(chatRoomId);

        // Assert
        Assert.NotEmpty(participants);
        Assert.Contains(participants, rp => rp.ChatRoomId == chatRoomId);
        Assert.Equal(user, participants.First().User);
    }

    [Fact]
    public async Task GetRoomsByUserIdAsync_ShouldReturnRooms()
    {
        // Arrange
        var userId = _mockContext.Object.Users.First().Id;
        var chatRoom = _mockContext.Object.ChatRooms.First();
        var roomParticipant = new RoomParticipant { Id = Guid.NewGuid(), ChatRoomId = chatRoom.Id, UserId = userId, ChatRoom = chatRoom };

        _mockContext.Setup(c => c.RoomParticipants).ReturnsDbSet(new List<RoomParticipant>() { roomParticipant }.AsQueryable());

        // Act
        var rooms = await _roomParticipantRepository.GetRoomsByUserIdAsync(userId);

        // Assert
        Assert.NotEmpty(rooms);
        Assert.Contains(rooms, rp => rp.UserId == userId);
        Assert.Equal(chatRoom, rooms.First().ChatRoom);
    }
}
