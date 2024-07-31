using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Queries.GetMessages;

public class GetChatRoomMessagesQuery : IRequest<PagedResultDTO>
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public Guid ChatRoomId { get; set; }
}
