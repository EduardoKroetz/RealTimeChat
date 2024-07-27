

using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Queries.GetUserChatRooms;

public class GetUserChatRoomsQuery : IRequest<Result>
{
    public Guid UserId { get; set; }
}
