using MediatR;
using RealTimeChat.Core.DTOs;
using RealTimeChat.Core.Exceptions;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.Application.Commands.LeaveChatRoom;

public class LeaveChatRoomCommandHandler : IRequestHandler<LeaveChatRoomCommand, Result>
{
    private readonly IChatRoomRepository _chatRoomRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoomParticipantRepository _roomParticipantRepository;

    public LeaveChatRoomCommandHandler(IChatRoomRepository chatRoomRepository, IUserRepository userRepository, IRoomParticipantRepository roomParticipantRepository)
    {
        _chatRoomRepository = chatRoomRepository;
        _userRepository = userRepository;
        _roomParticipantRepository = roomParticipantRepository;
    }

    public async Task<Result> Handle(LeaveChatRoomCommand request, CancellationToken cancellationToken)
    {
        var roomParticipant = await _roomParticipantRepository.GetRoomParticipantByRoomAndUserId(request.ChatRoomId, request.UserId);
        if (roomParticipant == null)
        {
            throw new NotFoundException($"Room participant not found");
        }

        await _roomParticipantRepository.DeleteAsync(roomParticipant);

        return Result.SuccessResult(new { request.UserId, request.ChatRoomId, roomParticipantId = roomParticipant.Id }, "User leave the chat room successfully");
    }
}
