
using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Queries.GetChatRoom;

public class GetChatRoomQuery : IRequest<Result>
{
    public Guid ChatRoomId { get; set; }
}
