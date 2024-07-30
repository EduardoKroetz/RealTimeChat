using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Application.Queries.GetUser;
using System.Security.Claims;

namespace RealTimeChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentAsync()
    {
        var userId = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var query = new GetUserQuery { UserId = userId };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

}
