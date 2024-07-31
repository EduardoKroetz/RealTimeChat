
using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Exceptions;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Queries.GetChatRoom;

public class GetChatRoomQueryHandler : IRequestHandler<GetChatRoomQuery, ResultDTO>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public GetChatRoomQueryHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<ResultDTO> Handle(GetChatRoomQuery request, CancellationToken cancellationToken)
    {
        var chatRoom = await _chatRoomRepository.GetByIdAsync(request.ChatRoomId);
        if (chatRoom == null)
        {
            throw new NotFoundException("Chat room not found");
        }

        var data = new GetChatRoomsDTO(chatRoom.Id, chatRoom.Name, chatRoom.CreatedAt, chatRoom.CreatedBy);

        return ResultDTO.SuccessResult(data, "Success!");
    }
}
