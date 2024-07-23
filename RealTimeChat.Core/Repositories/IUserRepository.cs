
using RealTimeChat.Core.Entities;

namespace RealTimeChat.Core.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetUserByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<IEnumerable<User>> GetUsersInRoomAsync(Guid chatRoomId);
}