using Microsoft.EntityFrameworkCore;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Repositories;
using RealTimeChat.Infrastructure.Persistence.Context;

namespace RealTimeChat.Infrastructure.Persistence.Repositories;

public class RoomParticipantRepository : IRoomParticipantRepository
{
    private readonly RealTimeChatDbContext _dbContext;

    public RoomParticipantRepository(RealTimeChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RoomParticipant?> GetByIdAsync(Guid roomParticipantId)
    {
        return await _dbContext.RoomParticipants
            .Include(rp => rp.ChatRoom) 
            .Include(rp => rp.User) 
            .SingleOrDefaultAsync(rp => rp.Id == roomParticipantId);
    }

    public async Task AddAsync(RoomParticipant roomParticipant)
    {
        await _dbContext.RoomParticipants.AddAsync(roomParticipant);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(RoomParticipant roomParticipant)
    {
        _dbContext.RoomParticipants.Update(roomParticipant);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(RoomParticipant roomParticipant)
    {
        _dbContext.RoomParticipants.Remove(roomParticipant);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<RoomParticipant>> GetParticipantsInRoomAsync(Guid chatRoomId)
    {
        return await _dbContext.RoomParticipants
            .Where(rp => rp.ChatRoomId == chatRoomId)
            .Include(rp => rp.User)
            .ToListAsync();
    }

    public async Task<IEnumerable<RoomParticipant>> GetRoomsByUserIdAsync(Guid userId)
    {
        return await _dbContext.RoomParticipants
            .Where(rp => rp.UserId == userId)
            .Include(rp => rp.ChatRoom) 
            .ToListAsync();
    }
}
