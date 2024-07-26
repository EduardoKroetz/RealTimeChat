using Microsoft.EntityFrameworkCore;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Repositories;
using RealTimeChat.Infrastructure.Persistence.Context;

namespace RealTimeChat.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly RealTimeChatDbContext _context;

    public UserRepository(RealTimeChatDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetUsersInRoomAsync(Guid chatRoomId)
    {
        return await _context.RoomParticipants
            .Where(rp => rp.ChatRoomId == chatRoomId)
            .Select(rp => rp.User)
            .ToListAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<IEnumerable<User>> GetAsync(int skip, int take)
    {
        return await _context.Users
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<object?> GetUserIncludesRoomParticipants(Guid userId)
    {
        return await _context.Users
            .Include(x => x.RoomParticipants)
            .ThenInclude(x => x.ChatRoom)
            .Select(x => new
            {
                x.Id,
                x.Username,
                x.Email,
                x.CreatedAt,
                RoomParticipants = x.RoomParticipants.Select(r => new
                {
                    r.Id,
                    r.ChatRoomId,
                    ChatRoomName = r.ChatRoom.Name
                })
            })
            .FirstOrDefaultAsync(x => x.Id == userId);
    }
}
