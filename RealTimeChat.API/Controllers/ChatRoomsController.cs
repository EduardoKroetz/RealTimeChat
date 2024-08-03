using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Application.Commands.CreateChatRoom;
using RealTimeChat.Application.Commands.DeleteChatRoom;
using RealTimeChat.Application.Commands.JoinChatRoom;
using RealTimeChat.Application.Commands.LeaveChatRoom;
using RealTimeChat.Application.Commands.UpdateChatRoom;
using RealTimeChat.Application.Queries.GetAllChatRooms;
using RealTimeChat.Application.Queries.GetChatRoom;
using RealTimeChat.Application.Queries.GetChatRoomsByName;
using RealTimeChat.Application.Queries.GetUserChatRooms;
using System.Security.Claims;

namespace RealTimeChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatRoomsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ChatRoomsController> _logger;

    public ChatRoomsController(IMediator mediator, ILogger<ChatRoomsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{chatRoomId:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid chatRoomId)
    {
        var query = new GetChatRoomQuery { ChatRoomId = chatRoomId };
        var result = await _mediator.Send(query);
        _logger.LogInformation($"Get Chat room {chatRoomId} successfully!");
        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetChatRoomsByNameAsync([FromQuery] string? name)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetChatRoomByNameQuery { Name = name ?? "", UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] int pageSize, [FromQuery] int pageNumber)
    {
        var query = new GetChatRoomsQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromQuery] string name)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new CreateChatRoomCommand { Name = name, UserId = userId };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var command = new DeleteChatRoomCommand { ChatRoomId = id };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdatedAsync([FromQuery] string name, [FromRoute] Guid id)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new UpdateChatRoomCommand { Id = id, Name = name, UserId = userId };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("join/{chatRoomId:guid}")]
    public async Task<IActionResult> JoinAsync([FromRoute] Guid chatRoomId)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new JoinChatRoomCommand { ChatRoomId = chatRoomId, UserId = userId };
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    [HttpDelete("leave/{chatRoomId:guid}")]
    public async Task<IActionResult> LeaveAsync([FromRoute] Guid chatRoomId)
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var command = new LeaveChatRoomCommand { ChatRoomId = chatRoomId, UserId = userId };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUserChatRooms()
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetUserChatRoomsQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
