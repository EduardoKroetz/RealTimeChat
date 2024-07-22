using Microsoft.EntityFrameworkCore;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Data.Repositories;

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

    public async Task<IEnumerable<Message>> GetMessagesInRoomAsync(Guid chatRoomId)
    {
        return await _dbContext.Messages
            .Where(m => m.ChatRoomId == chatRoomId)
            .OrderBy(m => m.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatRoom>> GetUserChatRoomsAsync(Guid userId)
    {
        return await _dbContext.RoomParticipants
            .Where(rp => rp.UserId == userId)
            .Select(rp => rp.ChatRoom)
            .ToListAsync();
    }


}
