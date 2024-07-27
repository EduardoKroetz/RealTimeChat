

using MediatR;
using RealTimeChat.Application.ViewModels;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Queries.GetUserChatRooms;

public class GetUserChatRoomsQueryHandler : IRequestHandler<GetUserChatRoomsQuery, Result>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public GetUserChatRoomsQueryHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<Result> Handle(GetUserChatRoomsQuery request, CancellationToken cancellationToken)
    {
        var chatRooms = await _chatRoomRepository.GetUserChatRoomsAsync(request.UserId);
        var data = chatRooms.Select(x => new GetChatRoomsViewModel(x.Id, x.Name, x.CreatedAt, x.CreatedBy)).ToList();
        return Result.SuccessResult(data, "Success!");
    }
}
