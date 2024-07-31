

using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Queries.GetUserChatRooms;

public class GetUserChatRoomsQueryHandler : IRequestHandler<GetUserChatRoomsQuery, ResultDTO>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public GetUserChatRoomsQueryHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<ResultDTO> Handle(GetUserChatRoomsQuery request, CancellationToken cancellationToken)
    {
        var chatRooms = await _chatRoomRepository.GetUserChatRoomsAsync(request.UserId);
        var data = chatRooms.Select(x => new GetChatRoomsDTO(x.Id, x.Name, x.CreatedAt, x.CreatedBy)).ToList();
        return ResultDTO.SuccessResult(data, "Success!");
    }
}
