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
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var chatRoom = await _chatRoomRepository.GetByIdAsync(request.ChatRoomId);
        if (chatRoom == null)
        {
            throw new NotFoundException("Chat room not found");
        }

        var userChatRooms = await _chatRoomRepository.GetUserChatRoomsAsync(user.Id);
        if (userChatRooms.Contains(chatRoom))
        {
            throw new InvalidOperationException("This user is already in this chat room");
        }

        var roomParticipant = new RoomParticipant
        {
            Id = Guid.NewGuid(),
            ChatRoomId = chatRoom.Id,
            UserId = user.Id
        };

        await _roomParticipantRepository.AddAsync(roomParticipant);

        return ResultDTO.SuccessResult(new { UserId = user.Id, ChatRoomId = chatRoom.Id, RoomParticipantId = roomParticipant.Id }, $"User has successfully entered the chat room");
    }
}
