

using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Exceptions;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Commands.UpdateChatRoom;

public class UpdateChatRoomCommandHandler : IRequestHandler<UpdateChatRoomCommand, Result>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public UpdateChatRoomCommandHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }
    public async Task<Result> Handle(UpdateChatRoomCommand request, CancellationToken cancellationToken)
    {
        var chatRoom = await _chatRoomRepository.GetByIdAsync(request.Id);
        if (chatRoom == null)
        {
            throw new NotFoundException("Chat room not found");
        }

        chatRoom.Name = request.Name;

        await _chatRoomRepository.UpdateAsync(chatRoom);

        return Result.SuccessResult(new { id = chatRoom.Id }, "Chat room updated");
    }
}
