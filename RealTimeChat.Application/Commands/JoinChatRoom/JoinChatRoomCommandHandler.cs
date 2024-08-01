using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Exceptions;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Commands.JoinChatRoom;

public class JoinChatRoomCommandHandler : IRequestHandler<JoinChatRoomCommand, ResultDTO>
{
    private readonly IChatRoomRepository _chatRoomRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoomParticipantRepository _roomParticipantRepository;

    public JoinChatRoomCommandHandler(IChatRoomRepository chatRoomRepository, IUserRepository userRepository, IRoomParticipantRepository roomParticipantRepository)
    {
        _chatRoomRepository = chatRoomRepository;
        _userRepository = userRepository;
        _roomParticipantRepository = roomParticipantRepository;
    }

    public async Task<ResultDTO> Handle(JoinChatRoomCommand request, CancellationToken cancellationToken)
    {
        var roomParticipantExists = await _roomParticipantRepository.GetRoomParticipantByRoomAndUserId(request.ChatRoomId, request.UserId);
        if (roomParticipantExists != null)
        {
            throw new InvalidOperationException("You are already in this chat room");
        }

        var roomParticipant = new RoomParticipant
        {
            Id = Guid.NewGuid(),
            ChatRoomId = request.ChatRoomId,
            UserId = request.UserId
        };

        await _roomParticipantRepository.AddAsync(roomParticipant);

        return ResultDTO.SuccessResult(new { request.UserId,request.ChatRoomId, RoomParticipantId = roomParticipant.Id }, $"User has successfully entered the chat room");
    }
}
