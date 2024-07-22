using RealTimeChat.Core.Entities;

namespace RealTimeChat.Core.Repositories;

public interface IRoomParticipantRepository
{
    Task<RoomParticipant?> GetByIdAsync(Guid roomParticipantId);
    Task AddAsync(RoomParticipant roomParticipant);
    Task UpdateAsync(RoomParticipant roomParticipant);
    Task DeleteAsync(RoomParticipant roomParticipant);
    Task<IEnumerable<RoomParticipant>> GetParticipantsInRoomAsync(Guid chatRoomId);
    Task<IEnumerable<RoomParticipant>> GetRoomsByUserIdAsync(Guid userId);
}
