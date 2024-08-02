

using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Exceptions;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Commands.UpdateChatRoom;

public class UpdateChatRoomCommandHandler : IRequestHandler<UpdateChatRoomCommand, ResultDTO>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public UpdateChatRoomCommandHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }
    public async Task<ResultDTO> Handle(UpdateChatRoomCommand request, CancellationToken cancellationToken)
    {
        var chatRoom = await _chatRoomRepository.GetByIdAsync(request.Id);
        if (chatRoom == null)
        {
            throw new NotFoundException("Chat room not found");
        }

        if (chatRoom.CreatedBy != request.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update the chat room");
        }

        chatRoom.Name = request.Name;

        await _chatRoomRepository.UpdateAsync(chatRoom);

        return ResultDTO.SuccessResult(new { id = chatRoom.Id }, "Chat room updated successfully!");
    }
}
