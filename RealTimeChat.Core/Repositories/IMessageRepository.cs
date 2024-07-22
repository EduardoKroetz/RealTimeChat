using RealTimeChat.Core.Entities;

namespace RealTimeChat.Core.Repositories;

public interface IMessageRepository
{
    Task<Message?> GetByIdAsync(Guid messageId);
    Task AddAsync(Message message);
    Task UpdateAsync(Message message);
    Task DeleteAsync(Message message);
    Task<IEnumerable<Message>> GetMessagesByRoomIdAsync(Guid chatRoomId);
    Task<IEnumerable<Message>> GetMessagesBySenderIdAsync(Guid senderId);
}