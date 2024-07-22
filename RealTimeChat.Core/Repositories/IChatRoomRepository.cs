using RealTimeChat.Core.Entities;

namespace RealTimeChat.Core.Repositories;

public interface IChatRoomRepository
{
    Task<ChatRoom> GetByIdAsync(int chatRoomId);
    Task AddAsync(ChatRoom chatRoom);
    Task UpdateAsync(ChatRoom chatRoom);
    Task DeleteAsync(int chatRoomId);
    Task<IEnumerable<ChatRoom>> GetUserChatRoomsAsync(int userId);
    Task<IEnumerable<Message>> GetMessagesInRoomAsync(int chatRoomId);
}