using RealTimeChat.Core.Entities;

namespace RealTimeChat.Core.Repositories;

public interface IMessageRepository
{
    Task<Message> GetByIdAsync(int messageId);
    Task AddAsync(Message message);
    Task UpdateAsync(Message message);
    Task DeleteAsync(int messageId);
    Task<IEnumerable<Message>> GetMessagesByRoomIdAsync(int chatRoomId);
    Task<IEnumerable<Message>> GetMessagesBySenderIdAsync(int senderId);
}