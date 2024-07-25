
using MediatR;
using RealTimeChat.Application.Queries.GetAllChatRooms;
using RealTimeChat.Application.ViewModels;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Queries.GetChatRooms;

public class GetChatRoomsQueryHandler : IRequestHandler<GetChatRoomsQuery, PagedResult>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public GetChatRoomsQueryHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<PagedResult> Handle(GetChatRoomsQuery request, CancellationToken cancellationToken)
    {
        var chatRooms = await _chatRoomRepository.GetAsync(request.Skip, request.Take);
        var selectedChatRooms = chatRooms.Select(x => 
            new GetChatRoomsViewModel(x.Id, x.Name, x.CreatedAt, x.CreatedBy)
        ).ToList();

        return PagedResult.SuccessResult(selectedChatRooms, request.PageNumber, request.PageSize, selectedChatRooms.Count, "Success");
    }
}
