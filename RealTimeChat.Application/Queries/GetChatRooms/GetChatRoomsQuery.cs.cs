using MediatR;
using RealTimeChat.Core.DTOs;

namespace RealTimeChat.Application.Queries.GetAllChatRooms;

public class GetChatRoomsQuery : IRequest<PagedResult>
{
    public int Skip => ( PageNumber - 1 ) * PageSize;
    public int Take => PageSize;
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
