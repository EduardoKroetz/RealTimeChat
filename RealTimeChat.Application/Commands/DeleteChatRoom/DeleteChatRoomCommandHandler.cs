using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Exceptions;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Commands.DeleteChatRoom;

public class DeleteChatRoomCommandHandler : IRequestHandler<DeleteChatRoomCommand, ResultDTO>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public DeleteChatRoomCommandHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<ResultDTO> Handle(DeleteChatRoomCommand request, CancellationToken cancellationToken)
    {
        var chatRoom = await _chatRoomRepository.GetByIdAsync(request.ChatRoomId);
        if (chatRoom == null)
        {
            throw new NotFoundException("Chat room not found");
        }

        await _chatRoomRepository.DeleteAsync(chatRoom);

        return ResultDTO.SuccessResult(new { id = request.ChatRoomId }, "Chat room deleted");
    }
}
