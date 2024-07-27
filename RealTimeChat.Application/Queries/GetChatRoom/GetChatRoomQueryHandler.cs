
using MediatR;
using RealTimeChat.Application.ViewModels;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Exceptions;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Queries.GetChatRoom;

public class GetChatRoomQueryHandler : IRequestHandler<GetChatRoomQuery, Result>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public GetChatRoomQueryHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<Result> Handle(GetChatRoomQuery request, CancellationToken cancellationToken)
    {
        var chatRoom = await _chatRoomRepository.GetByIdAsync(request.ChatRoomId);
        if (chatRoom == null)
        {
            throw new NotFoundException("Chat room not found");
        }

        var data = new GetChatRoomsViewModel(chatRoom.Id, chatRoom.Name, chatRoom.CreatedAt, chatRoom.CreatedBy);

        return Result.SuccessResult(data, "Success!");
    }
}
