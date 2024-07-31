
using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Queries.GetChatRoom;

public class GetChatRoomQuery : IRequest<ResultDTO>
{
    public Guid ChatRoomId { get; set; }
}
