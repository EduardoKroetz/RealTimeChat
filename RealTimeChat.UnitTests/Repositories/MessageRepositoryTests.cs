using Moq;
using Moq.EntityFrameworkCore;
using RealTimeChat.Core.Entities;
using RealTimeChat.Infrastructure.Persistence.Context;
using RealTimeChat.Infrastructure.Persistence.Repositories;

namespace RealTimeChat.UnitTests.Repositories;

public class MessageRepositoryTests
{
    private readonly Mock<RealTimeChatDbContext> _mockContext;
    private readonly MessageRepository _messageRepository;

    public MessageRepositoryTests()
    {
        _mockContext = new Mock<RealTimeChatDbContext>();

        var messages = new List<Message>
        {
            new Message { Id = Guid.NewGuid(), Content = "Hello", ChatRoomId = Guid.NewGuid(), SenderId = Guid.NewGuid(), Timestamp = DateTime.UtcNow }
        }.AsQueryable();

        _mockContext.Setup(c => c.Messages).ReturnsDbSet(messages);

        _messageRepository = new MessageRepository(_mockContext.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldAddMessageToDatabase()
    {
        // Arrange
        var message = new Message { Id = Guid.NewGuid(), Content = "New Message", ChatRoomId = Guid.NewGuid(), SenderId = Guid.NewGuid(), Timestamp = DateTime.UtcNow };

        var messages = new List<Message>().AsQueryable();
        _mockContext.Setup(c => c.Messages).ReturnsDbSet(messages);

        // Act
        await _messageRepository.AddAsync(message);

        // Assert
        _mockContext.Verify(c => c.Messages.AddAsync(message, default), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMessageInDatabase()
    {
        // Arrange
        var message = new Message { Id = Guid.NewGuid(), Content = "Old Message", ChatRoomId = Guid.NewGuid(), SenderId = Guid.NewGuid(), Timestamp = DateTime.UtcNow };

        var messages = new List<Message>() { message }.AsQueryable();
        _mockContext.Setup(c => c.Messages).ReturnsDbSet(messages);

        message.Content = "Updated Message";

        // Act
        await _messageRepository.UpdateAsync(message);

        // Assert
        _mockContext.Verify(c => c.Messages.Update(message), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveMessageFromDatabase()
    {
        // Arrange
        var message = new Message { Id = Guid.NewGuid(), Content = "Message to Delete", ChatRoomId = Guid.NewGuid(), SenderId = Guid.NewGuid(), Timestamp = DateTime.UtcNow };

        var messages = new List<Message>() { message }.AsQueryable();
        _mockContext.Setup(c => c.Messages).ReturnsDbSet(messages);

        // Act
        await _messageRepository.DeleteAsync(message);

        // Assert
        _mockContext.Verify(c => c.Messages.Remove(message), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMessage()
    {
        // Arrange
        var messageId = _mockContext.Object.Messages.First().Id;

        // Act
        var result = await _messageRepository.GetByIdAsync(messageId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(messageId, result.Id);
    }

    [Fact]
    public async Task GetMessagesByRoomIdAsync_ShouldReturnMessages()
    {
        // Arrange
        var chatRoomId = _mockContext.Object.Messages.First().ChatRoomId;

        // Act
        var messages = await _messageRepository.GetMessagesByRoomIdAsync(chatRoomId);

        // Assert
        Assert.NotEmpty(messages);
        Assert.All(messages, m => Assert.Equal(chatRoomId, m.ChatRoomId));
    }

    [Fact]
    public async Task GetMessagesBySenderIdAsync_ShouldReturnMessages()
    {
        // Arrange
        var senderId = _mockContext.Object.Messages.First().SenderId;

        // Act
        var messages = await _messageRepository.GetMessagesBySenderIdAsync(senderId);

        // Assert
        Assert.NotEmpty(messages);
        Assert.All(messages, m => Assert.Equal(senderId, m.SenderId));
    }
}
