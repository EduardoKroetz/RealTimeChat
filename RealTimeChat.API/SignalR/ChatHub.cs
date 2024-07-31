using Microsoft.AspNetCore.SignalR;
using RealTimeChat.Core.Entities;
using RealTimeChat.Core.Exceptions;
using RealTimeChat.Core.Repositories;

namespace RealTimeChat.API.SignalR;

public class ChatHub : Hub
{
    private readonly IMessageRepository _messageRepository;
    private readonly IRoomParticipantRepository _roomParticipantRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IMessageRepository messageRepository, IRoomParticipantRepository roomParticipantRepository, IUserRepository userRepository, ILogger<ChatHub> logger)
    {
        _messageRepository = messageRepository;
        _roomParticipantRepository = roomParticipantRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: " + Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public async Task JoinGroupAsync(Guid chatRoomId)
    {
        
        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId.ToString());
        _logger.LogInformation($"Client {Context.ConnectionId} JOINED group {chatRoomId}");
    }

    public async Task LeaveGroupAsync(Guid chatRoomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId.ToString());
        _logger.LogInformation($"Client {Context.ConnectionId} LEAVE group {chatRoomId}");
    }

    public async Task SendMessageAsync(Guid chatRoomId, Guid userId,  string message)
    {
        var chatMessage = new Message
        {
            ChatRoomId = chatRoomId,
            SenderId = userId,
            Content = message,
            Id = Guid.NewGuid(),
            Timestamp = DateTime.Now,
        };

        await _messageRepository.AddAsync(chatMessage);

        var user = await _userRepository.GetByIdAsync(userId) ?? throw new Exception("Sender not found");
        chatMessage.Sender = new User{ Id = user.Id, Email = user.Email, Username = user.Username, CreatedAt = user.CreatedAt };

        await Clients.Group(chatRoomId.ToString()).SendAsync("ReceiveMessage", chatMessage);
        _logger.LogInformation("Message sent successfully!");
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

        await Clients.Group(message.ChatRoomId.ToString()).SendAsync("UpdateMessage", message);
    }
}
