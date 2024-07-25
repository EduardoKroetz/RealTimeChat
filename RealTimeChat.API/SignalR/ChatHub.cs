using Microsoft.AspNetCore.SignalR;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Exceptions;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.API.SignalR;

public class ChatHub : Hub
{
    private readonly IMessageRepository _messageRepository;
    private readonly IRoomParticipantRepository _roomParticipantRepository;

    public ChatHub(IMessageRepository messageRepository, IRoomParticipantRepository roomParticipantRepository)
    {
        _messageRepository = messageRepository;
        _roomParticipantRepository = roomParticipantRepository;
    }

    public async Task SendMessageAsync(Guid chatRoomId, Guid userId,  string message)
    {
        var chatMessage = new Message
        {
            ChatRoomId = chatRoomId,
            SenderId = userId,
            Content = message,
            Id = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
        };

        await _messageRepository.AddAsync(chatMessage);

        await Clients.Group(chatRoomId.ToString()).SendAsync("ReceiveMessage", userId, message);
    }

    public async Task DeleteMessageAsync(Guid messageId)
    {
        var message = await _messageRepository.GetByIdAsync(messageId);
        if (message == null)
        {
            throw new NotFoundException("Message not found");
        }

        await _messageRepository.DeleteAsync(message);

        await Clients.Group(message.ChatRoomId.ToString()).SendAsync("DeleteMessage", messageId);
    }

    public async Task UpdateMessageAsync(Guid messageId, string newMessage)
    {
        var message = await _messageRepository.GetByIdAsync(messageId);
        if (message == null)
        {
            throw new NotFoundException("Message not found");
        }

        message.Content = newMessage;

        await _messageRepository.UpdateAsync(message);

        await Clients.Group(message.ChatRoomId.ToString()).SendAsync("UpdateMessage", messageId, newMessage);
    }
}
