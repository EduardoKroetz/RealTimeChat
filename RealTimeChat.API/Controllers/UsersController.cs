using MediatR;
using Microsoft.AspNetCore.Mvc;
using RealTimeChat.Application.Queries.GetUsers;

namespace RealTimeChat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] int pageSize, [FromQuery] int pageNumber)
    {
        var query = new GetUsersQuery { PageNumber = pageNumber, PageSize = pageSize};
        var result = await _mediator.Send(query);
        return Ok(result);
    }

}
