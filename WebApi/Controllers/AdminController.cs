using Application.Features.Admin.Commands.UpdateUserStatus;
using Application.Features.Admin.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("admin")]
[Authorize(Roles = "Admin")]
public class AdminController(IMediator mediator) : ControllerBase
{
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] string? searchTerm)
    {
        var query = new GetUsersQuery { SearchTerm = searchTerm };
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("users/status")]
    public async Task<IActionResult> UpdateUserStatus([FromBody] UpdateUserStatusCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}
