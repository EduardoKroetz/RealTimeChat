using RealTimeChat.Core.Entities;

namespace RealTimeChat.Core.Repositories;

public interface IRoomParticipantRepository
{
    Task<RoomParticipant> GetByIdAsync(int roomParticipantId);
    Task AddAsync(RoomParticipant roomParticipant);
    Task UpdateAsync(RoomParticipant roomParticipant);
    Task DeleteAsync(int roomParticipantId);
    Task<IEnumerable<RoomParticipant>> GetParticipantsInRoomAsync(int chatRoomId);
    Task<IEnumerable<RoomParticipant>> GetRoomsByUserIdAsync(int userId);
}
