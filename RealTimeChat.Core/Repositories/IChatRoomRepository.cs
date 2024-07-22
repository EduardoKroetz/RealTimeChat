﻿using RealTimeChat.Core.Entities;

namespace RealTimeChat.Core.Repositories;

public interface IChatRoomRepository
{
    Task<ChatRoom?> GetByIdAsync(Guid chatRoomId);
    Task AddAsync(ChatRoom chatRoom);
    Task UpdateAsync(ChatRoom chatRoom);
    Task DeleteAsync(ChatRoom chatRoom);
    Task<IEnumerable<ChatRoom>> GetUserChatRoomsAsync(Guid userId);
    Task<IEnumerable<Message>> GetMessagesInRoomAsync(Guid chatRoomId);
}