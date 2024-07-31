

using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Queries.GetUserChatRooms;

public class GetUserChatRoomsQuery : IRequest<ResultDTO>
{
    public Guid UserId { get; set; }
}
