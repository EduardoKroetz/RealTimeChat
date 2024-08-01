
using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Queries.GetChatRoomsByName;

public class GetChatRoomByNameQuery : IRequest<ResultDTO>
{
    public string Name { get; set; }
    public Guid UserId { get; set; }
}
