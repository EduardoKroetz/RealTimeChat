using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Commands.CreateChatRoom;

public class CreateChatRoomCommandHandler : IRequestHandler<CreateChatRoomCommand, ResultDTO>
{
    private readonly IChatRoomRepository _chatRoomRepository;
    private readonly IRoomParticipantRepository _roomParticipantRepository;

    public CreateChatRoomCommandHandler(IChatRoomRepository chatRoomRepository, IRoomParticipantRepository roomParticipantRepository)
    {
        _chatRoomRepository = chatRoomRepository;
        _roomParticipantRepository = roomParticipantRepository;
    }

    public async Task<ResultDTO> Handle(CreateChatRoomCommand request, CancellationToken cancellationToken)
    {
        var chatRoom = new ChatRoom
        {
            Id = Guid.NewGuid(),
            Messages = [],
            Name = request.Name,
            RoomParticipants = [],
            CreatedAt = DateTime.UtcNow,
            CreatedBy = request.UserId
        };

        await _chatRoomRepository.AddAsync(chatRoom);

        var roomParticipant = new RoomParticipant
        {
            Id = Guid.NewGuid(),
            ChatRoomId = chatRoom.Id,
            UserId = request.UserId
        };

        await _roomParticipantRepository.AddAsync(roomParticipant);

        return ResultDTO.SuccessResult(new { id = chatRoom.Id, roomParticipantId = roomParticipant.Id }, "Chat room successfully created");
    }
}