
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Entities;

namespace RealTimeChat.Core.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId);
    Task<IEnumerable<GetUserDTO>> GetAsync(int skip, int take);
    Task<User?> GetUserByEmailAsync(string email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<IEnumerable<GetUserDTO>> GetUsersInRoomAsync(Guid chatRoomId);
    Task<object?> GetUserIncludesRoomParticipants(Guid userId);
}