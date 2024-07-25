using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Commands.CreateChatRoom;

public class CreateChatRoomCommandHandler : IRequestHandler<CreateChatRoomCommand, Result>
{
    private readonly IChatRoomRepository _chatRoomRepository;

    public CreateChatRoomCommandHandler(IChatRoomRepository chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task<Result> Handle(CreateChatRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new ChatRoom
        {
            Id = Guid.NewGuid(),
            Messages = [],
            Name = request.Name,
            RoomParticipants = [],
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.UserId
        };

        await _chatRoomRepository.AddAsync(room);

        return Result.SuccessResult(new { id = room.Id }, "Chat room successfully created");
    }
}
