
using RealTimeChat.Core.Entities;

namespace RealTimeChat.Core.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int userId);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int userId);
    Task<IEnumerable<User>> GetUsersInRoomAsync(int chatRoomId);
}