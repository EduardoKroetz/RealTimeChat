using Moq;
using Moq.EntityFrameworkCore;
using RealTimeChat.Core.Entities;
using RealTimeChat.Infrastructure.Persistence.Context;
using RealTimeChat.Infrastructure.Persistence.Repositories;

namespace RealTimeChat.UnitTests.Repositories;

public class UserRepositoryTests
{
    private readonly Mock<RealTimeChatDbContext> _mockContext;
    private readonly UserRepository _userRepository;

    public UserRepositoryTests()
    {
        _mockContext = new Mock<RealTimeChatDbContext>();

        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Username = "JohnDoe", CreatedAt = DateTime.Now, Email = "johndoe@gmail.com", PasswordHash = "" }
        }.AsQueryable();

        _mockContext.Setup(c => c.Users).ReturnsDbSet(users);

        _userRepository = new UserRepository(_mockContext.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser()
    {
        // Arrange
        var userId = _mockContext.Object.Users.First().Id;

        // Act
        var result = await _userRepository.GetByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public async Task AddAsync_ShouldAddUserToDatabase()
    {
        //Arrange
        var user = new User { Id = Guid.NewGuid(), Username = "Luana", CreatedAt = DateTime.Now, Email = "luana@gmail.com", PasswordHash = "" };

        var users = new List<User>().AsQueryable();
        _mockContext.Setup(c => c.Users).ReturnsDbSet(users);

        //Act
        await _userRepository.AddAsync(user);

        //Assert
        _mockContext.Verify(c => c.Users.AddAsync(user, default), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveUserFromDatabase()
    {
        //Arrange
        var user = new User { Id = Guid.NewGuid(), Username = "Luana", CreatedAt = DateTime.Now, Email = "luana@gmail.com", PasswordHash = "" };

        var users = new List<User>() { user }.AsQueryable();
        _mockContext.Setup(c => c.Users).ReturnsDbSet(users);

        //Act
        await _userRepository.DeleteAsync(user);

        //Assert
        _mockContext.Verify(c => c.Users.Remove(user), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUserInDatabase()
    {
        //Arrange
        var user = new User { Id = Guid.NewGuid(), Username = "Luana", CreatedAt = DateTime.Now, Email = "luana@gmail.com", PasswordHash = "" };

        var users = new List<User>() { user } .AsQueryable();
        _mockContext.Setup(c => c.Users).ReturnsDbSet(users);

        user.Email = "luana1000@gmail.com";

        //Act
        await _userRepository.UpdateAsync(user);

        //Assert
        _mockContext.Verify(c => c.Users.Update(user), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetUsersInChatRooms_ShouldReturnUsers()
    {
        //Arrange
        var user = new User { Id = Guid.NewGuid(), Username = "Luana", CreatedAt = DateTime.Now, Email = "luana@gmail.com", PasswordHash = "" };
        var chatRoom = new ChatRoom { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, CreatedBy = user.Id, CreatedByUser = user,Name = "ChatRoom"};
        var roomParticipant = new RoomParticipant { Id = Guid.NewGuid(),ChatRoom = chatRoom,ChatRoomId = chatRoom.Id, User = user,UserId = user.Id};
        
        _mockContext.Setup(c => c.Users).ReturnsDbSet(new List<User>() { user }.AsQueryable());
        _mockContext.Setup(c => c.ChatRooms).ReturnsDbSet(new List<ChatRoom>() { chatRoom }.AsQueryable());
        _mockContext.Setup(c => c.RoomParticipants).ReturnsDbSet(new List<RoomParticipant>() { roomParticipant }.AsQueryable());

        //Act
        var users = await _userRepository.GetUsersInRoomAsync(chatRoom.Id);

        //Assert
        Assert.True(users.Any());
        Assert.Equal(user, users.First());
    }

    [Fact]
    public async Task GetUserByEmail_ShouldReturnUserByEmail()
    {
        // Arrange
        var email = "johndoe@gmail.com";
        var user = new User { Id = Guid.NewGuid(), Username = "JohnDoe", CreatedAt = DateTime.Now, Email = email, PasswordHash = "" };

        var users = new List<User> { user }.AsQueryable();
        _mockContext.Setup(c => c.Users).ReturnsDbSet(users);

        // Act
        var result = await _userRepository.GetUserByEmailAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(email, result.Email);
    }
}
