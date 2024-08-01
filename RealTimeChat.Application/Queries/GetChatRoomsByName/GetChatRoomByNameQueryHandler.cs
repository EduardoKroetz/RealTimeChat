
using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Queries.GetChatRoomsByName;

public class GetChatRoomByNameQueryHandler : IRequestHandler<GetChatRoomByNameQuery, ResultDTO>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public GetChatRoomByNameQueryHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<ResultDTO> Handle(GetChatRoomByNameQuery request, CancellationToken cancellationToken)
    {
        var chatRooms = await _chatRoomRepository.GetChatRoomsByName(request.UserId ,request.Name);
        return ResultDTO.SuccessResult(chatRooms, "Success!");
    }
}
