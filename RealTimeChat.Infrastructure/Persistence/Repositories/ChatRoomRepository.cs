using Microsoft.EntityFrameworkCore;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Repositories;
using RealTimeChat.Infrastructure.Persistence.Context;

namespace RealTimeChat.Infrastructure.Persistence.Repositories;

public class ChatRoomRepository : IChatRoomRepository
{
    private readonly RealTimeChatDbContext _dbContext;

    public ChatRoomRepository(RealTimeChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(ChatRoom chatRoom)
    {
        await _dbContext.ChatRooms.AddAsync(chatRoom);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(ChatRoom chatRoom)
    {
        _dbContext.ChatRooms.Remove(chatRoom);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ChatRoom?> GetByIdAsync(Guid chatRoomId)
    {
        return await _dbContext.ChatRooms
            .Include(cr => cr.Messages) 
            .Include(cr => cr.RoomParticipants) 
            .SingleOrDefaultAsync(cr => cr.Id == chatRoomId);
    }

    public async Task UpdateAsync(ChatRoom chatRoom)
    {
        _dbContext.ChatRooms.Update(chatRoom);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Message>> GetMessagesInRoomAsync(int skip, int take, Guid chatRoomId)
    {
        return await _dbContext.Messages
            .Where(m => m.ChatRoomId == chatRoomId)
            .OrderBy(m => m.Timestamp)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatRoom>> GetUserChatRoomsAsync(Guid userId)
    {
        return await _dbContext.RoomParticipants
            .Where(rp => rp.UserId == userId)
            .Select(rp => rp.ChatRoom)
            .ToListAsync();
    }

    public async Task<ICollection<ChatRoom>> GetAsync(int skip, int take)
    {
        return await _dbContext.ChatRooms
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}
