using Microsoft.EntityFrameworkCore;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Repositories;
using RealTimeChat.Infrastructure.Persistence.Context;

namespace RealTimeChat.Infrastructure.Persistence.Repositories;

public class MessageRepository : IMessageRepository
{
    private readonly RealTimeChatDbContext _dbContext;

    public MessageRepository(RealTimeChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Message message)
    {
        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Message message)
    {
        _dbContext.Messages.Update(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Message message)
    {
        _dbContext.Messages.Remove(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Message?> GetByIdAsync(Guid messageId)
    {
        return await _dbContext.Messages.Include(x => x.Sender).FirstOrDefaultAsync(x => x.Id == messageId);
    }

    public async Task<IEnumerable<Message>> GetMessagesByRoomIdAsync(Guid chatRoomId)
    {
        return await _dbContext.Messages.Where(x => x.ChatRoomId == chatRoomId).ToListAsync();
    }

    public async Task<IEnumerable<Message>> GetMessagesBySenderIdAsync(Guid senderId)
    {
        return await _dbContext.Messages.Where(x => x.SenderId == senderId).ToListAsync();
    }
}
