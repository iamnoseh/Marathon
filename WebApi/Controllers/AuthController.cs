using Application.Features.Auth.Commands.LoginUser;
using Application.Features.Auth.Commands.LogoutUser;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.RegisterUser;
using Application.Features.Auth.Commands.UpdateProfile;
using Application.Features.Auth.Queries.GetUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutUserCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(Application.Constants.ClaimTypes.UserId);
        var query = new GetUserProfileQuery { UserId = userId ?? string.Empty };

        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [Authorize]
    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        var userId = User.FindFirstValue(Application.Constants.ClaimTypes.UserId);
        command.UserId = userId ?? string.Empty;

        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}
