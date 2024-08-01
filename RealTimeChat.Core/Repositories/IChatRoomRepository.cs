using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Entities;

namespace RealTimeChat.Core.Repositories;

public interface IChatRoomRepository
{
    Task<ICollection<ChatRoom>> GetAsync(int skip, int take);
    Task<ChatRoom?> GetByIdAsync(Guid chatRoomId);
    Task<IEnumerable<GetChatRoomsDTO>> GetChatRoomsByName(Guid userId ,string name);
    Task AddAsync(ChatRoom chatRoom);
    Task UpdateAsync(ChatRoom chatRoom);
    Task DeleteAsync(ChatRoom chatRoom);
    Task<IEnumerable<ChatRoom>> GetUserChatRoomsAsync(Guid userId);
    Task<IEnumerable<Message>> GetMessagesInRoomAsync(int skip, int take ,Guid chatRoomId);
}